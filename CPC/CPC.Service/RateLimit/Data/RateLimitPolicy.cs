using System.Collections.Generic;

namespace CPC.Service.RateLimit
{
    public class RateLimitPolicy
    {
        public List<RateLimitRule> Rules { get; set; } = new List<RateLimitRule>();
    }
}
