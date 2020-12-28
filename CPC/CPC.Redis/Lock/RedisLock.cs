using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.Redis.Lock
{

    /// <summary>
    /// 分布式锁是一个全局性的稀有资源，一旦加锁或者锁的粒度较大,对于分布式的应用程序来说性能是非常差的。对于业务来说，我们优先考虑不采用锁，通过业务协调方式解决。
    /// 在一些必须，强一致性的情况下才考虑锁的使用。需要加锁的业务逻辑应该是细小的, 加锁的前要过滤一部分的重复业务逻辑，这样锁才不会很频繁
    /// </summary>
    public class RedisLock : IDistributedLock
    {
        #region Members
        private readonly object _lockObject = new object();

        private readonly ICollection<RedisClient> _redisCaches;

        private readonly int _quorum;
        private readonly int _quorumRetryCount;
        private readonly int _quorumRetryDelayMs;
        private readonly double _clockDriftFactor;
        private bool isDisposed;

        private Timer _lockKeepaliveTimer;

        private static readonly string UnlockScript = EmbeddedResourceLoader.GetEmbeddedResource("CPC.Redis.Lock.Lua.Unlock.lua");

        private static readonly string ExtendIfMatchingValueScript = EmbeddedResourceLoader.GetEmbeddedResource("CPC.Redis.Lock.Lua.Extend.lua");

        public string Resource { get; private set; }
        public string LockId { get; private set; }
        public bool IsAcquired => Status == DistributedLockStatus.Acquired;
        public DistributedLockStatus Status { get; private set; }
        public DistributedLockSummary InstanceSummary { get; private set; }
        public int ExtendCount { get; private set; }

        public string RedisKeyFormat { get; set; } = "lock:{0}";

        private readonly TimeSpan _expiryTime;
        private readonly TimeSpan? _waitTime;
        private readonly int _retryInterval;
        private readonly bool _keepLive;
        private readonly CancellationToken _cancellationToken;

        private readonly TimeSpan _minimumExpiryTime = TimeSpan.FromMilliseconds(10);
        private readonly int _minimumRetryInterval = 10;
        private readonly bool _forceLock = false;
        #endregion

        #region Constructors
        private RedisLock(ICollection<RedisClient> redisCaches, DistributedLockProfile profile, CancellationToken? cancellationToken = null)
        {
            if (profile.ExpiryTime < _minimumExpiryTime)
            {
                LogUtility.Warn("Expiry time {0}ms too low, setting to {1}ms", profile.ExpiryTime.TotalMilliseconds, _minimumExpiryTime.TotalMilliseconds);
                profile.ExpiryTime = _minimumExpiryTime;
            }

            if (profile.RetryInterval < _minimumRetryInterval)
            {
                LogUtility.Warn("Retry time {0}ms too low, setting to {1}ms", profile.RetryInterval, _minimumRetryInterval);
                profile.RetryInterval = _minimumRetryInterval;
            }

            _redisCaches = redisCaches;

            _quorum = redisCaches.Count / 2 + 1;
            _quorumRetryCount = 5;
            _quorumRetryDelayMs = 400;
            _clockDriftFactor = 0.01;

            Resource = profile.Resource;
            LockId = profile.LockId;
            _forceLock = !LockId.IsNull();
            if (!_forceLock)
            {
                LockId = RandomUtility.GuidString();
            }
            _expiryTime = profile.ExpiryTime;
            _waitTime = profile.WaitTime;
            _retryInterval = profile.RetryInterval;
            _keepLive = profile.KeepLive;
            _cancellationToken = cancellationToken ?? CancellationToken.None;
        }
        #endregion

        #region Methods
        public static RedisLock Create(ICollection<RedisClient> redisCaches, DistributedLockProfile profile, CancellationToken? cancellationToken = null)
        {
            var redisLock = new RedisLock(redisCaches, profile, cancellationToken);
            redisLock.Start();
            return redisLock;
        }

        private long GetRemainingValidityTicks(Stopwatch sw)
        {
            var driftTicks = (long)(_expiryTime.Ticks * _clockDriftFactor) + TimeSpan.FromMilliseconds(2).Ticks;
            var validityTicks = _expiryTime.Ticks - sw.Elapsed.Ticks - driftTicks;
            return validityTicks;
        }

        private DistributedLockStatus GetFailedLockStatus(DistributedLockSummary lockResult)
        {
            if (lockResult.Acquired >= _quorum)
            {
                // if we got here with a quorum then validity must have expired
                return DistributedLockStatus.Expired;
            }

            if (lockResult.Acquired + lockResult.Conflicted >= _quorum)
            {
                // we had enough instances for a quorum, but some were locked with another LockId
                return DistributedLockStatus.Conflicted;
            }

            return DistributedLockStatus.NoQuorum;
        }

        private DistributedLockSummary PopulateLockResult(IEnumerable<DistributedLockResult> instanceResults)
        {
            var acquired = 0;
            var conflicted = 0;
            var error = 0;

            foreach (var instanceResult in instanceResults)
            {
                switch (instanceResult)
                {
                    case DistributedLockResult.Success:
                        acquired++;
                        break;
                    case DistributedLockResult.Conflicted:
                        conflicted++;
                        break;
                    case DistributedLockResult.Error:
                        error++;
                        break;
                }
            }

            return new DistributedLockSummary(acquired, conflicted, error);
        }

        private string GetHost(RedisClient client)
        {
            var cache = client.Connection;
            var result = new StringBuilder();

            foreach (var endPoint in cache.GetEndPoints())
            {
                var server = cache.GetServer(endPoint);

                result.Append(server.EndPoint.GetFriendlyName());
                result.Append(" (");
                result.Append(server.IsReplica ? "slave" : "master");
                result.Append(server.IsConnected ? "" : ", disconnected");
                result.Append("), ");
            }

            return result.ToString().TrimEnd(' ', ',');
        }

        private DistributedLockResult LockInstance(RedisClient cache)
        {
            var redisKey = string.Format(RedisKeyFormat, Resource);
            var host = GetHost(cache);

            DistributedLockResult result;
            try
            {
                if (!_forceLock)
                {
                    var redisResult = cache.String.SetNx(redisKey, LockId, _expiryTime);

                    result = redisResult ? DistributedLockResult.Success : DistributedLockResult.Conflicted;
                }
                else
                {
                    result = ExtendInstance(cache);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error(ex, "Error locking lock instance {host}: {ex.Message}", host, ex.Message);
                result = DistributedLockResult.Error;
            }

            return result;
        }

        private DistributedLockSummary Lock()
        {
            var lockResults = new ConcurrentBag<DistributedLockResult>();

            Parallel.ForEach(_redisCaches, cache =>
            {
                lockResults.Add(LockInstance(cache));
            });

            return PopulateLockResult(lockResults);
        }

        private void UnlockInstance(RedisClient cache)
        {
            var redisKey = string.Format(RedisKeyFormat, Resource);
            var host = GetHost(cache);
            try
            {
                var result = (bool)cache.ScriptEvaluate(UnlockScript, new string[] { redisKey }, new string[] { LockId });
            }
            catch (Exception ex)
            {
                LogUtility.Error(ex, "Error unlocking lock instance {host}: {ex.Message}", host, ex.Message);
            }
        }

        private void Unlock() => Parallel.ForEach(_redisCaches, UnlockInstance);

        private (DistributedLockStatus, DistributedLockSummary) Acquire()
        {
            var lockSummary = new DistributedLockSummary();

            for (var i = 0; i < _quorumRetryCount; i++)
            {
                _cancellationToken.ThrowIfCancellationRequested();

                var stopwatch = Stopwatch.StartNew();

                lockSummary = Lock();

                var validityTicks = GetRemainingValidityTicks(stopwatch);

                if (lockSummary.Acquired >= _quorum && validityTicks > 0)
                {
                    return (DistributedLockStatus.Acquired, lockSummary);
                }

                // we failed to get enough locks for a quorum, unlock everything and try again
                Unlock();

                // only sleep if we have more retries left
                if (i < _quorumRetryCount - 1)
                {
                    var sleepMs = RandomUtility.Next(_quorumRetryDelayMs);
                    Task.Delay(sleepMs, _cancellationToken).Wait(_cancellationToken);
                }
            }

            var status = GetFailedLockStatus(lockSummary);

            return (status, lockSummary);
        }

        private DistributedLockResult ExtendInstance(RedisClient cache)
        {
            var redisKey = string.Format(RedisKeyFormat, Resource);
            var host = GetHost(cache);

            DistributedLockResult result;
            try
            {
                // Returns 1 on success, 0 on failure setting expiry or key not existing, -1 if the key value didn't match
                var extendResult = (long)cache.ScriptEvaluate(ExtendIfMatchingValueScript, new string[] { redisKey }, new string[] { LockId, _expiryTime.TotalMilliseconds.ConvertInt64().ToString() });

                result = extendResult == 1 ? DistributedLockResult.Success : extendResult == -1 ? DistributedLockResult.Conflicted : DistributedLockResult.Error;
            }
            catch (Exception ex)
            {
                LogUtility.Error(ex, "Error extending lock instance {host}: {ex.Message}", host, ex.Message);
                result = DistributedLockResult.Error;
            }
            return result;
        }

        private DistributedLockSummary Extend()
        {
            var extendResults = new ConcurrentBag<DistributedLockResult>();

            Parallel.ForEach(_redisCaches, cache =>
            {
                extendResults.Add(ExtendInstance(cache));
            });

            return PopulateLockResult(extendResults);
        }

        private void StartAutoExtendTimer()
        {
            var interval = _expiryTime.TotalMilliseconds / 2;

            _lockKeepaliveTimer = new Timer(
                _ =>
                {
                    try
                    {
                        var stopwatch = Stopwatch.StartNew();
                        var extendSummary = Extend();
                        var validityTicks = GetRemainingValidityTicks(stopwatch);
                        if (extendSummary.Acquired >= _quorum && validityTicks > 0)
                        {
                            Status = DistributedLockStatus.Acquired;
                            InstanceSummary = extendSummary;
                            ExtendCount++;
                        }
                        else
                        {
                            Status = GetFailedLockStatus(extendSummary);
                            InstanceSummary = extendSummary;
                            LogUtility.Warn("Failed to extend lock, {status} ({instabce}): {resource} ({id})", Status, InstanceSummary, Resource, LockId);
                        }
                    }
                    catch (Exception exception)
                    {
                        // All we can do here is log the exception and swallow it.
                        LogUtility.Error(exception, "Lock renewal timer thread failed: {resource} ({id})", Resource, LockId);
                    }
                }, null, (int)interval, (int)interval);
        }

        private void Start()
        {
            if (_waitTime.HasValue && _waitTime.Value.TotalMilliseconds > 0)
            {
                var stopwatch = Stopwatch.StartNew();

                // ReSharper disable PossibleInvalidOperationException
                while (!IsAcquired && stopwatch.Elapsed <= _waitTime.Value)
                {
                    (Status, InstanceSummary) = Acquire();

                    if (!IsAcquired)
                    {
                        Task.Delay(_retryInterval, _cancellationToken).Wait(_cancellationToken);
                    }
                }
            }
            else
            {
                (Status, InstanceSummary) = Acquire();
            }

            if (IsAcquired && _keepLive)
            {
                StartAutoExtendTimer();
            }
        }


        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                lock (_lockObject)
                {
                    if (_lockKeepaliveTimer != null)
                    {
                        _lockKeepaliveTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        _lockKeepaliveTimer.Dispose();
                        _lockKeepaliveTimer = null;
                    }
                }
            }

            Unlock();

            Status = DistributedLockStatus.Unlocked;
            InstanceSummary = new DistributedLockSummary();
            isDisposed = true;
        }
        #endregion
    }

    public class DistributedLockProfile
    {
        /// <summary>
        /// 分布式锁关键标识
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 不断尝试获取锁的超时的时间;null 表示 仅尝试"一次"获取锁,若失败就放弃尝试。
        /// </summary>
        public TimeSpan? WaitTime { get; set; }

        /// <summary>
        /// 定义任务执行最大超时时间（即任务锁最多保留的时间，默认值10s）,默认情况下任务锁被获取，任务执行完毕立即释放。但是若任务执行期间异常等情况，任务锁未被释放并被一直保留在分布式内存中。
        /// 另外任务锁保留期间,任何对锁的获取都会超时失败。
        /// WaitTime>=ExpiryTime,这样就能保证锁一定能被获取到，无非等待时间较长，具体看业务情况而定。
        /// </summary>
        public TimeSpan ExpiryTime { get; set; } = new TimeSpan(0, 0, 10);

        /// <summary>
        /// 重试获取锁时间间隔(ms)，默认为50ms
        /// </summary>
        public int RetryInterval { get; set; } = 50;

        /// <summary>
        /// 检测进程是否保持激活
        /// </summary>
        public bool KeepLive { get; set; } = true;

        /// <summary>
        /// value值标识，当标识不为空的时候，redis服务器原有的标识和其标识一致，将强制获取锁。
        /// </summary>
        public string LockId { get; set; }
    }
}
