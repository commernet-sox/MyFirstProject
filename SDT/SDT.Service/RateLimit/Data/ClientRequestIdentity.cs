namespace SDT.Service
{
    public class ClientRequestIdentity
    {
        public string ClientIp { get; set; }

        public string ClientId { get; set; }

        public string Path { get; set; }

        public string HttpVerb { get; set; }
    }
}
