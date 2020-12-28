using Microsoft.Extensions.Caching.Memory;

namespace CPC.Service.RateLimit
{
    public class MemoryCacheRateLimitCounterStore : MemoryCacheRateLimitStore<RateLimitCounter?>, IRateLimitCounterStore
    {
        public MemoryCacheRateLimitCounterStore(IMemoryCache cache) : base(cache)
        {
        }
    }
}
