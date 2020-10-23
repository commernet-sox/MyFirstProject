using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using SDT.BaseTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SDT.DbCore
{
    /// <summary>
    /// doc to https://github.com/snickler/EFCore-FluentStoredProcedure
    /// </summary>
    public static class DbSqlExtensions
    {
        #region Database
        public static void SqlQuery(this DatabaseFacade databaseFacade, string sql, Action<SqlResult> handleResults, params object[] parameters)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            var dependencies = ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Dependencies;

            if (!(dependencies is IRelationalDatabaseFacadeDependencies facadeDependencies))
            {
                throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
            }

            var concurrencyDetector = facadeDependencies.ConcurrencyDetector;
            var logger = facadeDependencies.CommandLogger;

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = facadeDependencies.RawSqlCommandBuilder
                    .Build(sql, parameters);

                var dbReader = rawSqlCommand.RelationalCommand.ExecuteReader(new RelationalCommandParameterObject(
                             facadeDependencies.RelationalConnection,
                             rawSqlCommand.ParameterValues,
                             null,
                             ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context,
                             logger)).DbDataReader;
                var sqlResult = new SqlResult(dbReader);
                handleResults(sqlResult);
            }
        }
        #endregion

        #region Db
        public static DbCommand FromSql(this DbContext context, string sql, short commandTimeout = 30)
        {
            var connect = context.Database.GetDbConnection();
            return connect.FromSql(sql, commandTimeout);
        }

        public static DbCommand FromSql(this DbConnection connect, string sql, short commandTimeout = 30)
        {
            var cmd = connect.CreateCommand();
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return cmd;
        }

        /// <summary>
        /// Creates an initial DbCommand object based on a stored procedure name
        /// </summary>
        /// <param name="context">target database context</param>
        /// <param name="storedProcName">target procedure name</param>
        /// <param name="prependDefaultSchema">Prepend the default schema name to <paramref name="storedProcName"/> if explicitly defined in <paramref name="context"/></param>
        /// <param name="commandTimeout">Command timeout in seconds. Default is 30.</param>
        /// <returns></returns>
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName, bool prependDefaultSchema = true, short commandTimeout = 30)
        {
            var schemaName = string.Empty;
            if (prependDefaultSchema)
            {
                schemaName = context.Model.GetDefaultSchema();
            }

            var connect = context.Database.GetDbConnection();
            var cmd = connect.LoadStoredProc(storedProcName, commandTimeout, schemaName);

            return cmd;
        }

        /// <summary>
        /// Creates an initial DbCommand object based on a stored procedure name
        /// </summary>
        /// <param name="context">target database connection</param>
        /// <param name="storedProcName">target procedure name</param>
        /// <param name="schemaName">schema name to <paramref name="storedProcName"/> if explicitly defined in <paramref name="context"/></param>
        /// <param name="commandTimeout">Command timeout in seconds. Default is 30.</param>
        /// <returns></returns>
        public static DbCommand LoadStoredProc(this DbConnection context, string storedProcName, short commandTimeout = 30, string schemaName = "")
        {
            var cmd = context.CreateCommand();
            cmd.CommandTimeout = commandTimeout;

            if (!schemaName.IsNull())
            {
                storedProcName = $"{schemaName}.{storedProcName}";
            }

            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }
        #endregion

        #region DbCommand
        /// <summary>
        /// Creates a DbParameter object and adds it to a DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue, Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                throw new InvalidOperationException("Call LoadStoredProc before using this method");
            }

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue ?? DBNull.Value;
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        /// <summary>
        /// Creates a DbParameter object and adds it to a DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                throw new InvalidOperationException("Call LoadStoredProc before using this method");
            }

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        /// <summary>
        /// Creates a DbParameter object based on the SqlParameter and adds it to a DbCommand.
        /// This enabled the ability to provide custom types for SQL-parameters.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                throw new InvalidOperationException("Call LoadStoredProc before using this method");
            }

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            return cmd;
        }

        public static DbCommand WithTimeout(this DbCommand command, int timeout)
        {
            command.CommandTimeout = timeout;
            return command;
        }
        #endregion

        #region DbTranstion
        public static DbCommand BeginTrans(this DbCommand cmd, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            var trans = cmd.Connection.BeginTransaction(level);
            cmd.Transaction = trans;
            return cmd;
        }
        #endregion

        #region Execute
        public static void Execute(this DbCommand command, Action<DbCommand> action, bool manageConnection = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                try
                {
                    action.Invoke(command);
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        public static void Read(this DbCommand command, Action<DbDataReader> action, CommandBehavior commandBehaviour = CommandBehavior.Default, bool manageConnection = true) => command.Execute(c =>
                                                                                                                                                                                {
                                                                                                                                                                                    using (var reader = command.ExecuteReader(commandBehaviour))
                                                                                                                                                                                    {
                                                                                                                                                                                        action?.Invoke(reader);
                                                                                                                                                                                    }

                                                                                                                                                                                }, manageConnection);

        public static int Execute(this DbCommand command, bool manageConnection = true)
        {
            var result = 0;
            command.Execute(c => result = c.ExecuteNonQuery(), manageConnection);
            return result;
        }

        public static object Scalar(this DbCommand command, bool manageConnection = true)
        {
            object result = null;
            command.Execute(c => result = c.ExecuteScalar(), manageConnection);
            return result;
        }

        public static IList<T> Query<T>(this DbCommand command, CommandBehavior commandBehaviour = CommandBehavior.Default, bool manageConnection = true)
        {
            IList<T> list = new List<T>();
            command.Query(t => list = t.ReadToList<T>(), commandBehaviour, manageConnection);
            return list;
        }

        public static void Query(this DbCommand command, Action<SqlResult> handleResults, CommandBehavior commandBehaviour = CommandBehavior.Default, bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            command.Read(r =>
            {
                var sqlResult = new SqlResult(r);
                handleResults?.Invoke(sqlResult);
            }, commandBehaviour, manageConnection);
        }

        [Obsolete("不推荐使用，后续将删除")]
        public static DbDataAdapter CreateAdapter(this DbCommand command, IDataProvider provider)
        {
            if (provider.IsNull())
            {
                throw new ArgumentNullException("provider");
            }

            return provider.DataAdapter(command);
        }

        [Obsolete("不推荐使用，后续将删除")]
        public static DataTable QueryTable(this DbCommand command, IDataProvider provider, string tableName = "table")
        {
            using (var da = command.CreateAdapter(provider))
            {
                var dt = new DataTable(tableName);
                da.Fill(dt);
                return dt;
            }
        }

        [Obsolete("不推荐使用，后续将删除")]
        public static DataSet Query(this DbCommand command, IDataProvider provider)
        {
            using (var da = command.CreateAdapter(provider))
            {
                var ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public static IList<object> Query(this DbCommand command, CommandBehavior commandBehaviour = CommandBehavior.Default, bool manageConnection = true)
        {
            IList<object> list = new List<object>();

            command.Read(reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(reader[0]);
                    }
                }
            }, commandBehaviour, manageConnection);

            return list;
        }
        #endregion
    }
}
