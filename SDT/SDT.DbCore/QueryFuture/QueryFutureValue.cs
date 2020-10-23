using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SDT.DbCore
{
    /// <summary>Class for query future value.</summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public class QueryFutureValue<TResult> : BaseQueryFuture
    {
        /// <summary>The result of the query future.</summary>
        private TResult _result;

        /// <summary>Constructor.</summary>
        /// <param name="ownerBatch">The batch that owns this item.</param>
        /// <param name="query">
        ///     The query to defer the execution and to add in the batch of future
        ///     queries.
        /// </param>
        public QueryFutureValue(QueryFutureBatch ownerBatch, IQueryable query)
        {
            OwnerBatch = ownerBatch;
            Query = query;
        }

        /// <summary>Gets the value of the future query.</summary>
        /// <value>The value of the future query.</value>
        public TResult Value
        {
            get
            {
                if (!HasValue)
                {
                    OwnerBatch.ExecuteQueries();
                }

                return _result;
            }
        }


        /// <summary>Gets the value of the future query.</summary>
        /// <value>The value of the future query.</value>
        public async Task<TResult> ValueAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!HasValue)
            {
                await OwnerBatch.ExecuteQueriesAsync(cancellationToken).ConfigureAwait(false);
            }

            return _result;
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

            var enumerator = GetQueryEnumerator<TResult>(reader);
            using (enumerator)
            {
                enumerator.MoveNext();
                _result = enumerator.Current;

            }

            // Enumerate on first item only

            HasValue = true;
        }



        public QueryDeferred<TResult> InMemoryDeferredQuery;

        public override void ExecuteInMemory()
        {
            if (InMemoryDeferredQuery != null)
            {
                _result = InMemoryDeferredQuery.Execute();
                HasValue = true;
            }
            else
            {

                var query = (IQueryable<TResult>)Query;
                var value = query.Provider.Execute<object>(query.Expression);

                if (value is TResult valueTResult)
                {
                    _result = valueTResult;
                }
                else if (value == null)
                {
                    _result = (TResult)value;
                }
                else if (value is IEnumerable<TResult> valueIEnumerable)
                {
                    using (var enumerator = valueIEnumerable.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        _result = enumerator.Current;
                    }
                }
                else
                {
                    throw new Exception($"Oops! The following return type is currently unsupported in the method GetResultDirectly: {value.GetType().FullName}. Please report this error to info@zzzprojects.com");
                }

                HasValue = true;
            }
        }

        public override void GetResultDirectly()
        {
            var query = (IQueryable<TResult>)Query;

            var value = query.Provider.Execute<object>(query.Expression);

            if (value is TResult valueTResult)
            {
                _result = valueTResult;
            }
            else if (value == null)
            {
                _result = (TResult)value;
            }
            else if (value is IEnumerable<TResult> valueIEnumerable)
            {
                using (var enumerator = valueIEnumerable.GetEnumerator())
                {
                    enumerator.MoveNext();
                    _result = enumerator.Current;
                }
            }
            else
            {
                throw new Exception($"Oops! The following return type is currently unsupported in the method GetResultDirectly: {value.GetType().FullName}. Please report this error to info@zzzprojects.com");
            }

            HasValue = true;
        }

        public override Task GetResultDirectlyAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GetResultDirectly();
            return Task.FromResult(0);
        }

        internal void GetResultDirectly(IQueryable<TResult> query)
        {
            var value = query.Provider.Execute<TResult>(query.Expression);

            _result = value;
            HasValue = true;
        }

        /// <summary>
        /// Performs an implicit conversion from QueryFutureValue to TResult.
        /// </summary>
        /// <param name="futureValue">The future value.</param>
        /// <returns>The result of forcing this lazy value.</returns>
        public static implicit operator TResult(QueryFutureValue<TResult> futureValue)
        {
            if (futureValue == null)
            {
                return default;
            }

            return futureValue.Value;
        }
    }
}