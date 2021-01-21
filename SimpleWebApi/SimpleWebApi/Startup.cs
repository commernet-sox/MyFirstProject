using AspectCore.DependencyInjection;
using CPC;
using CPC.DBCore;
using CPC.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SimpleWebApi.Application.Core;
using SimpleWebApi.Application.Service;
using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi
{
    public class Startup : BaseStartup
    {
        public Startup(IWebHostEnvironment env) : base(env)
        {
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //底层基础操作和接口文档
            services.AddControllers().AddJsonEx();

            services.AddOpenService(new OpenServiceOptions
            {
                CreateForApiDesc = (d, o) =>
                {
                    o.Title = "SimpleWebApi API";
                    o.Description = "SimpleWebApi 接口服务";
                },
                XmlComments = new[] { "SimpleWebApi.xml", "SimpleWebApi.Data.xml" },
                IgnoreApiDoc = false
            });
            services.AddHostedService<DbHost>();
            services.AddHttpContextAccessor();
            //数据连接
            services.AddNoLockDb<SimpleWebApiContext>(o => o.UseSqlServer(Configuration.GetConnectionString("TMSConnection")).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })));
        }
        /// <summary>
        /// 服务注册方法
        /// </summary>
        /// <param name="services"></param>
        protected override void ConfigureContext(IServiceContext services)
        {
            services.AddType<IOperate, Operate>();
            services.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            services.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            services.AddType<TestApiService>(Lifetime.Scoped);
            //services.AddType<EntrustOrderService>(Lifetime.Scoped);
            //services.AddType<AssembleService>(Lifetime.Scoped);
            //services.AddType<BillService>(Lifetime.Scoped);
            //services.AddType<EntrustTrackService>(Lifetime.Scoped);
            //services.AddType<RegionService>(Lifetime.Scoped);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //id随机生成算法设置
            IdGen.SetDefault(1, 1, 5);
            app.UseRouting();
            //添加中间件
            app.UseOpenService();

            app.UseEndpoints(endpoints =>
            {
                //请求地址映射
                endpoints.MapControllers();
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
