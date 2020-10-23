using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    /// <summary>Class for query future value.</summary>
    /// <typeparam name="T">The type of elements of the query.</typeparam>
    public class QueryFutureEnumerable<T> : BaseQueryFuture, IEnumerable<T>
    {
        /// <summary>The result of the query future.</summary>
        private IEnumerable<T> _result;

        /// <summary>Constructor.</summary>
        /// <param name="ownerBatch">The batch that owns this item.</param>
        /// <param name="query">
        ///     The query to defer the execution and to add in the batch of future
        ///     queries.
        /// </param>
        public QueryFutureEnumerable(QueryFutureBatch ownerBatch, IQueryable query)
        {
            OwnerBatch = ownerBatch;
            Query = query;
        }

        /// <summary>Gets the enumerator of the query future.</summary>
        /// <returns>The enumerator of the query future.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (!HasValue)
            {
                OwnerBatch.ExecuteQueries();
            }

            if (_result == null)
            {
                return new List<T>().GetEnumerator();
            }

            return _result.GetEnumerator();
        }


        /// <summary>Gets the enumerator of the query future.</summary>
        /// <returns>The enumerator of the query future.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!HasValue)
            {
                await OwnerBatch.ExecuteQueriesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (_result == null)
            {
                return new List<T>();
            }

            using (var enumerator = _result.GetEnumerator())
            {
                var list = new List<T>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }
                return list;
            }
        }


        public async Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!HasValue)
            {
                await OwnerBatch.ExecuteQueriesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (_result == null)
            {
                return new T[0];
            }

            using (var enumerator = _result.GetEnumerator())
            {
                var list = new List<T>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }
                return list.ToArray();
            }
        }

        /// <summary>Sets the result of the query deferred.</summary>
        /// <param name="reader">The reader returned from the query execution.</param>
        public override void SetResult(DbDataReader reader)
        {
            if (reader.GetType().FullName.Contains("Oracle"))
            {
                var reader2 = new QueryFutureOracleDbReader(reader);
                reader = reader2;
            }

            var enumerator = GetQueryEnumerator<T>(reader);

            using (enumerator)
            {
                SetResult(enumerator);
            }
        }

        public void SetResult(IEnumerator<T> enumerator)
        {
            // Enumerate on all items
            var list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            _result = list;

            HasValue = true;
        }


        public override void ExecuteInMemory()
        {
            HasValue = true;
            _result = ((IQueryable<T>)Query).ToList();
        }

        public override void GetResultDirectly()
        {
            var query = ((IQueryable<T>)Query);

            GetResultDirectly(query);
        }

        public override Task GetResultDirectlyAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = ((IQueryable<T>)Query);
            GetResultDirectly(query);

            return Task.FromResult(0);
        }

        internal void GetResultDirectly(IQueryable<T> query)
        {
            using (var enumerator = query.GetEnumerator())
            {
                SetResult(enumerator);
            }
        }
    }
}