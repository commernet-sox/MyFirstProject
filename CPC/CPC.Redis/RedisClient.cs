using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CPC.Redis
{
    public class RedisClient : IKeyCommand, IStringCommand, IListCommand, IHashCommand, ISetCommand, ISortedSetCommand
    {
        #region Private Members
        protected static readonly ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>> _dic = new ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>>();

        protected readonly ConfigurationOptions _option;

        protected readonly string _connectionString;
        #endregion

        #region Constructors
        /// <summary>
        /// operation redis of SDK based on StackExchange
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="totalDb">the number of databases (if more than 1 will be distributed automatically)</param>
        /// <param name="dbIndex">the current database index (to take effect, you need to set totalDb as the default value)</param>
        public RedisClient(string connectionString, int totalDb = 0, int dbIndex = -1)
        {
            _option = ConfigurationOptions.Parse(connectionString);
            _option.AbortOnConnectFail = false;
            _option.ReconnectRetryPolicy = new ExponentialRetry(1000);
            _connectionString = connectionString;
            _dic.GetOrAdd(connectionString, _ => LzayConnection());
            TotalDatabase = totalDb;
            if (dbIndex >= 0)
            {
                DatabaseIndex = dbIndex;
            }
            else if (_option.DefaultDatabase.HasValue)
            {
                DatabaseIndex = _option.DefaultDatabase.Value;
            }
        }

        public RedisClient(ConfigurationOptions options, int totalDb = 0)
        {
            _option = options ?? throw new ArgumentNullException(nameof(options));
            _connectionString = options.ToString();
            _dic.GetOrAdd(_connectionString, _ => LzayConnection());
            TotalDatabase = totalDb;
            if (_option.DefaultDatabase.HasValue)
            {
                DatabaseIndex = _option.DefaultDatabase.Value;
            }
        }
        #endregion

        #region Members

        public IDatabase Database => Connection.GetDatabase(DatabaseIndex);

        public IConnectionMultiplexer Connection
        {
            get
            {
                lock (_dic)
                {
                    if (_dic.TryGetValue(_connectionString, out var connLazy))
                    {
                        var conn = connLazy.Value;
                        if (!conn.IsConnected && !conn.IsConnecting)
                        {
                            connLazy = LzayConnection();
                            _ = _dic.AddOrUpdate(_connectionString, connLazy, (k, v) => connLazy);
                            conn = connLazy.Value;
                        }
                        return conn;
                    }
                }

                throw new RedisConnectionException(ConnectionFailureType.None, "redis connection");
            }
        }

        /// <summary>
        /// total number of databases
        /// </summary>
        public int TotalDatabase { get; private set; } = 0;

        /// <summary>
        /// current database index
        /// </summary>
        public int DatabaseIndex { get; private set; } = -1;

        /// <summary>
        /// custom calculate the database index
        /// </summary>
        public Func<string, int> CalcDbIndex;

        public IServer Server => Connection.GetServer(_option.EndPoints.FirstOrDefault());

        public ISubscriber Subscriber => Connection.GetSubscriber();

        public IKeyCommand Key => this;

        public IStringCommand String => this;

        public IListCommand List => this;

        public IHashCommand Hash => this;

        public ISetCommand HashSet => this;

        public ISortedSetCommand SortedSet => this;
        #endregion

        #region System
        protected virtual Lazy<IConnectionMultiplexer> LzayConnection()
        {
            var conn = ConnectionMultiplexer.Connect(_option);
            //连接失败
            conn.ConnectionFailed += (sender, e) =>
            {
                LogUtility.Warn($"ConnectionFailed: {e.EndPoint.GetFriendlyName()} ConnectionType: {e.ConnectionType} FailureType: {e.FailureType}", e.Exception);
            };

            //重新建立连接
            conn.ConnectionRestored += (sender, e) =>
            {
                LogUtility.Warn($"ConnectionRestored: {e.EndPoint.GetFriendlyName()} ConnectionType: {e.ConnectionType} FailureType: {e.FailureType}");
            };

            //发生内部错误
            conn.ErrorMessage += (sender, e) =>
            {
                LogUtility.Warn($"ErrorMessage: {e.EndPoint.GetFriendlyName()} Message: {e.Message}");
            };

            //类库发生的错误
            conn.InternalError += (sender, e) =>
            {
                LogUtility.Error($"InternalError: {e.EndPoint.GetFriendlyName()} Message: {e.Exception.Message}", e.Exception);
            };

            //集群被修改
            conn.HashSlotMoved += (sender, e) =>
            {
                LogUtility.Warn($"HashSlotMoved: New:{e.NewEndPoint.GetFriendlyName()} Old:{e.OldEndPoint.GetFriendlyName()}");
            };

            //配置被修改
            conn.ConfigurationChanged += (sender, e) =>
            {
                LogUtility.Warn($"ConfigurationChanged: {e.EndPoint.GetFriendlyName()}");
            };

            // 重新配置广播时（通常意味着主从同步更改）
            conn.ConfigurationChangedBroadcast += (sender, e) =>
            {
                LogUtility.Warn($"ConfigurationChangedBroadcast: {e.EndPoint.GetFriendlyName()}");
            };
            return new Lazy<IConnectionMultiplexer>(() => { return conn; });
        }

        protected virtual int CalcDatabaseIndex(string key)
        {
            var index = DatabaseIndex;
            if (CalcDbIndex != null)
            {
                index = CalcDbIndex.Invoke(key);
            }
            else if (TotalDatabase > 0)
            {
                index = Math.Abs(key.GetHashCode() % TotalDatabase);
            }
            DatabaseIndex = index;
            return DatabaseIndex;
        }

        public void ChangeDb(int index)
        {
            CalcDbIndex = null;
            TotalDatabase = 0;
            DatabaseIndex = index;
        }
        #endregion

        #region Key
        public long Del(params string[] keys) => Database.KeyDelete(keys.ToArrayEx());

        public bool Exists(string key)
        {
            CalcDatabaseIndex(key);
            return Database.KeyExists(key);
        }

        public bool Expire(string key, int sec)
        {
            CalcDatabaseIndex(key);
            return Database.KeyExpire(key, new TimeSpan(0, 0, sec));
        }

        public List<string> Keys(string pattern) => Server.Keys(DatabaseIndex, pattern).ToListEx();

        public bool Move(string key, int dbIndex)
        {
            CalcDatabaseIndex(key);
            return Database.KeyMove(key, dbIndex);
        }

        public bool Persist(string key)
        {
            CalcDatabaseIndex(key);
            return Database.KeyPersist(key);
        }

        public string RandomKey() => Database.KeyRandom();

        public void Rename(string key, string newKey)
        {
            CalcDatabaseIndex(key);
            Database.KeyRename(key, newKey);
        }

        public bool RenameNx(string key, string newKey)
        {
            CalcDatabaseIndex(key);
            return Database.KeyRename(key, newKey, When.NotExists);
        }

        public List<T> Sort<T>(string key, int skip = 0, int take = -1, bool orderDesc = false, bool sortAlphabetic = false)
        {
            CalcDatabaseIndex(key);
            var data = Database.Sort(key, skip, take, orderDesc ? Order.Descending : Order.Ascending, sortAlphabetic ? SortType.Alphabetic : SortType.Numeric);
            return data.ToListEx<T>();
        }

        public int TTL(string key)
        {
            CalcDatabaseIndex(key);
            return Database.KeyTimeToLive(key).GetValueOrDefault().TotalSeconds.ConvertInt32();
        }

        public string Type(string key)
        {
            CalcDatabaseIndex(key);
            return Database.KeyType(key).ToString();
        }
        #endregion

        #region String
        public int Append(string key, string value)
        {
            CalcDatabaseIndex(key);
            return Database.StringAppend(key, value).ConvertInt32();
        }

        public long Decr(string key)
        {
            CalcDatabaseIndex(key);
            return Database.StringDecrement(key);
        }

        public long DecrBy(string key, long count)
        {
            CalcDatabaseIndex(key);
            return Database.StringDecrement(key, count);
        }

        public T Get<T>(string key)
        {
            CalcDatabaseIndex(key);
            return (Database.StringGet(key)).ToDataEx<T>();
        }

        public T GetSet<T>(string key, T value)
        {
            CalcDatabaseIndex(key);
            return Database.StringGetSet(key, value.ToDataEx()).ToDataEx<T>();
        }

        public long Incr(string key)
        {
            CalcDatabaseIndex(key);
            return Database.StringIncrement(key);
        }

        public long IncrBy(string key, long count)
        {
            CalcDatabaseIndex(key);
            return Database.StringIncrement(key, count);
        }

        public bool Set<T>(string key, T value, TimeSpan? ttl = null)
        {
            CalcDatabaseIndex(key);
            return Database.StringSet(key, value.ToDataEx(), ttl);
        }

        public bool SetNx<T>(string key, T value, TimeSpan? ttl = null)
        {
            CalcDatabaseIndex(key);
            return Database.StringSet(key, value.ToDataEx(), ttl, When.NotExists, CommandFlags.DemandMaster);
        }

        public bool SetEx<T>(string key, T value, TimeSpan? ttl = null)
        {
            CalcDatabaseIndex(key);
            return Database.StringSet(key, value.ToDataEx(), ttl, When.Exists);
        }
        #endregion

        #region List
        public T LIndex<T>(string key, int index)
        {
            CalcDatabaseIndex(key);
            return Database.ListGetByIndex(key, index).ToDataEx<T>();
        }

        public int LLen(string key)
        {
            CalcDatabaseIndex(key);
            return Database.ListLength(key).ConvertInt32();
        }

        public T LPop<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.ListLeftPop(key).ToDataEx<T>();
        }

        public int LPush<T>(string key, params T[] values)
        {
            CalcDatabaseIndex(key);
            return Database.ListLeftPush(key, values.ToArrayEx().ToArray()).ConvertInt32();
        }

        public List<T> LRange<T>(string key, int start, int stop)
        {
            CalcDatabaseIndex(key);
            return Database.ListRange(key, start, stop).ToListEx<T>();
        }

        public int LRem<T>(string key, T value, int count = 0)
        {
            CalcDatabaseIndex(key);
            return Database.ListRemove(key, value.ToDataEx(), count).ConvertInt32();
        }

        public void LSet<T>(string key, T value, int index)
        {
            CalcDatabaseIndex(key);
            Database.ListSetByIndex(key, index, value.ToDataEx());
        }

        public void LTrim(string key, int start, int stop)
        {
            CalcDatabaseIndex(key);
            Database.ListTrim(key, start, stop);
        }

        public T RPop<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.ListRightPop(key).ToDataEx<T>();
        }

        public int RPush<T>(string key, params T[] values)
        {
            CalcDatabaseIndex(key);
            return Database.ListRightPush(key, values.ToArrayEx()).ConvertInt32();
        }
        #endregion

        #region Hash
        public int HDel<T>(string key, params T[] hashFields)
        {
            CalcDatabaseIndex(key);
            return Database.HashDelete(key, hashFields.ToArrayEx<T>()).ConvertInt32();
        }

        public bool HExists<T>(string key, T hashField)
        {
            CalcDatabaseIndex(key);
            return Database.HashExists(key, hashField.ToDataEx());
        }

        public TVal HGet<TKey, TVal>(string key, TKey hashField)
        {
            CalcDatabaseIndex(key);
            return Database.HashGet(key, hashField.ToDataEx()).ToDataEx<TVal>();
        }

        public List<TVal> HGet<TKey, TVal>(string key, params TKey[] hashFields)
        {
            CalcDatabaseIndex(key);
            var data = Database.HashGet(key, hashFields.ToArrayEx());
            return data.ToListEx<TVal>();
        }

        public Dictionary<TKey, TVal> HGetAll<TKey, TVal>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.HashGetAll(key).ToDictionary(t => t.Name.ToDataEx<TKey>(), t => t.Value.ToDataEx<TVal>());
        }

        public List<T> HKeys<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.HashKeys(key).ToListEx<T>();
        }

        public int HLen(string key)
        {
            CalcDatabaseIndex(key);
            return Database.HashLength(key).ConvertInt32();
        }

        public void HSet<TKey, TVal>(string key, Dictionary<TKey, TVal> hashData)
        {
            CalcDatabaseIndex(key);
            Database.HashSet(key, hashData.Select(p => new HashEntry(p.Key.ToDataEx(), p.Value.ToDataEx())).ToArray());
        }

        public bool HSet<TKey, TVal>(string key, TKey hashField, TVal hashVal)
        {
            CalcDatabaseIndex(key);
            return Database.HashSet(key, hashField.ToDataEx(), hashVal.ToDataEx());
        }

        public bool HSetNx<TKey, TVal>(string key, TKey hashField, TVal hashVal)
        {
            CalcDatabaseIndex(key);
            return Database.HashSet(key, hashField.ToDataEx(), hashVal.ToDataEx(), When.NotExists);
        }
        #endregion

        #region Set
        public int SAdd<T>(string key, params T[] members)
        {
            CalcDatabaseIndex(key);
            return Database.SetAdd(key, members.ToArrayEx()).ConvertInt32();
        }

        public int SCard(string key)
        {
            CalcDatabaseIndex(key);
            return Database.SetLength(key).ConvertInt32();
        }

        public HashSet<T> SDiff<T>(params string[] keys) => Database.SetCombine(SetOperation.Difference, keys.ToArrayEx()).ToListEx<T>().ToSetEx();

        public HashSet<T> SInter<T>(params string[] keys) => Database.SetCombine(SetOperation.Intersect, keys.ToArrayEx()).ToListEx<T>().ToSetEx();

        public bool SIsMember<T>(string key, T member)
        {
            CalcDatabaseIndex(key);
            return Database.SetContains(key, member.ToDataEx());
        }

        public HashSet<T> SMembers<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.SetMembers(key).ToListEx<T>().ToSetEx();
        }

        public T SPop<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.SetPop(key).ToDataEx<T>();
        }

        public int SRem<T>(string key, params T[] members)
        {
            CalcDatabaseIndex(key);
            return Database.SetRemove(key, members.ToArrayEx()).ConvertInt32();
        }

        public T SRandMember<T>(string key)
        {
            CalcDatabaseIndex(key);
            return Database.SetRandomMember(key).ToDataEx<T>();
        }

        public HashSet<T> SUnion<T>(params string[] keys) => Database.SetCombine(SetOperation.Union, keys.ToArrayEx()).ToListEx<T>().ToSetEx();
        #endregion

        #region SortSet
        public int ZAdd<T>(string key, Dictionary<T, double> members)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetAdd(key, members.Select(p => new SortedSetEntry(p.Key.ToDataEx(), p.Value)).ToArray()).ConvertInt32();
        }

        public int ZCard(string key)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetLength(key).ConvertInt32();
        }

        public int ZCount(string key, double min, double max)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetLength(key, min, max, Exclude.Both).ConvertInt32();
        }

        public double ZIncrBy<T>(string key, T member, double score)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetIncrement(key, member.ToDataEx(), score);
        }

        public Dictionary<T, double> ZRange<T>(string key, int start, int stop)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRangeByRankWithScores(key, start, stop).ToDictionary(t => t.Element.ToDataEx<T>(), t => t.Score);
        }

        public Dictionary<T, double> ZRangeByScore<T>(string key, double min, double max)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRangeByScoreWithScores(key, min, max).ToDictionary(t => t.Element.ToDataEx<T>(), t => t.Score);
        }


        public int? ZRank<T>(string key, T member)
        {
            CalcDatabaseIndex(key);
            var result = Database.SortedSetRank(key, member.ToDataEx());
            if (!result.HasValue)
            {
                return null;
            }

            return result.Value.ConvertInt32();
        }

        public int ZRem<T>(string key, params T[] members)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRemove(key, members.ToArrayEx()).ConvertInt32();
        }

        public int ZRemRangeByRank(string key, int start, int stop)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRemoveRangeByRank(key, start, stop).ConvertInt32();
        }

        public int ZRemRangeByScore(string key, double min, double max)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRemoveRangeByScore(key, min, max).ConvertInt32();
        }

        public Dictionary<T, double> ZRevRange<T>(string key, int start, int stop)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRangeByRankWithScores(key, start, stop, Order.Descending).ToDictionary(t => t.Element.ToDataEx<T>(), t => t.Score);
        }

        public Dictionary<T, double> ZRevRangeByScore<T>(string key, double min, double max)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetRangeByScoreWithScores(key, min, max, Exclude.Both, Order.Descending).ToDictionary(t => t.Element.ToDataEx<T>(), t => t.Score);
        }

        public int? ZRevRank<T>(string key, T member)
        {
            CalcDatabaseIndex(key);
            var result = Database.SortedSetRank(key, member.ToDataEx(), Order.Descending);
            if (!result.HasValue)
            {
                return null;
            }

            return result.Value.ConvertInt32();
        }

        public double? ZScore<T>(string key, T member)
        {
            CalcDatabaseIndex(key);
            return Database.SortedSetScore(key, member.ToDataEx());
        }
        #endregion

        #region Other
        /// <summary>
        /// 执行脚本（Lua）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="script"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RedisResult ScriptEvaluate<T>(string script, string[] key, T[] value)
        {
            var result = Database.ScriptEvaluate(script, key.ToArrayEx(), value.ToArrayEx(), CommandFlags.DemandMaster);
            return result;
        }

        /// <summary>
        /// 监听频道消息（消息队列）
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handler"></param>
        public void Listener(string channel, Action<string, string> handler) => Subscriber.Subscribe(channel, (c, m) => { handler?.Invoke(c, m); });

        /// <summary>
        /// 取消监听指定频道
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handler"></param>
        public void UnListener(string channel, Action<string, string> handler = null) => Subscriber.Unsubscribe(channel, (c, m) => { handler?.Invoke(c, m); });

        /// <summary>
        /// 取消监听所有频道
        /// </summary>
        public void UnListener() => Subscriber.UnsubscribeAll();

        /// <summary>
        /// 发布监听消息
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(string channel, string message) => Subscriber.Publish(channel, message);
        #endregion
    }

}
