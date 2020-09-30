using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SDT.BaseTool;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.Service
{
    /// <summary>
    /// 基础Startup类
    /// </summary>
    public class BaseStartup
    {
        /// <summary>
        /// 构造函数env
        /// </summary>
        /// <param name="env"></param>
        public BaseStartup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Set(builder.Build());
        }
        /// <summary>
        /// 构造函数configuration
        /// </summary>
        /// <param name="configuration"></param>
        public BaseStartup(IConfiguration configuration) => Set(configuration);

        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 配置appsettings.json对象
        /// </summary>
        /// <param name="configuration"></param>
        protected virtual void Set(IConfiguration configuration)
        {
            Configuration = configuration;
            Singleton<IConfiguration>.Instance = Configuration;
        }

        protected virtual void ConfigureContext(IServiceContext services)
        {
        }

        public virtual void ConfigureContainer(IServiceContext services)
        {
            ConfigureContext(services);
            var engineSettings = Configuration.Bind<EngineSettings>();
            EngineContext.Initialize(true, engineSettings, services);
        }
    }
}
