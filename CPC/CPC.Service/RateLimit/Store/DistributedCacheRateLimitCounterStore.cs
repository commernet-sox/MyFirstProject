using Microsoft.Extensions.Caching.Distributed;

namespace CPC.Service.RateLimit
{
    public class DistributedCacheRateLimitCounterStore : DistributedCacheRateLimitStore<RateLimitCounter?>, IRateLimitCounterStore
    {
        public DistributedCacheRateLimitCounterStore(IDistributedCache cache) : base(cache)
        {
        }
    }
}
