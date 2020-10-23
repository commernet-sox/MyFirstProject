using System.Collections.Generic;

namespace SDT.Service
{
    public class RateLimitPolicy
    {
        public List<RateLimitRule> Rules { get; set; } = new List<RateLimitRule>();
    }
}
