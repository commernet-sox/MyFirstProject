using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SDT.DbCore
{
    public class QueryFilterSet
    {
        #region Members
        public Lazy<Func<DbContext, QueryFilterSet, object, BaseQueryFilterQueryable>> CreateFilterQueryableCompiled { get; protected internal set; }

        public PropertyInfo DbSetProperty { get; protected internal set; }

        public Type ElementType { get; protected internal set; }

        public Lazy<Func<DbContext, IQueryable>> GetDbSetCompiled { get; protected internal set; }

        public Lazy<Action<DbContext, object>> UpdateInternalQueryCompiled { get; protected internal set; }
        #endregion

        #region Constructors
        public QueryFilterSet(PropertyInfo dbSetProperty)
        {
            CreateFilterQueryableCompiled = new Lazy<Func<DbContext, QueryFilterSet, object, BaseQueryFilterQueryable>>(CompileCreateFilterQueryable);
            DbSetProperty = dbSetProperty;
            ElementType = dbSetProperty.PropertyType.GetDbSetElementType();
            GetDbSetCompiled = new Lazy<Func<DbContext, IQueryable>>(() => CompileGetDbSet(dbSetProperty));
        }
        #endregion

        #region Methods
        public BaseQueryFilterQueryable AddOrGetFilterQueryable(DbContext context)
        {
            var set = GetDbSetCompiled.Value(context);
            if (!QueryFilterManager.CacheWeakFilterQueryable.TryGetValue(set, out var filterQueryable))
            {
                var field = set.GetType().GetProperty("EntityQueryable", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var internalQuery = field.GetValue(set);

                filterQueryable = CreateFilterQueryableCompiled.Value(context, this, internalQuery);
                QueryFilterManager.CacheWeakFilterQueryable.Add(set, filterQueryable);
            }
            return filterQueryable;
        }

        public Func<DbContext, QueryFilterSet, object, BaseQueryFilterQueryable> CompileCreateFilterQueryable()
        {
            var p1 = Expression.Parameter(typeof(DbContext));
            var p2 = Expression.Parameter(typeof(QueryFilterSet));
            var p3 = Expression.Parameter(typeof(object));

            var p3Convert = Expression.Convert(p3, typeof(IQueryable<>).MakeGenericType(ElementType));
            var contructorInfo = typeof(QueryFilterQueryable<>).MakeGenericType(ElementType).GetConstructors()[0];
            var expression = Expression.New(contructorInfo, p1, p2, p3Convert);

            return Expression.Lambda<Func<DbContext, QueryFilterSet, object, BaseQueryFilterQueryable>>(expression, p1, p2, p3).Compile();
        }

        public Func<DbContext, IQueryable> CompileGetDbSet(PropertyInfo dbSetProperty)
        {
            var p1 = Expression.Parameter(typeof(DbContext));
            var p1Convert = Expression.Convert(p1, dbSetProperty.DeclaringType);
            var expression = Expression.Property(p1Convert, dbSetProperty);

            return Expression.Lambda<Func<DbContext, IQueryable>>(expression, p1).Compile();
        }

        public void UpdateInternalQuery(DbContext context, object query)
        {
            // todo: Convert to expression once EF team fix the cast issue: https://github.com/aspnet/EntityFramework/issues/3736
            var set = GetDbSetCompiled.Value(context);

            var field = set.GetType().GetField("_entityQueryable", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(set, query);
        }
        #endregion
    }
}
