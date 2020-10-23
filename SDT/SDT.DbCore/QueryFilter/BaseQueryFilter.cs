using System;

namespace SDT.DbCore
{
    public abstract class BaseQueryFilter
    {
        #region Members
        public Type ElementType { get; protected internal set; }

        public bool IsDefaultEnabled { get; protected internal set; }

        public QueryFilterContext OwnerFilterContext { get; protected internal set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods
        public abstract object ApplyFilter<TEntity>(object query);
        public abstract object GetFilter();
        public abstract BaseQueryFilter Clone(QueryFilterContext filterContext);


        public void Disable() => Disable(null);

        public void Disable<T>() => Disable(typeof(T));

        public void Disable(params Type[] types)
        {
            if (OwnerFilterContext == null)
            {
                QueryFilterManager.GlobalInitializeFilterActions.Add(new Tuple<BaseQueryFilter, Action<BaseQueryFilter>>(this, filter => filter.Disable(types)));
            }
            else
            {
                OwnerFilterContext.DisableFilter(this, types);
            }
        }

        public void Enable() => Enable(null);

        public void Enable<T>() => Enable(typeof(T));

        public void Enable(params Type[] types)
        {
            if (OwnerFilterContext == null)
            {
                QueryFilterManager.GlobalInitializeFilterActions.Add(new Tuple<BaseQueryFilter, Action<BaseQueryFilter>>(this, filter => filter.Enable(types)));
            }
            else
            {
                OwnerFilterContext.EnableFilter(this, types);
            }
        }
        #endregion
    }
}
