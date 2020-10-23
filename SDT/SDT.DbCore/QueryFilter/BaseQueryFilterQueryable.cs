using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SDT.DbCore
{
    public abstract class BaseQueryFilterQueryable
    {
        #region Members
        public DbContext Context { get; protected internal set; }

        public List<BaseQueryFilter> Filters { get; protected internal set; }

        public QueryFilterSet FilterSet { get; protected internal set; }

        public object OriginalQuery { get; protected internal set; }
        #endregion

        #region Methods
        public void DisableFilter(BaseQueryFilter filter)
        {
            if (Filters.Remove(filter))
            {
                UpdateInternalQuery();
            }
        }

        public void EnableFilter(BaseQueryFilter filter)
        {
            if (!Filters.Contains(filter))
            {
                Filters.Add(filter);
                UpdateInternalQuery();
            }
        }

        public abstract void UpdateInternalQuery();
        #endregion
    }
}
