using AspectCore.DependencyInjection;
using System;

namespace CPC.GrpcCore
{
    public static class GrpcServerExtensions
    {
        public static IServiceContext AddGrpcHostService<T>(this IServiceContext services)
    where T : GrpcHostService => services.AddType<IEngineService, T>();

        public static IServiceContext AddGrpcHostService<T>(this IServiceContext services, Func<IServiceResolver, T> implementationFactory)
    where T : GrpcHostService => services.AddDelegate(implementationFactory);
    }
}
