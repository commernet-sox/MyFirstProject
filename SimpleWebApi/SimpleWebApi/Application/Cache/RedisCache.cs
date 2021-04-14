using System;
using CPC.Redis;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CPC;

namespace SimpleWebApi.Application.Cache
{
    public class RedisCache : ISeedCache
    {
        private readonly RedisClient _redisClient;
        private readonly int[] _dbIndexArray;
        private readonly IHttpContextAccessor _accessor;

        public string CachePrefix { get; set; } = "cache:";

        public RedisCache(RedisClient redisClient, IHttpContextAccessor accessor, int[] dbIndex)
        {
            _redisClient = redisClient;
            if (dbIndex.IsNull())
            {
                dbIndex = new int[] { 1 };
            }
            _dbIndexArray = dbIndex;
            _accessor = accessor;
        }

        public virtual void Add<T>(string key, T item, TimeSpan? ttl = null)
        {
            var redisKey = GenKey(key);
            Handle(redisKey, () =>
            {
                return _redisClient.Set(redisKey, item, ttl);
            });
        }

        public virtual string GetString(string key)
        {
            var redisKey = GenKey(key);
            return Handle(redisKey, () =>
            {
                string result = _redisClient.Database.StringGet(redisKey);
                return result;
            });
        }


        public T GetOrAdd<T>(string key, Func<T> creator) => GetOrAdd(key, creator, TimeSpan.FromDays(1));

        public T GetOrAdd<T>(string key, Func<T> creator, TimeSpan ttl)
        {
            var redisKey = GenKey(key);

            return Handle(redisKey, () =>
            {
                var value = _redisClient.Get<T>(redisKey);
                if (value == null)
                {
                    value = creator();
                    if (value != null)
                    {
                        _redisClient.SetNx(redisKey, value, ttl);
                    }
                }

                return value;
            });
        }

        private string GenKey(string key)
        {
            var cfgId = _accessor?.HttpContext?.GetHeaderValue("CfgId");
            if (!cfgId.IsNull())
            {
                key = cfgId + "#" + key;
            }

            var redisKey = CachePrefix + key;
            return redisKey;
        }

        private T Handle<T>(string redisKey, Func<T> creator)
        {
            var originalIndex = _redisClient.DatabaseIndex;
            var originalFunc = _redisClient.CalcDbIndex;

            if (_dbIndexArray.Length == 1)
            {
                _redisClient.ChangeDb(_dbIndexArray[0]);
            }
            else
            {
                var index = _dbIndexArray[Math.Abs(redisKey.GetHashCode() % _dbIndexArray.Length)];
                _redisClient.ChangeDb(index);
            }

            var value = creator.Invoke();

            _redisClient.ChangeDb(originalIndex);
            _redisClient.CalcDbIndex = originalFunc;
            return value;
        }

        public long Remove(params string[] keys)
        {
            if (keys.IsNull())
            {
                return 0;
            }

            var redisKeys = keys.Select(t => GenKey(t)).ToArray();
            var originalIndex = _redisClient.DatabaseIndex;
            var originalFunc = _redisClient.CalcDbIndex;
            var res = 0L;
            if (_dbIndexArray.Length == 1)
            {
                _redisClient.ChangeDb(_dbIndexArray[0]);
                res = _redisClient.Del(redisKeys);
            }
            else
            {
                var dic = new Dictionary<int, HashSet<string>>();
                foreach (var redisKey in redisKeys)
                {
                    var index = _dbIndexArray[Math.Abs(redisKey.GetHashCode() % _dbIndexArray.Length)];
                    var val = dic.GetValueOrDefault(index);
                    if (val.IsNull())
                    {
                        val = new HashSet<string>() { redisKey };
                        dic.Add(index, val);
                    }
                    else
                    {
                        val.Add(redisKey);
                    }
                }

                foreach (var group in dic)
                {
                    _redisClient.ChangeDb(group.Key);
                    res += _redisClient.Del(group.Value.ToArray());
                }
            }

            _redisClient.ChangeDb(originalIndex);
            _redisClient.CalcDbIndex = originalFunc;
            return res;
        }
    }
}
