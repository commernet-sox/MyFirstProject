using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore.QueryFuture
{
    /// <summary>Interface for QueryFuture class.</summary>
    public abstract class BaseQueryFuture
    {
        /// <summary>Gets the value indicating whether the query future has a value.</summary>
        /// <value>true if this query future has a value, false if not.</value>
        public bool HasValue { get; protected set; }

        /// <summary>Gets or sets the batch that owns the query future.</summary>
        /// <value>The batch that owns the query future.</value>
        public QueryFutureBatch OwnerBatch { get; set; }

        /// <summary>Gets or sets the query deferred.</summary>
        /// <value>The query deferred.</value>
        public IQueryable Query { get; set; }

        /// <summary>Gets or sets the query deferred executor.</summary>
        /// <value>The query deferred executor.</value>
        public object QueryExecutor { get; set; }

        /// <summary>Gets or sets a context for the query deferred.</summary>
        /// <value>The query deferred context.</value>
        public QueryContext QueryContext { get; set; }

        /// <summary>Gets or sets the query connection.</summary>
        /// <value>The query connection.</value>
        internal IRelationalConnection QueryConnection { get; set; }

        internal object CompiledQuery { get; set; }

        internal Action RestoreConnection { get; set; }

        public virtual void ExecuteInMemory()
        {

        }

        /// <summary>Creates executor and get command.</summary>
        /// <returns>The new executor and get command.</returns>
        public virtual IRelationalCommand CreateExecutorAndGetCommand(out RelationalQueryContext queryContext)
        {

            var relationalCommand = Query.CreateCommand(queryable =>
            {
                var context = queryable.GetDbContext();

                QueryConnection = context.Database.GetService<IRelationalConnection>();

                var innerConnection = new CreateEntityConnection(QueryConnection.DbConnection, null);
                var innerConnectionField = typeof(RelationalConnection).GetField("_connection", BindingFlags.NonPublic | BindingFlags.Instance);
                var initalConnection = innerConnectionField.GetValue(QueryConnection);

                innerConnectionField.SetValue(QueryConnection, innerConnection);

                RestoreConnection = () => innerConnectionField.SetValue(QueryConnection, initalConnection);
            }, out queryContext, out var compiledQueryOut);

            QueryContext = queryContext;
            CompiledQuery = compiledQueryOut;

            return relationalCommand;
        }


        /// <summary>Sets the result of the query deferred.</summary>
        /// <param name="reader">The reader returned from the query execution.</param>
        public virtual void SetResult(DbDataReader reader)
        {
        }

        /// <summary>Sets the result of the query deferred.</summary>
        /// <param name="reader">The reader returned from the query execution.</param>
        public IEnumerator<T> GetQueryEnumerator<T>(DbDataReader reader)
        {
            ((CreateEntityConnection)QueryConnection.DbConnection).OriginalDataReader = reader;
            var compiledQuery = CompiledQuery;
            var getEnumeratorMethod = compiledQuery.GetType().GetMethod("GetEnumerator", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var getEnumerator = getEnumeratorMethod.Invoke(compiledQuery, new object[0]);
            var enumerator = (IEnumerator<T>)getEnumerator;
            {
                var fielReaderColumns = enumerator.GetType().GetField("_readerColumns", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (fielReaderColumns != null)
                {
                    fielReaderColumns.SetValue(enumerator, null);
                }
            }

            return enumerator;
        }

        public virtual void GetResultDirectly()
        {

        }

        public virtual Task GetResultDirectlyAsync(CancellationToken cancellationToken) => throw new Exception("Not implemented");

    }
}