namespace SDT.Service
{
    public class IpCounterKeyBuilder : ICounterKeyBuilder
    {
        private readonly IpRateLimitOptions _options;

        public IpCounterKeyBuilder(IpRateLimitOptions options) => _options = options;

        public string Build(ClientRequestIdentity requestIdentity, RateLimitRule rule) => $"{_options.RateLimitCounterPrefix}_{requestIdentity.ClientIp}_{rule.Period}";
    }
}
