namespace SDT.Service
{
    public class ClientRateLimitPolicy : RateLimitPolicy
    {
        public string ClientId { get; set; }
    }
}
