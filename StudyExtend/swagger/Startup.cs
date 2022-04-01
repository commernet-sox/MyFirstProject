using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPC.WebCore;
using AspectCore.DependencyInjection;
using CPC.DependencyInjection;
using CPC;
using CPC.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using HealthChecks.UI.Core;
using HealthChecks.UI;
using CPC.Redis;
using CPC.DBCore;
using CPC.Extensions;
using RabbitMQ.Client;
using CPC.EventBus;

namespace swagger
{
    public class Startup : EngineStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //    //注册Swagger生成器，定义一个和多个Swagger 文档
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
        //        // 为 Swagger JSON and UI设置xml文档注释路径
        //        var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
        //        var xmlPath = Path.Combine(basePath, "swagger.xml");
        //        c.IncludeXmlComments(xmlPath);
        //    });
        //    services.AddControllers();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public override void ConfigureServices(IServiceCollection services)
        {
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "swagger.xml");
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHealthChecks();
            services.AddHealthChecksUI(setup =>
            {
                // Set the maximum history entries by endpoint that will be served by the UI api middleware
                setup.MaximumHistoryEntriesPerEndpoint(500);
            }).AddSqlServerStorage("server=47.98.229.13;database=Test;uid=sa;password=123qwe!@#");
            services.AddSqlServer<TestApiContext>("server=47.98.229.13;database=Test;uid=sa;password=123qwe!@#");
            services.AddControllers();
        }

        public override void ConfigureContainer(IServiceContext services)
        {
            services.AddNLogger();
            services.AddElasticSearch("http://172.26.0.90:9200/").AddLogger("",new ELoggingConfiguration() { Name="*",Index="test"});
            //services.AddLiteLogger("testLite.db").AddLiteHost();
            //services.AddRabbitEventBus(new ConnectionFactory//创建连接工厂对象
            //{
            //    HostName = "127.0.0.1",//IP地址
            //    Port = 5672,//端口号
            //    UserName = "guest",//用户账号
            //    Password = "guest"//用户密码
            //},"testConsumer");
            
            //数据库健康检查\redis健康检查
            services.AddRedisClient("47.98.229.13:6379,defaultDatabase=10");
            services.AddHealth().AddElasticSearch().AddDbContext<TestApiContext>().AddRedisClient().AddDiskStorage(_=>_.AddDrive(@"D:\")).AddAllocatedMemory(1000000).AddPrivateMemory(10000000000000).AddVirtualMemory(100000000000000).AddProcess("dwm.exe", (p) => p.IsNull()).AddMaximumValue("WF", 10, () => RandomUtility.Next(20));
            //if (Configuration != null)
            //{
            //    services.RemoveAll<IConfiguration>();
            //    services.AddInstance(Configuration);
            //}

            //EngineSettings settings = Configuration.Bind<EngineSettings>();
            //EngineContext.Initialize(services, settings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/check", new HealthCheckOptions
                {
                    ResponseWriter = (ctx, report) =>
                    {
                        return ctx.Response.WriteAsync(report.ToJsonEx());
                    }
                });
                endpoints.MapHealthChecksUI();
            });
        }
    }
}
