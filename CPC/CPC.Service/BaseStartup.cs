using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CPC.Service
{
    public class BaseStartup
    {
        public BaseStartup()
        { }

        public BaseStartup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
               .AddEnvironmentVariables().AddApollo();

            Set(builder.Build());
        }

        public BaseStartup(IConfiguration configuration) => Set(configuration);

        public IConfiguration Configuration { get; private set; }

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
            if (Configuration != null)
            {
                services.RemoveAll<IConfiguration>();
                services.AddInstance(Configuration);
            }
            var engineSettings = Configuration.Bind<EngineSettings>();
            EngineContext.Initialize(true, engineSettings, services);
        }
    }
}
