﻿using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 服务黑名单策略
    /// </summary>
    public class ServiceBlackPolicy
    {
        public static readonly ConcurrentDictionary<string, DateTime> _blacklist = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 黑名单
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="target"></param>
        public static void Add(string serviceName, string target)
        {
            var key = $"{serviceName}_{target}";
            var now = DateTime.UtcNow;
            _blacklist.AddOrUpdate(key, k => now, (k, old) => now);
        }

        /// <summary>
        /// 是否在黑名单
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool In(string serviceName, string target)
        {
            var key = $"{serviceName}_{target}";
            if (_blacklist.TryGetValue(key, out var lastFailure))
            {
                if (DateTime.UtcNow - lastFailure < ClientTimespan.BlacklistPeriod)
                {
                    return true;
                }

                _blacklist.TryRemove(key, out _);
            }
            return false;
        }

        /// <summary>
        /// 服务是否有节点存在黑名单
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool Exist(string serviceName) => _blacklist.Keys.Any(oo => oo.StartsWith(serviceName));
    }
}
