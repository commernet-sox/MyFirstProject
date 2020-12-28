using AspectCore.DependencyInjection;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace CPC.GrpcCore
{
    public static class GrpcExtensions
    {
        public static IServiceContext AddGrpcClientCore(this IServiceContext services, GrpcDiscovery discovery = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (discovery == null)
            {
                discovery = Singleton<IConfiguration>.Instance.GetSection(nameof(GrpcDiscovery)).Get<GrpcDiscovery>() ?? new GrpcDiscovery();
            }

            services.TryAddInstance(discovery);
            services.TryAddType<IClientTracer, ClientMockTracer>();
            services.TryAddType(typeof(IGrpcClient<>), typeof(GrpcClient<>), Lifetime.Singleton);
            services.TryAddType(typeof(IGrpcClientFactory<>), typeof(GrpcClientFactory<>));
            return services;
        }

        public static IServiceContext AddGrpcClient<T>(this IServiceContext services, GrpcDiscovery discovery = null)
            where T : IClientTracer
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddType<IClientTracer, T>();
            services.AddGrpcClientCore(discovery);
            return services;
        }

        public static IServiceContext AddGrpcClientCredentials(this IServiceContext services, Func<IServiceResolver, ChannelCredentials> implementation)
        {
            services.AddDelegate(implementation);
            return services;
        }

        public static IServiceContext AddGrpcTracer<T>(this IServiceContext services)
            where T : IServerTracer
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddType<IServerTracer, T>();
            return services;
        }
    }
}
