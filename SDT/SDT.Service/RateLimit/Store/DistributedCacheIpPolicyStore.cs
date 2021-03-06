﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDT.Service
{
    public class DistributedCacheIpPolicyStore : DistributedCacheRateLimitStore<List<IpRateLimitPolicy>>, IIpPolicyStore
    {
        private readonly IpRateLimitOptions _options;
        private readonly List<IpRateLimitPolicy> _policies;

        public DistributedCacheIpPolicyStore(
            IDistributedCache cache,
            IOptions<IpRateLimitOptions> options = null,
            IOptions<List<IpRateLimitPolicy>> policies = null) : base(cache)
        {
            _options = options?.Value;
            _policies = policies?.Value;
        }

        public async Task SeedAsync()
        {
            // on startup, save the IP rules defined in appsettings
            if (_options != null && _policies != null)
            {
                await SetAsync($"{_options.IpPolicyPrefix}", _policies).ConfigureAwait(false);
            }
        }
    }
}
