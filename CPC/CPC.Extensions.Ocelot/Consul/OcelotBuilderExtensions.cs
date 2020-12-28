using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.ServiceDiscovery;

namespace CPC.Extensions
{
    public static partial class OcelotBuilderExtensions
    {
        public static IOcelotBuilder AddStableConsul(this IOcelotBuilder builder)
        {
            builder.AddConsul();
            builder.Services.RemoveAll<ServiceDiscoveryFinderDelegate>();
            builder.Services.AddSingleton<ServiceDiscoveryFinderDelegate>(StableConsulProviderFactory.Get);
            return builder;
        }
    }
}
