using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using SDT.BaseTool;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace SDT.Service
{
    public static class OpenServiceExtensions
    {
        /// <summary>
        /// 添加swagger服务说明
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        public static void AddOpenService(this IServiceCollection services, OpenServiceOptions configure = null)
        {
            configure ??= new OpenServiceOptions();

            services.AddRouting(o => o.LowercaseUrls = true);

            if (!configure.IgnoreApiVersion)
            {
                services.AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.DefaultApiVersion = configure.DefaultVersion;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    configure.ApiVersionSetup?.Invoke(o);
                });
            }

            if (configure.EnableConsul)
            {
                services.AddHostedService<ConsulHostService>();
            }

            if (!configure.IgnoreApiDoc)
            {
                services.AddVersionedApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VV";
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    configure.ApiExplorerSetup?.Invoke(o);
                });

                SwaggerConfigureOptions.CreateForApiDesc = configure.CreateForApiDesc;
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
                services.AddSwaggerGen(
                    options =>
                    {
                        options.OperationFilter<SwaggerOperationFilter>();
                        configure.SwaggerGenSetup?.Invoke(options);

                        if (!configure.XmlComments.IsNull())
                        {
                            var baseDir = PlatformServices.Default.Application.ApplicationBasePath;
                            foreach (var xml in configure.XmlComments)
                            {
                                var xmlPath = Path.Combine(baseDir, xml);
                                options.IncludeXmlComments(xmlPath, true);
                            }
                        }
                    });

                if (configure.SwaggerNewtonsoftSupport)
                {
                    services.AddSwaggerGenNewtonsoftSupport();
                }
            }
        }

        /// <summary>
        /// 使用swagger中间件服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="appOptionsSetup"></param>
        public static void UseOpenService(this IApplicationBuilder app, Action<OpenServiceAppOptions> appOptionsSetup = null)
        {
            app.UseRouting();
            var appOptions = new OpenServiceAppOptions();
            appOptionsSetup?.Invoke(appOptions);

            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(() =>
            {
                EngineContext.Dispose();
            });

            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

            if (provider != null)
            {
                app.UseSwagger(appOptions.SwaggerSetup);

                app.UseSwaggerUI(c =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                    c.DocExpansion(DocExpansion.None);
                    c.DefaultModelsExpandDepth(-1);
                    c.RoutePrefix = "doc";

                    appOptions.SwaggerUISetup?.Invoke(c);
                });
            }
        }
    }
}
