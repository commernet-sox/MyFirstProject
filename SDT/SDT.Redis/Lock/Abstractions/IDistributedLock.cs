using System;

namespace SDT.Redis.Lock
{
    public interface IDistributedLock : IDisposable
    {
        /// <summary>
        /// 锁定的资源名称
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// 分配给此锁的唯一标识符
        /// </summary>
        string LockId { get; }

        /// <summary>
        /// 是否已获取锁
        /// </summary>
        bool IsAcquired { get; }

        /// <summary>
        /// 锁的状态🔒
        /// </summary>
        DistributedLockStatus Status { get; }

        /// <summary>
        /// 获取锁的实例的详细信息
        /// </summary>
        DistributedLockSummary InstanceSummary { get; }

        /// <summary>
        /// 扩展锁的次数
        /// </summary>
        int ExtendCount { get; }
    }
}
