﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject
{
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.WriteLine("启动了StartupDevelopment");
        }
        public IConfiguration Configuration { get; }
        //此方法由运行时调用。使用此方法将服务添加到容器。
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册mvc服务
            services.AddMvc();
            
        }
        //此方法由运行时调用。使用此方法配置HTTP请求管道。
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc((RouteBuilder) =>
            //{
            //    RouteBuilder.MapRoute("SinDynasty", "{Controller}/{Action}/{Parameter}", new { @Controller = "Home", @Action = "Index", @Parameter = string.Empty });
            //});
        }
    }
}
