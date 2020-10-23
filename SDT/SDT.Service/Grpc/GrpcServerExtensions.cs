using AspectCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace SDT.Service
{
    public static class GrpcServerExtensions
    {
        public static IServiceCollection AddGrpcHostService<T>(this IServiceCollection services)
            where T : GrpcHostService => services.AddHostedService<T>();

        public static IServiceCollection AddGrpcHostService<T>(this IServiceCollection services, Func<IServiceProvider, T> implementationFactory)
    where T : GrpcHostService => services.AddHostedService<T>(implementationFactory);

        public static IServiceContext AddGrpcHostService<T>(this IServiceContext services)
    where T : GrpcHostService => services.AddType<IHostedService, T>();

        public static IServiceContext AddGrpcHostService<T>(this IServiceContext services, Func<IServiceResolver, T> implementationFactory)
    where T : GrpcHostService => services.AddDelegate<IHostedService>(implementationFactory);

        public static IServiceCollection AddGrpcTracer<T>(this IServiceCollection services)
            where T : class, IServerTracer
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IServerTracer, T>();
            return services;
        }
    }
}
