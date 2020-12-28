namespace CPC.Extensions
{
    public class ConsulSettings
    {
        public string ConsulUrl { get; set; }

        public string ServiceName { get; set; }

        public string ServiceAddress { get; set; }

        public string CheckScheme { get; set; } = "http";

        public string CheckAddress { get; set; }

        public int CheckAfter { get; set; } = 5;

        public int CheckInterval { get; set; } = 10;

        public int CheckTimeout { get; set; } = 10;

        public string GrpcAddress { get; set; }
    }
}
