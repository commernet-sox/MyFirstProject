namespace CPC.Redis.Lock
{
    public enum DistributedLockResult
    {
        Success,
        Conflicted,
        Error
    }
}
