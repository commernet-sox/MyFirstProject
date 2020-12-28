using Ocelot.Logging;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPC.Extensions
{
    public class Consul : IServiceDiscoveryProvider
    {
        private readonly string _serviceName;
        private readonly IOcelotLogger _logger;
        private readonly IServiceDiscoveryProvider _consulServiceDiscoveryProvider;
        private readonly ConcurrentDictionary<string, List<Service>> _services = new ConcurrentDictionary<string, List<Service>>();

        public Consul(string serviceName, IOcelotLoggerFactory factory, IServiceDiscoveryProvider consulServiceDiscoveryProvider)
        {
            _serviceName = serviceName;
            _logger = factory.CreateLogger<Consul>();
            _consulServiceDiscoveryProvider = consulServiceDiscoveryProvider;
        }

        public async Task<List<Service>> Get()
        {
            var list = new List<Service>();
            try
            {
                list = await _consulServiceDiscoveryProvider.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError("there was a problem accessing the consult service", ex);
            }

            //consul 服务出现问题，使用本地缓存的连接
            if (list.IsNull())
            {
                _services.TryGetValue(_serviceName, out list);
                return list;
            }

            _services.AddOrUpdate(_serviceName, list, (_, v) =>
            {
                v.Clear();
                return list;
            });
            return list;
        }
    }
}
