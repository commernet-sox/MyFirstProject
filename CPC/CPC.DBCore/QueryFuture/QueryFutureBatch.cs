using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CPC.DBCore.QueryFuture
{
    /// <summary>Class to own future queries in a batch</summary>
    public class QueryFutureBatch
    {
        /// <summary>Constructor.</summary>
        /// <param name="context">The context related to the query future batched.</param>
        public QueryFutureBatch(DbContext context)
        {
            Context = context;
            Queries = new List<BaseQueryFuture>();
        }

        /// <summary>Gets or sets the context related to the query future batched.</summary>
        /// <value>The context related to the query future batched.</value>
        public DbContext Context { get; set; }

        public bool IsInMemory { get; set; }

        /// <summary>Gets or sets deferred query lists waiting to be executed.</summary>
        /// <value>The deferred queries list waiting to be executed.</value>
        public List<BaseQueryFuture> Queries { get; set; }

        /// <summary>Executes deferred query lists.</summary>
        public void ExecuteQueries()
        {
            if (Queries.Count == 0)
            {
                // Already all executed
                return;
            }

            if (IsInMemory)
            {
                foreach (var query in Queries)
                {
                    query.ExecuteInMemory();
                }
                Queries.Clear();
                return;
            }

            if (Queries.Count == 1)
            {
                Queries[0].GetResultDirectly();
                Queries.Clear();
                return;
            }

            var allowQueryBatch = QueryFutureManager.AllowQueryBatch;

            var databaseCreator = Context.Database.GetService<IDatabaseCreator>();

            var assembly = databaseCreator.GetType().GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;

            // We deactivated temporary some QueryFuture for EF Core as they don't work correctly            
            // We need to still make them "work" for IncludeFilter feature
            var isMySqlPomelo = assemblyName == "Pomelo.EntityFrameworkCore.MySql";
            var isOracle = assemblyName == "Oracle.EntityFrameworkCore" || assemblyName == "Devart.Data.Oracle.Entity.EFCore";
            if (allowQueryBatch && (isOracle || isMySqlPomelo))
            {
                allowQueryBatch = false;
            }


            if (!allowQueryBatch)
            {
                foreach (var query in Queries)
                {
                    query.GetResultDirectly();
                }

                Queries.Clear();
                return;
            }

            if (IsInMemory)
            {
                foreach (var query in Queries)
                {
                    query.ExecuteInMemory();
                }
                return;
            }

            var connection = Context.Database.GetDbConnection();

            var firstQuery = Queries[0];

            var command = CreateCommandCombined();

            var ownConnection = false;

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    ownConnection = true;
                }

                using (command)
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var createEntityDataReader = new CreateEntityDataReader(reader);
                        foreach (var query in Queries)
                        {
                            query.SetResult(createEntityDataReader);
                            reader.NextResult();
                        }
                    }
                }

                Queries.Clear();
            }
            finally
            {
                if (ownConnection)
                {
                    connection.Close();
                }
            }

            firstQuery.RestoreConnection?.Invoke();
        }


        /// <summary>Executes deferred query lists.</summary>
        public async Task ExecuteQueriesAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Queries.Count == 0)
            {
                // Already all executed
                return;
            }

            if (IsInMemory)
            {
                foreach (var query in Queries)
                {
                    query.ExecuteInMemory();
                }
                Queries.Clear();
                return;
            }

            if (Queries.Count == 1)
            {
                await Queries[0].GetResultDirectlyAsync(cancellationToken).ConfigureAwait(false);
                Queries.Clear();
                return;
            }


            if (IsInMemory)
            {
                foreach (var query in Queries)
                {
                    query.ExecuteInMemory();
                }
                return;
            }

            var connection = Context.Database.GetDbConnection();

            var firstQuery = Queries[0];

            var command = CreateCommandCombined();

            var ownConnection = false;

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                    ownConnection = true;
                }

                using (command)
                {
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                    {
                        var createEntityDataReader = new CreateEntityDataReader(reader);
                        foreach (var query in Queries)
                        {
                            query.SetResult(createEntityDataReader);
                            await reader.NextResultAsync(cancellationToken).ConfigureAwait(false);
                        }
                    }
                }

                Queries.Clear();
            }
            finally
            {
                if (ownConnection)
                {
                    connection.Close();
                }
            }

            firstQuery.RestoreConnection?.Invoke();

        }

        /// <summary>Creates a new command combining deferred queries.</summary>
        /// <returns>The combined command created from deferred queries.</returns>
        protected DbCommand CreateCommandCombined()
        {
            var command = Context.CreateStoreCommand();

            var sb = new StringBuilder();
            var queryCount = 1;

            var isOracle = command.GetType().FullName.Contains("Oracle.DataAccess");
            var isOracleManaged = command.GetType().FullName.Contains("Oracle.ManagedDataAccess");
            var isOracleDevArt = command.GetType().FullName.Contains("Devart");

            foreach (var query in Queries)
            {
                // GENERATE SQL

                var queryCommand = query.CreateExecutorAndGetCommand(out var queryContext);
                var sql = queryCommand.CommandText;
                var parameters = queryCommand.Parameters;

                // UPDATE parameter name
                foreach (var relationalParameter in queryCommand.Parameters)
                {
                    var parameter = queryContext.ParameterValues[relationalParameter.InvariantName];

                    var oldValue = relationalParameter.InvariantName;
                    var newValue = string.Concat("Z_", queryCount, "_", oldValue);

                    // CREATE parameter
                    var dbParameter = command.CreateParameter();
                    dbParameter.CopyFrom(relationalParameter, parameter, newValue);

                    command.Parameters.Add(dbParameter);

                    // REPLACE parameter with new value
                    if (isOracle || isOracleManaged || isOracleDevArt)
                    {
                        sql = sql.Replace(":" + oldValue, ":" + newValue);
                    }
                    else
                    {
                        sql = sql.Replace("@" + oldValue, "@" + newValue);
                    }
                }

                sb.AppendLine(string.Concat("-- EF+ Query Future: ", queryCount, " of ", Queries.Count));

                if (isOracle || isOracleManaged || isOracleDevArt)
                {
                    var parameterName = "zzz_cursor_" + queryCount;
                    sb.AppendLine("open :" + parameterName + " for " + sql);
                    var param = command.CreateParameter();
                    param.ParameterName = parameterName;
                    param.Direction = ParameterDirection.Output;
                    param.Value = DBNull.Value;

                    if (isOracle)
                    {
                        SetOracleDbType(command.GetType().Assembly, param, 121);
                    }
                    else if (isOracleManaged)
                    {
                        SetOracleManagedDbType(command.GetType().Assembly, param, 121);
                    }
                    else if (isOracleDevArt)
                    {
                        SetOracleDevArtDbType(command.GetType().Assembly, param, 7);
                    }


                    command.Parameters.Add(param);
                }
                else
                {
                    sb.AppendLine(sql);
                }


                sb.Append(";"); // SQL Server, SQL Azure, MySQL
                sb.AppendLine();
                sb.AppendLine();

                queryCount++;
            }

            command.CommandText = sb.ToString();

            if (isOracle || isOracleManaged || isOracleDevArt)
            {
                var bindByNameProperty = command.GetType().GetProperty("BindByName") ?? command.GetType().GetProperty("PassParametersByName");
                bindByNameProperty.SetValue(command, true, null);

                command.CommandText = "BEGIN" + Environment.NewLine + command.CommandText + Environment.NewLine + "END;";
            }

            return command;
        }

        private static Action<DbParameter, object> _setOracleDbType;
        private static Action<DbParameter, object> _setOracleManagedDbType;
        private static Action<DbParameter, object> _setOracleDevArtDbType;

        public static void SetOracleManagedDbType(Assembly assembly, DbParameter dbParameter, object type)
        {
            if (_setOracleManagedDbType == null)
            {
                var dbtype = assembly.GetType("Oracle.ManagedDataAccess.Client.OracleDbType");
                var dbParameterType = assembly.GetType("Oracle.ManagedDataAccess.Client.OracleParameter");
                var propertyInfo = dbParameter.GetType().GetProperty("OracleDbType");

                var parameter = Expression.Parameter(typeof(DbParameter));
                var parameterConvert = Expression.Convert(parameter, dbParameterType);
                var parameterValue = Expression.Parameter(typeof(object));
                var parameterValueConvert = Expression.Convert(parameterValue, dbtype);

                var property = Expression.Property(parameterConvert, propertyInfo);
                var expression = Expression.Assign(property, parameterValueConvert);

                _setOracleManagedDbType = Expression.Lambda<Action<DbParameter, object>>(expression, parameter, parameterValue).Compile();
            }

            _setOracleManagedDbType(dbParameter, type);
        }

        public static void SetOracleDbType(Assembly assembly, DbParameter dbParameter, object type)
        {
            if (_setOracleDbType == null)
            {
                var dbtype = assembly.GetType("Oracle.DataAccess.Client.OracleDbType");
                var dbParameterType = assembly.GetType("Oracle.DataAccess.Client.OracleParameter");
                var propertyInfo = dbParameter.GetType().GetProperty("OracleDbType");

                var parameter = Expression.Parameter(typeof(DbParameter));
                var parameterConvert = Expression.Convert(parameter, dbParameterType);
                var parameterValue = Expression.Parameter(typeof(object));
                var parameterValueConvert = Expression.Convert(parameterValue, dbtype);

                var property = Expression.Property(parameterConvert, propertyInfo);
                var expression = Expression.Assign(property, parameterValueConvert);

                _setOracleDbType = Expression.Lambda<Action<DbParameter, object>>(expression, parameter, parameterValue).Compile();
            }

            _setOracleDbType(dbParameter, type);
        }

        public static void SetOracleDevArtDbType(Assembly assembly, DbParameter dbParameter, object type)
        {
            if (_setOracleDevArtDbType == null)
            {
                var dbtype = assembly.GetType("Devart.Data.Oracle.OracleDbType");
                var dbParameterType = assembly.GetType("Devart.Data.Oracle.OracleParameter");
                var propertyInfo = dbParameter.GetType().GetProperty("OracleDbType");

                var parameter = Expression.Parameter(typeof(DbParameter));
                var parameterConvert = Expression.Convert(parameter, dbParameterType);
                var parameterValue = Expression.Parameter(typeof(object));
                var parameterValueConvert = Expression.Convert(parameterValue, dbtype);

                var property = Expression.Property(parameterConvert, propertyInfo);
                var expression = Expression.Assign(property, parameterValueConvert);

                _setOracleDevArtDbType = Expression.Lambda<Action<DbParameter, object>>(expression, parameter, parameterValue).Compile();
            }

            _setOracleDevArtDbType(dbParameter, type);
        }
    }
}