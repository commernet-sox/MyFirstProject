
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SDT.BaseTool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.Service
{
    public abstract class GrpcHostService : IHostedService
    {
        protected virtual IServerTracer Tracer { get; } = new ServerMockTracer();

        protected virtual GrpcServiceSettings Settings { get; }

        protected virtual List<Interceptor> Interceptors { get; }

        protected virtual List<ChannelOption> ChannelOptions { get; }

        protected virtual IConfiguration Configuration { get; } = Singleton<IConfiguration>.Instance;

        protected virtual string ConsulAddress { get; set; }

        protected abstract IEnumerable<ServerServiceDefinition> CreateDefinitions();

        public async Task StartAsync(CancellationToken cancellationToken) => await Task.Run(() =>
                                                                           {
                                                                               GrpcServiceSettings serviceSettings;
                                                                               if (ConsulAddress.IsNull())
                                                                               {
                                                                                   var settings = Configuration.Bind<ConsulSettings>();
                                                                                   ConsulAddress = settings.ConsulUrl;
                                                                                   settings.GrpcAddress = settings.GrpcAddress.Replace("*", IPUtility.GetLocalIntranetIP().ToString());
                                                                                   var ip = IPEndPoint.Parse(settings.GrpcAddress);
                                                                                   serviceSettings = new GrpcServiceSettings
                                                                                   {
                                                                                       ServiceName = "Grpc:" + settings.ServiceName,
                                                                                       Host = ip.Address.ToString(),
                                                                                       Port = ip.Port
                                                                                   };
                                                                               }
                                                                               else
                                                                               {
                                                                                   serviceSettings = Configuration.Bind<GrpcServiceSettings>();
                                                                               }

                                                                               GrpcServiceManager.Start(CreateDefinitions(), serviceSettings, ConsulAddress, Tracer, Interceptors, ChannelOptions, OnException);
                                                                           }, cancellationToken);

        public async Task StopAsync(CancellationToken cancellationToken) => await Task.Run(() => { GrpcServiceManager.Stop(OnException); }, cancellationToken);

        protected virtual void OnException(Exception exception) => LogUtility.Error(exception, nameof(GrpcHostService));
    }
}
