using Microsoft.Extensions.Caching.Distributed;
using SDT.BaseTool;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.Service
{
    public class DistributedCacheRateLimitStore<T> : IRateLimitStore<T>
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheRateLimitStore(IDistributedCache cache) => _cache = cache;

        public Task SetAsync(string id, T entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions();

            if (expirationTime.HasValue)
            {
                options.SetAbsoluteExpiration(expirationTime.Value);
            }

            return _cache.SetStringAsync(id, entry.ToJsonEx(), options, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            var stored = await _cache.GetStringAsync(id, cancellationToken);

            return !string.IsNullOrEmpty(stored);
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var stored = await _cache.GetStringAsync(id, cancellationToken);

            if (!string.IsNullOrEmpty(stored))
            {
                return stored.ToDataEx<T>();
            }

            return default;
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken = default) => _cache.RemoveAsync(id, cancellationToken);
    }
}
