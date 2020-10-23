using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDT.DbCore
{
    public class QueryFilterContext
    {
        #region Members
        public DbContext Context { get; protected internal set; }

        public Dictionary<object, BaseQueryFilter> Filters { get; protected internal set; } = new Dictionary<object, BaseQueryFilter>();

        public Dictionary<Type, List<QueryFilterSet>> FilterSetByType { get; protected internal set; }

        public List<QueryFilterSet> FilterSets { get; protected internal set; }
        #endregion

        #region Constructors
        public QueryFilterContext(DbContext context, bool isGenericContext = false)
        {
            if (isGenericContext)
            {
                LoadGenericContextInfo(context);
            }
            else
            {
                Context = context;

                var genericContext = QueryFilterManager.AddOrGetGenericFilterContext(context);
                FilterSetByType = genericContext.FilterSetByType;
                FilterSets = genericContext.FilterSets;
            }
        }
        #endregion

        #region Methods
        public BaseQueryFilter AddFilter<T>(object key, Func<IQueryable<T>, IQueryable<T>> filter)
        {
            var queryFilter = new QueryFilter<T>(this, filter);

            Filters.Add(key, queryFilter);

            return queryFilter;
        }

        public IQueryable<T> ApplyFilter<T>(IQueryable<T> query)
        {
            object newQuery = query;

            foreach (var filter in Filters)
            {
                if (filter.Value.IsDefaultEnabled)
                {
                    newQuery = (IQueryable)filter.Value.ApplyFilter<T>(newQuery);
                }
            }

            return (IQueryable<T>)newQuery;
        }

        public IQueryable<T> ApplyFilter<T>(IQueryable<T> query, object[] keys)
        {
            object newQuery = query;
            foreach (var key in keys)
            {
                var filter = GetFilter(key);

                if (filter != null)
                {
                    newQuery = ((IQueryable)filter.ApplyFilter<T>(newQuery));
                }
            }

            return (IQueryable<T>)newQuery;
        }

        public void DisableFilter(BaseQueryFilter filter, params Type[] types)
        {
            // check if the element type can be used in the context
            if (FilterSetByType.TryGetValue(filter.ElementType, out var filterSets))
            {
                if (types != null)
                {
                    var applySets = new List<QueryFilterSet>();

                    foreach (var type in types)
                    {
                        if (FilterSetByType.TryGetValue(type, out var setToAdd))
                        {
                            applySets.AddRange(setToAdd);
                        }
                    }

                    // keep only applicable filter set
                    filterSets = filterSets.Intersect(applySets.Distinct()).ToList();
                }

                foreach (var set in filterSets)
                {
                    set.AddOrGetFilterQueryable(Context).DisableFilter(filter);
                }
            }
        }

        public void EnableFilter(BaseQueryFilter filter, params Type[] types)
        {
            // check if the element type can be used in the context
            if (FilterSetByType.TryGetValue(filter.ElementType, out var filterSets))
            {
                if (types != null)
                {
                    var applySets = new List<QueryFilterSet>();

                    foreach (var type in types)
                    {
                        if (FilterSetByType.TryGetValue(type, out var setToAdd))
                        {
                            applySets.AddRange(setToAdd);
                        }
                    }

                    // keep only applicable filter set
                    filterSets = filterSets.Intersect(applySets.Distinct()).ToList();
                }

                foreach (var set in filterSets)
                {
                    set.AddOrGetFilterQueryable(Context).EnableFilter(filter);
                }
            }
        }

        public BaseQueryFilter GetFilter(object key)
        {
            Filters.TryGetValue(key, out var filter);
            return filter;
        }

        public void LoadGenericContextInfo(DbContext context)
        {
            FilterSetByType = new Dictionary<Type, List<QueryFilterSet>>();
            FilterSets = new List<QueryFilterSet>();

            var dbSetProperties = context.GetDbSetProperties();

            // add DbSet
            foreach (var dbSetProperty in dbSetProperties)
            {
                FilterSets.Add(new QueryFilterSet(dbSetProperty));
            }

            // link DbSet to Type
            foreach (var filterDbSet in FilterSets)
            {
                var baseType = filterDbSet.ElementType;

                while (baseType != null && baseType != typeof(object))
                {
                    // link type
                    FilterSetByType.AddOrAppend(baseType, filterDbSet);

                    // link interface
                    var interfaces = baseType.GetInterfaces();
                    foreach (var @interface in interfaces)
                    {
                        FilterSetByType.AddOrAppend(@interface, filterDbSet);
                    }

                    baseType = baseType.BaseType;
                }
            }
        }
        #endregion
    }
}
