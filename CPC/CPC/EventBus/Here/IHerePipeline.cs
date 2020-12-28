using System.Threading.Tasks;

namespace CPC.EventBus
{
    public interface IHerePipeline<in TEvent>
        where TEvent : IntegrationEvent
    {
        Task Invoke(TEvent @event, HereEventDelegate next);
    }

    public interface IHerePipeline<in TEvent, TResponse>
        where TEvent : IntegrationEvent
    {
        Task<TResponse> Invoke(TEvent @event, HereEventDelegate<TResponse> next);
    }
}
