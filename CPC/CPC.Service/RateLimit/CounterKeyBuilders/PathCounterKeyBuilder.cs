namespace CPC.Service.RateLimit
{
    public class PathCounterKeyBuilder : ICounterKeyBuilder
    {
        public string Build(ClientRequestIdentity requestIdentity, RateLimitRule rule) => $"_{requestIdentity.HttpVerb}_{requestIdentity.Path}";
    }
}
