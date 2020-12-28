namespace CPC.Service.RateLimit
{
    public interface ICounterKeyBuilder
    {
        string Build(ClientRequestIdentity requestIdentity, RateLimitRule rule);
    }
}
