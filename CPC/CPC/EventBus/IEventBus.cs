using System.Threading.Tasks;

namespace CPC.EventBus
{
    public interface IEventBus
    {
        Task Publish<T>(T @event)
            where T : IntegrationEvent;

        Task<TResponse> Send<TEvent, TResponse>(TEvent @event)
            where TEvent : IntegrationEvent;

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler;

        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler
            where T : IntegrationEvent;
    }
}
