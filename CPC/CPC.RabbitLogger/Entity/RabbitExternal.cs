using CPC.EventBus;

namespace CPC.Logger
{
    public class RabbitExternal : RabbitSettings
    {
        public string Exchangename { get; set; }

        public string Queuename { get; set; }

        public string FileType { get; set; }
    }
}
