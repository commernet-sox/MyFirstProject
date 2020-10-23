using System;

namespace SDT.Service
{
    public struct RateLimitCounter
    {
        public DateTime Timestamp { get; set; }

        public double Count { get; set; }
    }
}
