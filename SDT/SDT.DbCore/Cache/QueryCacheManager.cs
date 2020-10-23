using Microsoft.Extensions.Caching.Memory;
using SDT.BaseTool;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SDT.DbCore
{
    public static class QueryCacheManager
    {
        #region Members
        /// <summary>
        /// 获取或设置查询缓存是否启用（默认true）
        /// </summary>
        /// <value></value>
        public static bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 获取或设置用于QueryCacheExtensions扩展方法的缓存
        /// </summary>
        public static IMemoryCache Cache { get; set; } = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 获取或设置缓存键值前缀
        /// </summary>
        public static string CachePrefix { get; set; } = "QueryCache_";

        public static Func<IQueryable, string, string> CacheKeyFactory { get; set; }

        private static MemoryCacheEntryOptions _defaultMemoryCacheEntryOptions = new MemoryCacheEntryOptions();

        private static Func<MemoryCacheEntryOptions> _memoryCacheEntryOptionsFactory;

        public static MemoryCacheEntryOptions DefaultMemoryCacheEntryOptions
        {
            get
            {
                if (_defaultMemoryCacheEntryOptions == null && MemoryCacheEntryOptionsFactory != null)
                {
                    return MemoryCacheEntryOptionsFactory();
                }

                return _defaultMemoryCacheEntryOptions;
            }
            set
            {
                _defaultMemoryCacheEntryOptions = value;
                _memoryCacheEntryOptionsFactory = null;
            }
        }

        public static Func<MemoryCacheEntryOptions> MemoryCacheEntryOptionsFactory
        {
            get => _memoryCacheEntryOptionsFactory;
            set
            {
                _memoryCacheEntryOptionsFactory = value;
                _defaultMemoryCacheEntryOptions = null;
            }
        }
        #endregion

        #region Methods
        internal static string GetCacheKey(IQueryable query, string tag = "")
        {
            if (CacheKeyFactory != null)
            {
                return CacheKeyFactory.Invoke(query, tag);
            }

            if (tag.IsNull())
            {
                var sb = new StringBuilder();
                var command = query.CreateCommand(out var queryContext);
                sb.Append(queryContext.Connection.DbConnection.ConnectionString);
                sb.Append(query.Expression.ToString());
                sb.Append(command.CommandText);
                foreach (var parameter in queryContext.ParameterValues)
                {
                    sb.Append(parameter.Key);
                    sb.Append("=");
                    sb.Append(parameter.Value);
                    sb.AppendLine("|");
                }

                tag = sb.ToString();
                tag = StringUtility.HashHex<SHA1CryptoServiceProvider>(tag);
            }

            var key = CachePrefix + tag;
            return key;
        }

        internal static string GetCacheKey<T>(QueryDeferred<T> query, string tag = "") => GetCacheKey(query.Query, tag);

        /// <summary>
        /// 指定缓存过期
        /// </summary>
        /// <param name="keys"></param>
        public static void RemoveCache(params string[] keys)
        {
            foreach (var key in keys)
            {
                Cache.Remove(key);
            }
        }
        #endregion
    }
}
