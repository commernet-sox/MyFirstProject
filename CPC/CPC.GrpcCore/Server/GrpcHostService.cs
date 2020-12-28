using CPC.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CPC.GrpcCore
{
    public abstract class GrpcHostService : IEngineService
    {
        protected virtual IServerTracer Tracer { get; } = new ServerMockTracer();

        protected virtual List<Interceptor> Interceptors { get; }

        protected virtual List<ChannelOption> ChannelOptions { get; }

        protected virtual ServerCredentials Credentials { get; }

        protected virtual string ConsulAddress { get; private set; }

        protected virtual GrpcServiceSettings ServiceSettings { get; set; }

        protected abstract IEnumerable<ServerServiceDefinition> CreateDefinitions();

        public void Start()
        {
            if (ConsulAddress.IsNull())
            {
                var settings = Singleton<IConfiguration>.Instance.GetSection(nameof(ConsulSettings)).Get<ConsulSettings>() ?? new ConsulSettings();
                ConsulAddress = settings.ConsulUrl;
                if (settings.ServiceAddress.IsNull() || settings.ServiceAddress.Contains("*"))
                {
                    settings.GrpcAddress = settings.GrpcAddress.Replace("*", IPUtility.GetLocalIntranetIP().ToString());
                }
                else if (!settings.GrpcAddress.IsNull() && settings.GrpcAddress.Contains("*"))
                {
                    var serviceIp = IPUtility.Parse(settings.ServiceAddress);
                    settings.GrpcAddress = settings.GrpcAddress.Replace("*", serviceIp.Address.ToString());
                }
                var ip = IPUtility.Parse(settings.GrpcAddress);
                ServiceSettings = new GrpcServiceSettings
                {
                    ServiceName = "Grpc:" + settings.ServiceName,
                    Host = ip.Address.ToString(),
                    Port = ip.Port
                };
            }
            else if (ServiceSettings == null)
            {
                ServiceSettings = Singleton<IConfiguration>.Instance.GetSection(nameof(GrpcServiceSettings)).Get<GrpcServiceSettings>() ?? new GrpcServiceSettings();
            }

            GrpcServiceManager.Start(CreateDefinitions(), ServiceSettings, ConsulAddress, Tracer, Credentials, Interceptors, ChannelOptions, OnException);
        }

        public void Stop() => GrpcServiceManager.Stop(OnException);

        protected virtual void OnException(Exception exception) => LogUtility.Error(exception, nameof(GrpcHostService));
    }
}
