using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using CPC.DBCore;
using CPC.Redis;
using CPC.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityService
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
                    o.Title = "Identity Authentication API";
                    o.Description = "统一身份认证API";
                },
<<<<<<< HEAD
                //XmlComments = new[] { "IdentityService.xml", "Data.IdentityService.xml" }
=======
                //XmlComments = new[] { "AMS.IdentityService.xml", "AMS.Data.IdentityService.xml" }
>>>>>>> b3605b5bc406de91a3ad5846c938891e052aea1c
            });

            //services.AddNoLockDb<AMSContext>(o => o.UseMySql(Configuration.GetConnectionString("AMSConnection")).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })));
        }

        public override void ConfigureContainer(IServiceContext services)
        {
            services.AddInstance(Configuration.Bind<JwtSettings>());
            services.AddDelegate(_ => new RedisClient(Configuration["RedisConnection"]));
            services.AddType<JwtManager>(Lifetime.Singleton);
            services.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            services.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //services.AddType<SysUserService>(Lifetime.Scoped);

            base.ConfigureContainer(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenService();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
