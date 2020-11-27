using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using TestApi.Application.Core;
using TestApi.Application.Services;
using TestInfrastructure;

namespace TestApi
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
            services.AddControllers().AddJsonEx();

            services.AddOpenService(new OpenServiceOptions
            {
                CreateForApiDesc = (d, o) =>
                {
                    o.Title = "Test API";
                    o.Description = "Test 接口服务";
                },
                XmlComments = new[] { "TestApi.xml", "TestData.xml" },
                IgnoreApiDoc = false
            });
            services.AddHostedService<DbHost>();
            services.AddHttpContextAccessor();
            services.AddNoLockDb<TestContext>(o => o.UseSqlServer(Configuration.GetConnectionString("TMSConnection")).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })));
        }

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
            IdGen.SetDefault(1, 1, 5);
            app.UseRouting();

            app.UseOpenService();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
