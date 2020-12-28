using System;

namespace CPC.EventBus
{
    public interface IQueueProducer : IDisposable
    {
        void Publish<T>(T @event)
            where T : IntegrationEvent;
    }
}
