namespace CPC.EventBus
{
    public class RabbitSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Virtual { get; set; }

        public int RetryCount { get; set; } = 5;
    }
}
