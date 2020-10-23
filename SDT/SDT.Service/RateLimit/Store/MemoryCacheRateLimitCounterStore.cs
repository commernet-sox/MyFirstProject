using Microsoft.Extensions.Caching.Memory;

namespace SDT.Service
{
    public class MemoryCacheRateLimitCounterStore : MemoryCacheRateLimitStore<RateLimitCounter?>, IRateLimitCounterStore
    {
        public MemoryCacheRateLimitCounterStore(IMemoryCache cache) : base(cache)
        {
        }
    }
}
