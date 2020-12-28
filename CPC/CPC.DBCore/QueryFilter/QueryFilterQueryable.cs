using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CPC.DBCore.QueryFilter
{
    public class QueryFilterQueryable<T> : BaseQueryFilterQueryable
    {
        #region Constructors
        public QueryFilterQueryable(DbContext context, QueryFilterSet filterSet, IQueryable<T> originalQuery)
        {
            Context = context;
            Filters = new List<BaseQueryFilter>();
            FilterSet = filterSet;
            OriginalQuery = originalQuery;
        }
        #endregion

        #region Methods
        public override void UpdateInternalQuery()
        {
            var query = OriginalQuery;

            foreach (var filter in Filters)
            {
                query = filter.ApplyFilter<T>(query);
            }

            FilterSet.UpdateInternalQuery(Context, query);
        }
        #endregion
    }
}
