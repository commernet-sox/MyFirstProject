using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore
{

    public static class QueryCacheExtensions
    {
        #region IQueryable
        public static IEnumerable<T> FromCache<T>(this IQueryable<T> query, string tag = "", bool refreshCache = false) where T : class => query.FromCache(QueryCacheManager.DefaultMemoryCacheEntryOptions, tag, refreshCache);

        public static IEnumerable<T> FromCache<T>(this IQueryable<T> query, MemoryCacheEntryOptions options, string tag = "", bool refreshCache = false) where T : class
        {
            if (!QueryCacheManager.IsEnabled)
            {
                return query.AsNoTracking().ToList();
            }

            var key = QueryCacheManager.GetCacheKey(query, tag);

            if (refreshCache)
            {
                QueryCacheManager.RemoveCache(key);
            }

            if (!QueryCacheManager.Cache.TryGetValue(key, out var item))
            {
                item = query.AsNoTracking().ToList();
                item = QueryCacheManager.Cache.Set(key, item, options);
            }

            item = item.IfDbNullThenNull();

            return (IEnumerable<T>)item;
        }


        public static Task<IEnumerable<T>> FromCacheAsync<T>(this IQueryable<T> query, string tag = "", bool refreshCache = false, CancellationToken cancellationToken = default) where T : class => query.FromCacheAsync(QueryCacheManager.DefaultMemoryCacheEntryOptions, tag, refreshCache, cancellationToken);

        public static async Task<IEnumerable<T>> FromCacheAsync<T>(this IQueryable<T> query, MemoryCacheEntryOptions options, string tag = "", bool refreshCache = false, CancellationToken cancellationToken = default) where T : class
        {
            if (!QueryCacheManager.IsEnabled)
            {
                return await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            var key = QueryCacheManager.GetCacheKey(query, tag);

            if (refreshCache)
            {
                QueryCacheManager.RemoveCache(key);
            }

            if (!QueryCacheManager.Cache.TryGetValue(key, out var item))
            {
                item = await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
                item = QueryCacheManager.Cache.Set(key, item, options);
            }

            item = item.IfDbNullThenNull();

            return (IEnumerable<T>)item;
        }
        #endregion

        #region QueryDeferred
        public static T FromCache<T>(this QueryDeferred<T> query, string tag = "", bool refreshCache = false) => query.FromCache(QueryCacheManager.DefaultMemoryCacheEntryOptions, tag, refreshCache);

        public static T FromCache<T>(this QueryDeferred<T> query, MemoryCacheEntryOptions options, string tag = "", bool refreshCache = false)
        {
            if (!QueryCacheManager.IsEnabled)
            {
                return query.Execute();
            }

            var key = QueryCacheManager.GetCacheKey(query, tag);

            if (refreshCache)
            {
                QueryCacheManager.RemoveCache(key);
            }

            if (!QueryCacheManager.Cache.TryGetValue(key, out var item))
            {
                item = query.Execute();
                item = QueryCacheManager.Cache.Set(key, item ?? DBNull.Value, options);
            }

            item = item.IfDbNullThenNull();

            return (T)item;
        }

        public static Task<T> FromCacheAsync<T>(this QueryDeferred<T> query, string tag = "", bool refreshCache = false, CancellationToken cancellationToken = default) => query.FromCacheAsync(QueryCacheManager.DefaultMemoryCacheEntryOptions, tag, refreshCache, cancellationToken);

        public static async Task<T> FromCacheAsync<T>(this QueryDeferred<T> query, MemoryCacheEntryOptions options, string tag = "", bool refreshCache = false, CancellationToken cancellationToken = default)
        {
            if (!QueryCacheManager.IsEnabled)
            {
                return await query.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }

            var key = QueryCacheManager.GetCacheKey(query, tag);

            if (refreshCache)
            {
                QueryCacheManager.RemoveCache(key);
            }

            if (!QueryCacheManager.Cache.TryGetValue(key, out var item))
            {
                item = await query.ExecuteAsync(cancellationToken).ConfigureAwait(false);
                item = QueryCacheManager.Cache.Set(key, item ?? DBNull.Value, options);
            }

            item = item.IfDbNullThenNull();

            return (T)item;
        }
        #endregion
    }
}
