﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SDT.BaseTool;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.Service
{
    public class ConsulHostService : IHostedService
    {
        private ConsulRegister _consulRegister;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consulSettings = Singleton<IConfiguration>.Instance.Bind<ConsulSettings>();
            if (!consulSettings.ConsulUrl.IsNull())
            {
                if (consulSettings.ServiceAddress.IsNull())
                {
                    var urls = Singleton<IConfiguration>.Instance["Urls"].ToString();
                    if (!urls.IsNull())
                    {
                        var port = urls.Substring(urls.LastIndexOf(":") + 1).ConvertInt32();
                        if (port > 0)
                        {
                            consulSettings.ServiceAddress = $"{IPUtility.GetLocalIntranetIP()}:{port}";
                        }
                    }
                }

                _consulRegister = new ConsulRegister(consulSettings);
                _consulRegister.Register();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consulRegister.Dispose();
            return Task.CompletedTask;
        }
    }
}
