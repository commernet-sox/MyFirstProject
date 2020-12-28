using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace CPC.DBCore
{
    internal static class InternalExtensions
    {
        #region Type
        internal static Type GetTypeFromAssembly(this Type fromType, string name) => fromType.Assembly.GetType(name);

        internal static Type GetDbSetElementType(this Type type) => type.GetGenericArguments()[0];

        internal static void AddOrAppend<TKey, TElement>(this Dictionary<TKey, List<TElement>> dictionary, TKey key, TElement element)
        {
            if (!dictionary.TryGetValue(key, out var elements))
            {
                elements = new List<TElement>();
                dictionary.Add(key, elements);
            }

            elements.Add(element);
        }
        #endregion

        #region DbContext
        internal static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;

                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }
            return dbSetProperties;
        }

        internal static DbContext GetDbContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            var internalContext = dbSet.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance);
            return (DbContext)internalContext.GetValue(dbSet);
        }
        #endregion

        #region MethodInfo
        internal static MethodInfo MethodInfo<T1, T2>(this Func<T1, T2> source, T1 unused1) => source.Method;

        internal static MethodInfo MethodInfo<T1, T2, T3>(this Func<T1, T2, T3> source, T1 unused1, T2 unused2) => source.Method;

        internal static MethodInfo MethodInfo<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> source, T1 unused1, T2 unused2, T3 unused3) => source.Method;

        internal static MethodInfo MethodInfo<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> source, T1 unused1, T2 unused2, T3 unused3, T4 unused4) => source.Method;
        #endregion

        #region Common
        internal static StateManager GetStateManager(this ChangeTracker changeTracker)
        {
            var stateManagerField = changeTracker.GetType().GetProperty("StateManager", BindingFlags.NonPublic | BindingFlags.Instance);
            return (StateManager)stateManagerField.GetValue(changeTracker);
        }

        internal static void CopyFrom(this DbParameter param, IRelationalParameter from, object value, string newParameterName)
        {
            param.ParameterName = newParameterName;

            if (from is TypeMappedRelationalParameter)
            {
                var relationalTypeMappingProperty = typeof(TypeMappedRelationalParameter).GetProperty("RelationalTypeMapping", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (relationalTypeMappingProperty != null)
                {
                    var relationalTypeMapping = (RelationalTypeMapping)relationalTypeMappingProperty.GetValue(from);

                    if (relationalTypeMapping.DbType.HasValue)
                    {
                        param.DbType = relationalTypeMapping.DbType.Value;
                    }
                }
            }

            param.Value = value ?? DBNull.Value;
        }
        #endregion
    }
}
