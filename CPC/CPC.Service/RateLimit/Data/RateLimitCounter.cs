using System;

namespace CPC.Service.RateLimit
{
    public struct RateLimitCounter
    {
        public DateTime Timestamp { get; set; }

        public double Count { get; set; }
    }
}
