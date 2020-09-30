using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using CPC.Service;
using Gateway.Middlerware;
using Gateway.OcelotExtensions;
using Gateway.OcelotExtensions.Administration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Gateway
{
    public class Startup:BaseStartup
    {
        public Startup(IConfiguration configuration):base(configuration)
        { 
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddOcelot().AddConsul().AddAdministration("/admin");
            services.AddCors(o => o.AddDefaultPolicy(b =>
            {
                b.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyMethod().AllowAnyHeader();
            }));
        }

        public override void ConfigureContainer(IServiceContext builder)
        {
            builder.AddInstance(Configuration.Bind<JwtSettings>());
            builder.AddType<JwtManager>();
            base.ConfigureContainer(builder);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            var configuration = new OcelotPipelineConfigurationEx
            {
                Extend = oapp =>
                {
                    oapp.UseMiddleware<JwtMiddleware>();
                }
            };
            app.UseOcelotEx(configuration);

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
