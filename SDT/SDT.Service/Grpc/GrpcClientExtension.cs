using AspectCore.DependencyInjection;
using CPC.GrpcCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace SDT.Service
{
    public static class GrpcClientExtension
    {
        public static IServiceCollection AddGrpcClient(this IServiceCollection services, GrpcDiscovery discovery = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (discovery == null)
            {
                discovery = Singleton<IConfiguration>.Instance.Bind<GrpcDiscovery>();
            }
            services.TryAddSingleton(discovery);
            services.TryAddTransient<IClientTracer, ClientMockTracer>();
            services.TryAddSingleton(typeof(IGrpcClient<>), typeof(GrpcClient<>));
            services.TryAddSingleton(typeof(IGrpcClientFactory<>), typeof(GrpcClientFactory<>));
            return services;
        }

        public static IServiceCollection AddGrpcClient<T>(this IServiceCollection services, GrpcDiscovery discovery = null)
            where T : class, IClientTracer
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IClientTracer, T>();
            services.AddGrpcClient(discovery);
            return services;
        }

        public static IServiceContext AddGrpcClient(this IServiceContext services, GrpcDiscovery discovery = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (discovery == null)
            {
                discovery = Singleton<IConfiguration>.Instance.Bind<GrpcDiscovery>();
            }
            services.AddGrpcClientCore(discovery);
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
            services.AddGrpcClient(discovery);
            return services;
        }
    }
}
