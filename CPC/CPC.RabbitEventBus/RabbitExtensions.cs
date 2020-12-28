using AspectCore.DependencyInjection;
using CPC.Logger;
using RabbitMQ.Client;
using System;

namespace CPC.EventBus
{
    public static class RabbitExtensions
    {
        public static void Operator(this IRabbitConnection connection, Action<IModel> setup)
        {
            if (!connection.IsConnected)
            {
                connection.TryConnect();
            }

            using (var channel = connection.CreateModel())
            {
                setup?.Invoke(channel);
            }
        }

        public static IServiceContext AddRabbitEventBus(this IServiceContext serviceContext, IConnectionFactory connection, IEventBusSubscriptionsManager subsManager, string subscriptionClientName, int retryCount = 5, ILogger logger = null)
        {
            serviceContext.TryAddInstance<IRabbitConnection>(new RabbitConnection(connection, retryCount, logger));
            serviceContext.TryAddInstance(subsManager);
            serviceContext.TryAddDelegate<IEventBus>(s => new RabbitEventBus(s.Resolve<IRabbitConnection>(), s.Resolve<IEventBusSubscriptionsManager>(), subscriptionClientName, retryCount, logger), Lifetime.Singleton);
            return serviceContext;
        }

        public static IServiceContext AddRabbitEventBus(this IServiceContext serviceContext, IConnectionFactory connection, string subscriptionClientName, int retryCount = 5, ILogger logger = null) => serviceContext.AddRabbitEventBus(connection, new InMemoryEventBusSubscriptionsManager(), subscriptionClientName, retryCount, logger);

        public static IServiceContext AddRabbitEventBus(this IServiceContext serviceContext, RabbitSettings settings, IEventBusSubscriptionsManager subsManager, string subscriptionClientName, Action<IConnectionFactory> setup = null, ILogger logger = null)
        {
            serviceContext.TryAddInstance<IRabbitConnection>(new RabbitConnection(settings, setup, logger));
            serviceContext.TryAddInstance(subsManager);
            serviceContext.TryAddDelegate<IEventBus>(s => new RabbitEventBus(s.Resolve<IRabbitConnection>(), s.Resolve<IEventBusSubscriptionsManager>(), subscriptionClientName, settings.RetryCount, logger), Lifetime.Singleton);
            return serviceContext;
        }

        public static IServiceContext AddRabbitEventBus(this IServiceContext serviceContext, RabbitSettings settings, string subscriptionClientName, Action<IConnectionFactory> setup = null, ILogger logger = null) => serviceContext.AddRabbitEventBus(settings, new InMemoryEventBusSubscriptionsManager(), subscriptionClientName, setup, logger);
    }
}
