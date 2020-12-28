namespace CPC.EventBus
{
    public interface IRabbitIntegrationEventHandler<in TEvent> : IIntegrationEventHandler<TEvent>
        where TEvent : IntegrationEvent
    {
        RabbitMessageResult HandlerResult { get; }
    }
}
