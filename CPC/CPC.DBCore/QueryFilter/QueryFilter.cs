using System;
using System.Linq;

namespace CPC.DBCore.QueryFilter
{
    public class QueryFilter<T> : BaseQueryFilter
    {

        #region Members
        public Func<IQueryable<T>, IQueryable<T>> Filter { get; protected internal set; }
        #endregion
        #region Constructors
        public QueryFilter(QueryFilterContext ownerFilterContext, Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ElementType = typeof(T);
            Filter = filter;
            OwnerFilterContext = ownerFilterContext;
        }
        #endregion

        #region Methods
        public override object ApplyFilter<TEntity>(object query)
        {
            if (QueryFilterManager.ForceCast)
            {
                return Filter((IQueryable<T>)query).Cast<TEntity>();
            }

            return Filter((IQueryable<T>)query);
        }

        public override object GetFilter() => Filter;

        public override BaseQueryFilter Clone(QueryFilterContext filterContext) => new QueryFilter<T>(filterContext, Filter);
        #endregion
    }
}
