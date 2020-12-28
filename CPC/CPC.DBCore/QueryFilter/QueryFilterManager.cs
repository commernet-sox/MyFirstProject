using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CPC.DBCore.QueryFilter
{
    public static class QueryFilterManager
    {
        #region Members
        private static readonly object GenericFilterContextLock = new object();

        public static bool ForceCast { get; set; }

        public static Dictionary<string, BaseQueryFilter> GlobalFilters { get; } = new Dictionary<string, BaseQueryFilter>();

        public static List<Tuple<BaseQueryFilter, Action<BaseQueryFilter>>> GlobalInitializeFilterActions { get; private set; } = new List<Tuple<BaseQueryFilter, Action<BaseQueryFilter>>>();

        public static Dictionary<string, QueryFilterContext> CacheGenericFilterContext { get; private set; } = new Dictionary<string, QueryFilterContext>();

        public static ConditionalWeakTable<DbContext, QueryFilterContext> CacheWeakFilterContext { get; private set; } = new ConditionalWeakTable<DbContext, QueryFilterContext>();

        public static ConditionalWeakTable<IQueryable, BaseQueryFilterQueryable> CacheWeakFilterQueryable { get; private set; } = new ConditionalWeakTable<IQueryable, BaseQueryFilterQueryable>();
        #endregion

        #region Constructors

        #endregion

        #region Methods
        public static QueryFilterContext AddOrGetGenericFilterContext(DbContext context)
        {
            var key = context.GetType().FullName;

            if (!CacheGenericFilterContext.TryGetValue(key, out var filterContext))
            {
                lock (GenericFilterContextLock)
                {
                    if (!CacheGenericFilterContext.TryGetValue(key, out filterContext))
                    {
                        filterContext = new QueryFilterContext(context, true);
                        CacheGenericFilterContext.Add(key, filterContext);
                    }
                }
            }

            return filterContext;
        }

        public static QueryFilterContext AddOrGetFilterContext(DbContext context)
        {

            if (!CacheWeakFilterContext.TryGetValue(context, out var filterContext))
            {
                filterContext = new QueryFilterContext(context);
                CacheWeakFilterContext.Add(context, filterContext);
            }

            return filterContext;
        }

        public static BaseQueryFilterQueryable GetFilterQueryable(IQueryable query)
        {
            CacheWeakFilterQueryable.TryGetValue(query, out var filterQueryable);
            return filterQueryable;
        }

        public static BaseQueryFilter Filter(string key)
        {
            GlobalFilters.TryGetValue(key, out var filter);

            return filter;
        }

        public static BaseQueryFilter Filter<T>(Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true) => Filter(Guid.NewGuid().ToString("N"), queryFilter, isEnabled);

        public static BaseQueryFilter Filter<T>(string key, Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true)
        {
            if (!GlobalFilters.TryGetValue(key, out var filter))
            {
                filter = new QueryFilter<T>(null, queryFilter) { IsDefaultEnabled = isEnabled };
                GlobalFilters.Add(key, filter);
            }

            return filter;
        }

        public static void InitilizeGlobalFilter(DbContext context)
        {
            var cloneDictionary = new Dictionary<BaseQueryFilter, BaseQueryFilter>();

            var filterContext = AddOrGetFilterContext(context);

            foreach (var filter in GlobalFilters)
            {
                var clone = filter.Value.Clone(filterContext);
                filterContext.Filters.Add(filter.Key, clone);
                if (filter.Value.IsDefaultEnabled)
                {
                    clone.Enable();
                }

                cloneDictionary.Add(filter.Value, clone);
            }

            foreach (var initlizeAction in GlobalInitializeFilterActions)
            {
                initlizeAction.Item2(cloneDictionary[initlizeAction.Item1]);
            }
        }
        #endregion
    }
}
