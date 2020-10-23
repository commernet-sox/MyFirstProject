using System.Linq;

namespace SDT.DbCore
{
    public static class QueryFutureExtensions
    {
        /// <summary>
        ///     Defer the execution of the <paramref name="query" /> and batch the query command with other
        ///     future queries. The batch is executed when a future query requires a database round trip.
        /// </summary>
        /// <typeparam name="T">The type of elements of the query.</typeparam>
        /// <param name="query">
        ///     The query to defer the execution of and to add in the batch of future
        ///     queries.
        /// </param>
        /// <returns>
        ///     The QueryFutureEnumerable&lt;TEntity&gt; added to the batch of futures queries.
        /// </returns>
        public static QueryFutureEnumerable<T> Future<T>(this IQueryable<T> query)
        {
            if (!QueryFutureManager.AllowQueryBatch)
            {
                var queryFuture = new QueryFutureEnumerable<T>(null, null);
                queryFuture.GetResultDirectly(query);
                return queryFuture;
            }

            QueryFutureBatch futureBatch;
            QueryFutureEnumerable<T> futureQuery;
            if (query.IsInMemoryQueryContext())
            {
                var context = query.GetInMemoryContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureBatch.IsInMemory = true;
                futureQuery = new QueryFutureEnumerable<T>(futureBatch, query);
            }
            else
            {
                var context = query.GetDbContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureQuery = new QueryFutureEnumerable<T>(futureBatch, query);
            }

            futureBatch.Queries.Add(futureQuery);

            return futureQuery;
        }

        /// <summary>
        ///     Defer the execution of the <paramref name="query" /> and batch the query command with other
        ///     future queries. The batch is executed when a future query requires a database round trip.
        /// </summary>
        /// <typeparam name="TResult">The type of the query result.</typeparam>
        /// <param name="query">
        ///     The query to defer the execution of and to add in the batch of future
        ///     queries.
        /// </param>
        /// <returns>
        ///     The QueryFutureValue&lt;TResult,TResult&gt; added to the batch of futures queries.
        /// </returns>
        public static QueryFutureValue<TResult> FutureValue<TResult>(this IQueryable<TResult> query)
        {
            if (!QueryFutureManager.AllowQueryBatch)
            {
                var futureValue = new QueryFutureValue<TResult>(null, null);
                futureValue.GetResultDirectly(query);
                return futureValue;
            }

            QueryFutureBatch futureBatch;
            QueryFutureValue<TResult> futureQuery;
            if (query.IsInMemoryQueryContext())
            {
                var context = query.GetInMemoryContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureBatch.IsInMemory = true;
                futureQuery = new QueryFutureValue<TResult>(futureBatch, query);
            }
            else
            {
                var context = query.GetDbContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureQuery = new QueryFutureValue<TResult>(futureBatch, query);
            }

            futureBatch.Queries.Add(futureQuery);

            return futureQuery;
        }

        /// <summary>
        ///     Defer the execution of the <paramref name="query" /> and batch the query command with other
        ///     future queries. The batch is executed when a future query requires a database round trip.
        /// </summary>
        /// <typeparam name="TResult">The type of the query result.</typeparam>
        /// <param name="query">The query to defer the execution and to add in the batch of future queries.</param>
        /// <returns>
        ///     The QueryFutureValue&lt;TResult,TResult&gt; added to the batch of futures queries.
        /// </returns>
        public static QueryFutureValue<TResult> FutureValue<TResult>(this QueryDeferred<TResult> query)
        {
            if (!QueryFutureManager.AllowQueryBatch)
            {
                var futureValue = new QueryFutureValue<TResult>(null, null);
                futureValue.GetResultDirectly(query.Query);
                return futureValue;
            }

            QueryFutureBatch futureBatch;
            QueryFutureValue<TResult> futureQuery;
            if (query.Query.IsInMemoryQueryContext())
            {
                var context = query.Query.GetInMemoryContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureBatch.IsInMemory = true;
                futureQuery = new QueryFutureValue<TResult>(futureBatch, query.Query)
                {
                    InMemoryDeferredQuery = query
                };
            }
            else
            {
                var context = query.Query.GetDbContext();
                futureBatch = QueryFutureManager.AddOrGetBatch(context);
                futureQuery = new QueryFutureValue<TResult>(futureBatch, query.Query);
            }
            futureBatch.Queries.Add(futureQuery);

            return futureQuery;
        }
    }
}
