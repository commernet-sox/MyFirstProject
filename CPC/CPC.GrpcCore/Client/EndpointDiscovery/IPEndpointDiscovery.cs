using System;
using System.Collections.Generic;
using System.Linq;

namespace CPC.GrpcCore
{
    /// <summary>
    /// IP服务发现
    /// </summary>
    internal class IPEndpointDiscovery : IEndpointDiscovery
    {
        #region Members
        private readonly List<GrpcEndpoint> _ipEndPoints;

        public string ServiceName { get; set; }

        public Action Watched { get; set; }
        #endregion

        #region Constructor
        public IPEndpointDiscovery(string serviceName, List<GrpcEndpoint> ipEndPoints)
        {
            if ((ipEndPoints?.Count ?? 0) <= 0)
            {
                throw new ArgumentNullException("no ip endpoints availble");
            }

            _ipEndPoints = ipEndPoints.Where(t => t.ServiceName.EqualsEx(serviceName)).ToList();
            ServiceName = serviceName;
        }
        #endregion

        #region Methods
        public List<string> FindServiceEndpoints(bool filterBlack = true)
        {
            if ((_ipEndPoints?.Count ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException("endpoint not provide");
            }

            var targets = _ipEndPoints.Select(x => $"{x.Host}:{x.Port}")
                                      .Where(target => !ServiceBlackPolicy.In(ServiceName, target) || !filterBlack)
                                      .ToList();
            return targets;
        }
        #endregion
    }
}
