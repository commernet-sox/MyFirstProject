namespace CPC.Redis.Lock
{
    public enum DistributedLockStatus
    {
        /// <summary>
        /// 尚未成功获取或释放锁
        /// </summary>
        Unlocked,

        /// <summary>
        /// 已成功获取锁
        /// </summary>
        Acquired,

        /// <summary>
        /// 未获取锁，因为没有资源可用
        /// </summary>
        NoQuorum,

        /// <summary>
        /// 未获取该锁，因为它当前被另一个LockId锁定
        /// </summary>
        Conflicted,

        /// <summary>
        /// 已过期
        /// </summary>
        Expired

    }
}
