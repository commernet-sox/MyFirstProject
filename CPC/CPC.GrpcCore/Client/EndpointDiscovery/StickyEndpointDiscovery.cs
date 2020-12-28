﻿using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 注册中心服务发现
    /// </summary>
    internal sealed class StickyEndpointDiscovery : IEndpointDiscovery
    {
        #region Members
        private readonly ConsulClient _client;

        public string ServiceName { get; set; }

        public Action Watched { get; set; }
        #endregion

        #region Constructors
        public StickyEndpointDiscovery(string serviceName, string address, bool startWatch = true)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("consul address");
            }

            _client = new ConsulClient((cfg) =>
            {
                var uriBuilder = new UriBuilder(address);
                cfg.Address = uriBuilder.Uri;
            });

            ServiceName = serviceName;

            if (startWatch)
            {
                StartWatchService();
            }
        }
        #endregion

        #region Methods
        public List<string> FindServiceEndpoints(bool filterBlack = true)
        {
            if (_client == null)
            {
                throw new ArgumentNullException("consul client");
            }

            var targets = new List<string>();
            try
            {
                var r = _client.Health.Service(ServiceName, "", true).Result;
                if (r.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException($"failed to query consul server");
                }

                targets = r.Response
                           .Select(x => $"{x.Service.Address}:{x.Service.Port}")
                           .Where(target => !ServiceBlackPolicy.In(ServiceName, target) || !filterBlack)
                           .ToList();
            }
            catch { }
            return targets;
        }

        /// <summary>
        /// 开始监听服务变动
        /// </summary>
        private void StartWatchService() => Task.Factory.StartNew(async () =>
                                          {
                                              ulong lastWaitIndex = 0;
                                              do
                                              {
                                                  try
                                                  {
                                                      var serviceQueryResult = await _client.Catalog.Service(ServiceName, "", new QueryOptions()
                                                      {
                                                          WaitTime = TimeSpan.FromSeconds(30),
                                                          WaitIndex = lastWaitIndex
                                                      });
                                                      var waitIndex = serviceQueryResult.LastIndex;
                                                      if (lastWaitIndex <= 0)
                                                      {
                                                          lastWaitIndex = waitIndex;
                                                          continue;
                                                      }
                                                      if (waitIndex == lastWaitIndex)
                                                      {
                                                          continue;
                                                      }

                                                      // 重置服务
                                                      lastWaitIndex = waitIndex;
                                                      if (Watched != null)
                                                      {
                                                          Watched.Invoke();
                                                      }
                                                  }
                                                  catch { }
                                              } while (true);
                                          });
        #endregion
    }
}