namespace CPC.GrpcCore
{
    public class GrpcServiceSettings
    {
        public string ServiceName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public override string ToString() => $"{ServiceName},{Host},{Port}";
    }
}
