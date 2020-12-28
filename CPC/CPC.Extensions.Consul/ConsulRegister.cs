using Consul;
using System;
using System.Linq;
using System.Net;
using System.Timers;

namespace CPC.Extensions
{
    public class ConsulRegister : IDisposable
    {
        private readonly ConsulSettings _settings;
        private readonly ConsulClient _client;
        private string _serviceId;
        private Timer _selfCheckTimer;
        private readonly object _locker = new object();

        public TimeSpan SelfCheckInterval { get; set; } = TimeSpan.FromSeconds(30);

        public ConsulRegister(ConsulSettings settings)
        {
            _settings = settings;
            _client = new ConsulClient((cfg) =>
            {
                var uriBuilder = new UriBuilder(_settings.ConsulUrl);
                cfg.Address = uriBuilder.Uri;
            });
        }

        public void Register()
        {
            if (_selfCheckTimer != null)
            {
                throw new InvalidOperationException("service is already registered");
            }

            if (!RegisterService())
            {
                throw new Exception($"failed to register service {_settings.ServiceName}");
            }

            _selfCheckTimer = new Timer(SelfCheckInterval.TotalMilliseconds);
            _selfCheckTimer.Elapsed += (sender, e) =>
            {
                lock (_locker)
                {
                    _selfCheckTimer.Stop();
                    DoSelfCheck();
                    _selfCheckTimer.Start();
                }
            };
            _selfCheckTimer.Start();
        }

        private bool RegisterService()
        {
            var ip = IPUtility.Parse(_settings.ServiceAddress);

            var agent = new AgentServiceRegistration()
            {
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(_settings.CheckAfter),
                    Interval = TimeSpan.FromSeconds(_settings.CheckInterval),
                    Timeout = TimeSpan.FromSeconds(_settings.CheckTimeout),
                    HTTP = $"{_settings.CheckScheme}://{_settings.ServiceAddress}/{_settings.CheckAddress}",
                },
                Address = ip.Address.ToString(),
                Port = ip.Port,
                Name = _settings.ServiceName,
                ID = $"{_settings.ServiceName},{_settings.ServiceAddress}"
            };

            _serviceId = agent.ID;
            var result = _client.Agent.ServiceRegister(agent).GetAwaiter().GetResult();
            return result.StatusCode == HttpStatusCode.OK;
        }

        private void DoSelfCheck()
        {
            if (_client == null)
            {
                return;
            }

            try
            {
                if (_serviceId.IsNull())
                {
                    return;
                }

                var response = _client.Health.Service(_settings.ServiceName, "", true).Result;
                var serviceEntry = response?.Response?.FirstOrDefault(s => s?.Service?.ID == _serviceId);
                if (serviceEntry == null)
                {
                    RegisterService();
                }
            }
            catch
            {
                //异常忽略
            }
        }

        public void Dispose()
        {
            _selfCheckTimer?.Stop();
            _selfCheckTimer?.Dispose();
            _client?.Agent?.ServiceDeregister(_serviceId).GetAwaiter().GetResult();
        }
    }
}
