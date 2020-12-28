using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace CPC.Service
{
    public static class EngineExtensions
    {
        public static IHostBuilder UseEngine(this IHostBuilder host) => host.UseServiceProviderFactory(new ServiceContextProviderFactory());

        public static IMvcBuilder AddJsonEx(this IMvcBuilder builder, Action<JsonSerializerSettings> setupAction = null) => builder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            options.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            options.SerializerSettings.Formatting = Formatting.None;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            setupAction?.Invoke(options.SerializerSettings);
        });

        public static void UseEngine(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(() =>
            {
                EngineContext.Dispose();
            });
        }
    }
}
