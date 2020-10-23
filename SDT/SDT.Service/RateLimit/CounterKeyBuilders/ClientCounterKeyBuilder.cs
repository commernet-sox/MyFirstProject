namespace SDT.Service
{
    public class ClientCounterKeyBuilder : ICounterKeyBuilder
    {
        private readonly ClientRateLimitOptions _options;

        public ClientCounterKeyBuilder(ClientRateLimitOptions options) => _options = options;

        public string Build(ClientRequestIdentity requestIdentity, RateLimitRule rule) => $"{_options.RateLimitCounterPrefix}_{requestIdentity.ClientId}_{rule.Period}";
    }
}
