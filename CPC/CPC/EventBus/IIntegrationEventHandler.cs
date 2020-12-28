using System.Threading.Tasks;

namespace CPC.EventBus
{
    public interface IIntegrationEventHandler<in TEvent> : IIntegrationEventHandler
    where TEvent : IntegrationEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IIntegrationEventHandler<in TEvent, TResponse> : IIntegrationEventHandler
        where TEvent : IntegrationEvent
    {
        Task<TResponse> Handle(TEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
