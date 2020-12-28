using AspectCore.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CPC.EventBus
{
    internal class HereEventBus : IEventBus
    {
        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            using (var scope = GlobalContext.CreateScope())
            {
                var handlers = scope.ResolveAll<IIntegrationEventHandler<T>>();
                if (handlers.IsNull())
                {
                    throw new InvalidOperationException("this object is not subscribed");
                }

                Task Handler() => Notifications(handlers, @event);

                var pipelines = scope.ResolveAll<IHerePipeline<T>>();
                var result = pipelines.Reverse().Aggregate((HereEventDelegate)Handler, (next, pipeline) => () => pipeline.Invoke(@event, next))();
                await result;
            }
        }

        private async Task Notifications<T>(IIntegrationEventHandler<T>[] handlers, T @event)
            where T : IntegrationEvent
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(@event).ConfigureAwait(false);
            }
        }

        public Task<TResponse> Send<TEvent, TResponse>(TEvent @event)
            where TEvent : IntegrationEvent
        {
            using (var scope = GlobalContext.CreateScope())
            {

                var handler = scope.Resolve<IIntegrationEventHandler<TEvent, TResponse>>();
                if (handler == null)
                {
                    throw new InvalidOperationException("this object is not subscribed");
                }

                Task<TResponse> Handler() => handler.Handle(@event);

                var pipelines = scope.ResolveAll<IHerePipeline<TEvent, TResponse>>();
                var result = pipelines.Reverse().Aggregate((HereEventDelegate<TResponse>)Handler, (next, pipeline) => () => pipeline.Invoke(@event, next))();
                return result;
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler => throw new NotSupportedException(nameof(HereEventBus));

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler => throw new NotSupportedException(nameof(HereEventBus));
    }
}
