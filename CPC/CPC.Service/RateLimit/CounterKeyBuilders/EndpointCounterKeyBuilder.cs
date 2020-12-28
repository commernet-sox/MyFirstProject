﻿namespace CPC.Service.RateLimit
{
    public class EndpointCounterKeyBuilder : ICounterKeyBuilder
    {
        public string Build(ClientRequestIdentity requestIdentity, RateLimitRule rule) =>
            // This will allow to rate limit /api/values/1 and api/values/2 under same counter
            $"_{rule.Endpoint}";
    }
}
