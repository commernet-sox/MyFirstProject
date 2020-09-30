using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Setter;
using Ocelot.DependencyInjection;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using System;
using System.Diagnostics;
using System.Linq;

namespace Gateway.OcelotExtensions
{
    public static class OcelotExtensions
    {
        #region Public Methods
        public static IApplicationBuilder UseOcelotEx(this IApplicationBuilder app, Action<OcelotPipelineConfigurationEx> pipelineConfiguration = null)
        {
            var config = new OcelotPipelineConfigurationEx();
            pipelineConfiguration?.Invoke(config);
            return app.UseOcelotEx(config);
        }

        public static IApplicationBuilder UseOcelotEx(this IApplicationBuilder app, OcelotPipelineConfigurationEx pipelineConfiguration)
        {
            CreateConfiguration(app);

            ConfigureDiagnosticListener(app);

            return CreateOcelotPipeline(app, pipelineConfiguration);
        }

        public static IApplicationBuilder UseOcelotEx(this IApplicationBuilder app, Action<IApplicationBuilder, OcelotPipelineConfigurationEx> builderAction) => UseOcelotEx(app, builderAction, new OcelotPipelineConfigurationEx());

        public static IApplicationBuilder UseOcelotEx(this IApplicationBuilder app, Action<IApplicationBuilder, OcelotPipelineConfigurationEx> builderAction, OcelotPipelineConfigurationEx configuration)
        {
            CreateConfiguration(app);  // initConfiguration

            ConfigureDiagnosticListener(app);

            builderAction?.Invoke(app, configuration ?? new OcelotPipelineConfigurationEx());

            app.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

            return app;
        }
        #endregion

        #region Private Methods
        private static IInternalConfiguration CreateConfiguration(IApplicationBuilder builder)
        {
            // make configuration from file system?
            // earlier user needed to add ocelot files in startup configuration stuff, asp.net will map it to this
            var fileConfig = builder.ApplicationServices.GetService<IOptionsMonitor<FileConfiguration>>();

            // now create the config
            var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            var internalConfig = internalConfigCreator.Create(fileConfig.CurrentValue).Result;

            //Configuration error, throw error message
            if (internalConfig.IsError)
            {
                ThrowToStopOcelotStarting(internalConfig);
            }

            // now save it in memory
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
            internalConfigRepo.AddOrReplace(internalConfig.Data);

            fileConfig.OnChange(async (config) =>
            {
                var newInternalConfig = await internalConfigCreator.Create(config);
                internalConfigRepo.AddOrReplace(newInternalConfig.Data);
            });

            var adminPath = builder.ApplicationServices.GetService<IAdministrationPath>();

            var configurations = builder.ApplicationServices.GetServices<OcelotMiddlewareConfigurationDelegate>();

            // Todo - this has just been added for consul so far...will there be an ordering problem in the future? Should refactor all config into this pattern?
            foreach (var configuration in configurations)
            {
                configuration(builder).Wait();
            }

            if (adminPath != null)
            {
                //We have to make sure the file config is set for the ocelot.env.json and ocelot.json so that if we pull it from the
                //admin api it works...boy this is getting a spit spags boll.
                var fileConfigSetter = builder.ApplicationServices.GetService<IFileConfigurationSetter>();

                SetFileConfig(fileConfigSetter, fileConfig);
            }

            return GetOcelotConfigAndReturn(internalConfigRepo);
        }

        private static void SetFileConfig(IFileConfigurationSetter fileConfigSetter, IOptionsMonitor<FileConfiguration> fileConfig)
        {
            var response = fileConfigSetter.Set(fileConfig.CurrentValue).Result;

            if (IsError(response))
            {
                ThrowToStopOcelotStarting(response);
            }
        }

        private static bool IsError(Response response) => response == null || response.IsError;

        private static void ThrowToStopOcelotStarting(Response config) => throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");

        private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
        {
            var ocelotConfiguration = provider.Get();

            if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
            {
                ThrowToStopOcelotStarting(ocelotConfiguration);
            }

            return ocelotConfiguration.Data;
        }

        private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
        {
            var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
            var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(listener);
        }

        private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder app, OcelotPipelineConfigurationEx pipelineConfiguration)
        {
            app.BuildOcelotPipeline(pipelineConfiguration);

            /*
            inject first delegate into first piece of asp.net middleware..maybe not like this
            then because we are updating the http context in ocelot it comes out correct for
            rest of asp.net..
            */

            app.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

            return app;
        }
        #endregion
    }
}
