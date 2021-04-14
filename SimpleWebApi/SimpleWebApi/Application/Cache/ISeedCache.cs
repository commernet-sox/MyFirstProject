using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Cache
{
    public interface ISeedCache
    {
        string GetString(string key);

        void Add<T>(string key, T item, TimeSpan? ttl = null);

        /// <summary>
        /// 从缓存中获取数据，获取不到则插入到缓存中，并返回结果，有效期默认为1天
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="creator">数据</param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<T> creator);

        /// <summary>
        /// 从缓存中获取数据，获取不到则插入到缓存中，并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="creator">数据</param>
        /// <param name="ttl">有效期</param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<T> creator, TimeSpan ttl);

        long Remove(params string[] keys);
    }
}
