namespace SDT.Redis.Lock
{
    public enum DistributedLockResult
    {
        Success,
        Conflicted,
        Error
    }
}
