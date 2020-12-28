using Grpc.Core;
using System;
using System.Collections.Concurrent;

namespace CPC.GrpcCore
{
    /// <summary>
    /// Endpoint 策略工厂
    /// </summary>
    internal class StrategyFactory
    {
        private static readonly ConcurrentDictionary<Type, Exitus> _endpointStrategys = new ConcurrentDictionary<Type, Exitus>();
        private static readonly object _lockHelper = new object();

        /// <summary>
        /// 获取EndpointStrategy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="discovery"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static Exitus Get<T>(GrpcDiscovery discovery, string serviceName)
            where T : ClientBase
        {
            if (_endpointStrategys.TryGetValue(typeof(T), out var exitus) &&
                exitus?.EndpointStrategy != null)
            {
                return exitus;
            }

            lock (_lockHelper)
            {
                if (_endpointStrategys.TryGetValue(typeof(T), out exitus) &&
                    exitus?.EndpointStrategy != null)
                {
                    return exitus;
                }

                IEndpointStrategy strategy;
                if (discovery.ConsulAddress.IsNull() || !discovery.EnableConsul)
                {
                    var iPEndpointDiscovery = new IPEndpointDiscovery(serviceName, discovery.EndPoints);
                    IPEndpointStrategy.Instance.AddServiceDiscovery(iPEndpointDiscovery);
                    strategy = IPEndpointStrategy.Instance;
                }
                else
                {
                    var stickyEndpointDiscovery = new StickyEndpointDiscovery(serviceName, discovery.ConsulAddress);
                    StickyEndpointStrategy.Instance.AddServiceDiscovery(stickyEndpointDiscovery);
                    strategy = StickyEndpointStrategy.Instance;
                }

                exitus = new Exitus(serviceName, discovery.RetryCount, strategy);
                _endpointStrategys.AddOrUpdate(typeof(T), exitus, (k, v) => exitus);
                return exitus;
            }
        }
    }
}
