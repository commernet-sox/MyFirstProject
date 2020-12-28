using AspectCore.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace CPC.EventBus
{
    public static class HereExtensions
    {
        public static IServiceContext AddHereEventBus(this IServiceContext services, params Assembly[] assemblies)
        {
            if (assemblies.IsNull())
            {
                throw new ArgumentException("no assemblies found to scan. Supply at least one assembly to scan for handlers.");
            }

            var openTypes = new[]{
                typeof(IIntegrationEventHandler<,>),
                typeof(IIntegrationEventHandler<>)
            };

            foreach (var serviceType in openTypes)
            {
                services.AddAssemblyTypes(serviceType, assemblies);
            }

            services.RemoveAll<IEventBus>();
            services.AddType<IEventBus, HereEventBus>();

            return services;
        }

        public static IServiceContext AddHereEventBus(this IServiceContext services, params string[] assemblyNames) => services.AddHereEventBus(TypeFinderUtility.GetAssemblies(assemblyNames).ToArray());

        public static IServiceContext AddHerePipeline(this IServiceContext services, Type pipelineType)
        {
            if (pipelineType == null)
            {
                throw new ArgumentNullException(nameof(pipelineType));
            }

            if (pipelineType.IsInterface || !pipelineType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"error in {nameof(pipelineType)} type");
            }

            var types = new Type[] { typeof(IHerePipeline<>), typeof(IHerePipeline<,>) };
            var interfaces = pipelineType.GetInterfaces();
            foreach (var implInterface in interfaces)
            {
                if (!implInterface.IsGenericType)
                {
                    continue;
                }

                var genericTypeDefinition = implInterface.GetGenericTypeDefinition();

                foreach (var type in types)
                {
                    if (type.IsAssignableFrom(genericTypeDefinition))
                    {
                        services.AddType(type, pipelineType);
                        return services;
                    }
                }
            }

            throw new ArgumentException($"error in {nameof(pipelineType)} type");
        }

        public static IServiceContext AddHerePipeline<TEvent>(this IServiceContext services, Func<IServiceResolver, IHerePipeline<TEvent>> pipelineImpl)
            where TEvent : IntegrationEvent
        {
            services.AddDelegate(pipelineImpl);
            return services;
        }

        public static IServiceContext AddHerePipeline<TEvent, TResponse>(this IServiceContext services, Func<IServiceResolver, IHerePipeline<TEvent, TResponse>> pipelineImpl)
     where TEvent : IntegrationEvent
        {
            services.AddDelegate(pipelineImpl);
            return services;
        }
    }
}
