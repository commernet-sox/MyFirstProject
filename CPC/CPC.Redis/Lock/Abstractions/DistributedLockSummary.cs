namespace CPC.Redis.Lock
{
    public struct DistributedLockSummary
    {
        public DistributedLockSummary(int acquired, int conflicted, int error)
        {
            Acquired = acquired;
            Conflicted = conflicted;
            Error = error;
        }

        public readonly int Acquired;
        public readonly int Conflicted;
        public readonly int Error;

        public override string ToString() => $"Acquired: {Acquired}, Conflicted: {Conflicted}, Error: {Error}";
    }
}
