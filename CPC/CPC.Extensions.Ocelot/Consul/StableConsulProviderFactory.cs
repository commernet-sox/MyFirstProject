using Microsoft.Extensions.DependencyInjection;
using Ocelot.Logging;
using Ocelot.Provider.Consul;
using Ocelot.ServiceDiscovery;

namespace CPC.Extensions
{
    public class StableConsulProviderFactory
    {
        public static ServiceDiscoveryFinderDelegate Get = (provider, config, route) =>
        {
            var factory = provider.GetService<IOcelotLoggerFactory>();

            var consulFactory = provider.GetService<IConsulClientFactory>();

            var consulRegistryConfiguration = new ConsulRegistryConfiguration(config.Scheme, config.Host, config.Port, route.ServiceName, config.Token);

            var originalConsul = new Ocelot.Provider.Consul.Consul(consulRegistryConfiguration, factory, consulFactory);

            var consulServiceDiscoveryProvider = new Consul(route.ServiceName, factory, originalConsul);

            if (config.Type?.ToLower() == "pollconsul")
            {
                return new PollConsul(config.PollingInterval, factory, consulServiceDiscoveryProvider);
            }

            return consulServiceDiscoveryProvider;
        };
    }
}
