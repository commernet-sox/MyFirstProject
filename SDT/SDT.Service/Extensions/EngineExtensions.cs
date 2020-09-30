using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using SDT.BaseTool;

namespace SDT.Service
{
    public static class EngineExtensions
    {
        public static IHostBuilder UseEngine(this IHostBuilder host) => host.UseServiceProviderFactory(new ServiceContextProviderFactory());

        /// <summary>
        /// 获取配置的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Bind<T>(this IConfiguration configuration, string key = "") where T : class, new()
        {
            if (key.IsNull())
            {
                key = typeof(T).Name;
            }

            var data = new T();
            configuration.Bind(key, data);
            return data;
        }

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
    }
}
