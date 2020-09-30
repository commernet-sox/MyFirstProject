using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.OcelotExtensions.Swagger
{
    public static class OcelotSwaggerExtensions
    {
        public static IServiceCollection AddOcelotSwagger(this IServiceCollection services, Action<OcelotSwaggerOptions> configureOptions)
        {
            services.Configure(configureOptions);
            return services.AddOcelotSwagger();
        }

        public static IServiceCollection AddOcelotSwagger(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSwaggerGen();
            return services;
        }

        public static IApplicationBuilder UseOcelotSwagger(this IApplicationBuilder app)
        {
            var optionsAccessor = app.ApplicationServices.GetRequiredService<IOptionsMonitor<OcelotSwaggerOptions>>();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    optionsAccessor.CurrentValue.SwaggerEndPoints.ForEach(
                        i => options.SwaggerEndpoint(i.Url, i.Name));
                });

            app.UseMiddleware<OcelotSwaggerMiddleware>();
            return app;
        }
    }
}
