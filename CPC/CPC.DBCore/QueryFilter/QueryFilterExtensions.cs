using CPC.DBCore.QueryFilter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CPC.DBCore
{
    public static class QueryFilterExtensions
    {
        public static BaseQueryFilter Filter(this DbContext context, object key)

        {
            var filterContext = QueryFilterManager.AddOrGetFilterContext(context);

            return filterContext.GetFilter(key);
        }

        public static BaseQueryFilter Filter<T>(this DbContext context, Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true) => context.Filter(Guid.NewGuid(), queryFilter, isEnabled);

        public static BaseQueryFilter Filter<T>(this DbContext context, object key, Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true)
        {
            var filterContext = QueryFilterManager.AddOrGetFilterContext(context);

            var filter = filterContext.AddFilter(key, queryFilter);

            if (isEnabled)
            {
                filter.Enable();
            }

            return filter;
        }

        public static IQueryable<T> Filter<T>(this DbSet<T> query, params object[] keys) where T : class
        {
            var queryFilterQueryable = QueryFilterManager.GetFilterQueryable(query);

            var nonQueryFilter = queryFilterQueryable != null ? (IQueryable<T>)queryFilterQueryable.OriginalQuery : query;

            var context = queryFilterQueryable != null ? queryFilterQueryable.Context : query.GetDbContext();

            var filterContext = QueryFilterManager.AddOrGetFilterContext(context);

            return filterContext.ApplyFilter(nonQueryFilter, keys);
        }

        public static IQueryable<T> AsNoFilter<T>(this IQueryable<T> query) where T : class
        {
            var queryFilterQueryable = QueryFilterManager.GetFilterQueryable(query);

            return queryFilterQueryable != null ? (IQueryable<T>)queryFilterQueryable.OriginalQuery : query;
        }

        public static IQueryable<T> SetFiltered<T>(this DbContext context) where T : class
        {
            var filterContext = QueryFilterManager.AddOrGetFilterContext(context);

            if (filterContext.FilterSetByType.ContainsKey(typeof(T)))
            {
                var set = filterContext.FilterSetByType[typeof(T)];

                if (set.Count == 1)
                {
                    return (IQueryable<T>)set[0].DbSetProperty.GetValue(context);
                }
                throw new Exception("many set for the specified type has been found");
            }

            throw new Exception("no set for the specified type has been found.");
        }
    }
}
