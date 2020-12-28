using CPC.EventBus;
using System.Collections.Generic;

namespace CPC.Logger
{

    public class RabbitEntity<T> : RabbitIntegrationEvent
    {
        public List<T> Contexts { get; set; }
    }

    public class RabbitLogProducerEntity<T> : IntegrationEvent
    {
        public T Data { get; set; }
    }
}
