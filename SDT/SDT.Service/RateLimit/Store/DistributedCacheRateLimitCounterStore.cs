using Microsoft.Extensions.Caching.Distributed;

namespace SDT.Service
{
    public class DistributedCacheRateLimitCounterStore : DistributedCacheRateLimitStore<RateLimitCounter?>, IRateLimitCounterStore
    {
        public DistributedCacheRateLimitCounterStore(IDistributedCache cache) : base(cache)
        {
        }
    }
}
