using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore
{
    public class QueryDeferred<T>
    {
        #region Members
        public IQueryable<T> Query { get; protected internal set; }

        public Expression Expression { get; protected internal set; }
        #endregion

        #region Constructors
        public QueryDeferred(IQueryable query, Expression expression)
        {
            Expression = expression;
            // EF Core 3.x
            Query = new EntityQueryable<T>((IAsyncQueryProvider)query.Provider, Expression);
        }
        #endregion

        #region Methods
        public T Execute() => Query.Provider.Execute<T>(Expression);

        public async ValueTask<T> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (!(Query.Provider is IAsyncQueryProvider asyncQueryProvider))
            {
                return Execute();
            }

            return await Task.Run(() => { return asyncQueryProvider.ExecuteAsync<T>(Expression, cancellationToken); }, cancellationToken);
        }

        #endregion
    }
}
