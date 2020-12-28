using Consul;
using System;
using System.Linq;
using System.Net;
using System.Timers;

namespace CPC.GrpcCore
{
    public class ServerRegister
    {
        #region Members
        private readonly ConsulClient _client;
        private readonly object _locker = new object();
        private readonly Timer _selfCheckTimer;
        #endregion

        #region Constructors
        public ServerRegister(string consulAddress)
        {
            if (string.IsNullOrEmpty(consulAddress))
            {
                throw new ArgumentNullException(nameof(consulAddress));
            }

            _client = new ConsulClient((cfg) =>
            {
                var uriBuilder = new UriBuilder(consulAddress);
                cfg.Address = uriBuilder.Uri;
            });

            _selfCheckTimer = new Timer(ConsulTimespan.SelfCheckInterval.TotalSeconds * 1000);
        }
        #endregion

        #region Methods
        public void Register(GrpcServiceSettings settings, Action<ConsulEntry> registered)
        {
            if (_client == null)
            {
                throw new ArgumentNullException($"consul client");
            }

            var registerResult = RegisterService(settings, registered);
            if (!registerResult)
            {
                throw new Exception($"failed to register service {settings}");
            }

            InitIntervalSelfCheckTimer(settings);
        }

        /// <summary>
        /// 初始化Timer
        /// </summary>
        private void InitIntervalSelfCheckTimer(GrpcServiceSettings settings)
        {
            _selfCheckTimer.Elapsed += (sender, e) =>
            {
                lock (_locker)
                {
                    _selfCheckTimer.Stop();
                    DoSelfCheck(settings);
                    _selfCheckTimer.Start();
                }
            };
            _selfCheckTimer.Start();
        }

        /// <summary>
        /// 做反向检查
        /// </summary>
        private void DoSelfCheck(GrpcServiceSettings settings)
        {
            if (_client == null)
            {
                return;
            }

            try
            {
                var response = _client.Health.Service(settings.ServiceName, "", true).Result;
                var servcieId = settings.ToString();
                var serviceEntry = response?.Response?.FirstOrDefault(oo => oo?.Service?.ID == servcieId);
                if (serviceEntry == null)
                {
                    RegisterService(settings);
                }
            }
            catch
            {
                //异常忽略
            }
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="registered">注册成功后执行</param>
        /// <returns></returns>
        private bool RegisterService(GrpcServiceSettings settings, Action<ConsulEntry> registered = null)
        {
            if (settings.Host.IsNull())
            {
                settings.Host = IPUtility.GetLocalIntranetIP().ToString();
            }

            var serviceId = settings.ToString();
            var checkName = $"Check:" + serviceId;
            var acr = new AgentCheckRegistration
            {
                Name = checkName,
                TCP = $"{settings.Host}:{settings.Port}",
                Interval = ConsulTimespan.CheckInterval,
                Status = HealthStatus.Passing,
                DeregisterCriticalServiceAfter = ConsulTimespan.CriticalInterval,
                ServiceID = serviceId,
            };

            var asr = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = settings.ServiceName,
                Address = settings.Host,
                Port = settings.Port,
                Check = acr
            };

            var res = _client.Agent.ServiceRegister(asr).Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            registered?.Invoke(new ConsulEntry(this, serviceId));
            return true;
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <param name="serviceId"></param>
        public void Deregister(string serviceId)
        {
            _selfCheckTimer?.Stop();
            _selfCheckTimer?.Dispose();
            _client?.Agent?.ServiceDeregister(serviceId).GetAwaiter().GetResult();
        }
        #endregion
    }
}
