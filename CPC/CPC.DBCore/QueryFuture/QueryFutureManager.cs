using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace CPC.DBCore.QueryFuture
{
    /// <summary>Manage EF+ Query Future Configuration.</summary>
    public static class QueryFutureManager

    {
        /// <summary>Static constructor.</summary>
        static QueryFutureManager() => CacheWeakFutureBatch = new ConditionalWeakTable<DbContext, QueryFutureBatch>();

        /// <summary>Gets or sets a value indicating whether we allow query batch.</summary>
        /// <value>True if allow query batch, false if not.</value>
        public static bool AllowQueryBatch { get; set; } = true;

        /// <summary>Gets or sets the weak table used to cache future batch associated to a context.</summary>
        /// <value>The weak table used to cache future batch associated to a context.</value>
        public static ConditionalWeakTable<DbContext, QueryFutureBatch> CacheWeakFutureBatch { get; set; }

        /// <summary>Adds or gets the future batch associated to the context.</summary>
        /// <param name="context">The context used to cache the future batch.</param>
        /// <returns>The future batch associated to the context.</returns>
        public static QueryFutureBatch AddOrGetBatch(DbContext context)
        {
            if (!CacheWeakFutureBatch.TryGetValue(context, out var futureBatch))
            {
                futureBatch = new QueryFutureBatch(context);
                CacheWeakFutureBatch.Add(context, futureBatch);
            }

            return futureBatch;
        }

        public static void ExecuteBatch(DbContext context)
        {
            var batch = AddOrGetBatch(context);
            batch.ExecuteQueries();
        }
    }
}