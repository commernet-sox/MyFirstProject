using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPC.Service.RateLimit
{
    public class DistributedCacheClientPolicyStore : DistributedCacheRateLimitStore<ClientRateLimitPolicy>, IClientPolicyStore
    {
        private readonly ClientRateLimitOptions _options;
        private readonly List<ClientRateLimitPolicy> _policies;

        public DistributedCacheClientPolicyStore(
            IDistributedCache cache,
            IOptions<ClientRateLimitOptions> options = null,
            IOptions<List<ClientRateLimitPolicy>> policies = null) : base(cache)
        {
            _options = options?.Value;
            _policies = policies?.Value;
        }

        public async Task SeedAsync()
        {
            // on startup, save the IP rules defined in appsettings
            if (_options != null && _policies != null)
            {
                foreach (var rule in _policies)
                {
                    await SetAsync($"{_options.ClientPolicyPrefix}_{rule.ClientId}", new ClientRateLimitPolicy { ClientId = rule.ClientId, Rules = rule.Rules }).ConfigureAwait(false);
                }
            }
        }
    }
}
