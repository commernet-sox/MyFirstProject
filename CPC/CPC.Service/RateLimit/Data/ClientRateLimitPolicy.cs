namespace CPC.Service.RateLimit
{
    public class ClientRateLimitPolicy : RateLimitPolicy
    {
        public string ClientId { get; set; }
    }
}
