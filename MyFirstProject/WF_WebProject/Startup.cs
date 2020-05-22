using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WFWebProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WFWebProject
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            //设置配置项读取路径
            var configurationbuilder = new ConfigurationBuilder();
            configurationbuilder.SetBasePath(environment.ContentRootPath);
            configurationbuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configurationbuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json",optional:true,reloadOnChange:true);
            configurationbuilder.AddEnvironmentVariables();
            Core.Infrastructure.Global.Configuration= Configuration =configurationbuilder.Build();
            Console.WriteLine("启动了Startup");
        }

        public IConfigurationRoot Configuration { get; }
        //此方法由运行时调用。使用此方法将服务添加到容器。
        
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            // .AddCookie(options =>
            // {
            //     options.LoginPath = new PathString("/login");
            //     options.AccessDeniedPath = new PathString("/denied");
            //     options.Cookie.Name = "WF";

            // }
            // );
            //添加授权支持，并添加使用Cookie的方式，配置登录页面和没有权限时的跳转页面
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Home/Login");            //登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径。
                    o.AccessDeniedPath = new PathString("/Home/Error");     //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径。
                    o.SlidingExpiration = true; //Cookie可以分为永久性的和临时性的。 临时性的是指只在当前浏览器进程里有效，浏览器一旦关闭就失效（被浏览器删除）。 永久性的是指Cookie指定了一个过期时间，在这个时间到达之前，此cookie一直有效（浏览器一直记录着此cookie的存在）。 slidingExpriation的作用是，指示浏览器把cookie作为永久性cookie存储，但是会自动更改过期时间，以使用户不会在登录后并一直活动，但是一段时间后却自动注销。也就是说，你10点登录了，服务器端设置的TimeOut为30分钟，如果slidingExpriation为false,那么10: 30以后，你就必须重新登录。如果为true的话，你10: 16分时打开了一个新页面，服务器就会通知浏览器，把过期时间修改为10: 46。
                });

            services.AddDistributedMemoryCache();//启用session之前必须先添加内存

            services.AddSession(options =>
            {
                //本方法的底层缓存方法使用的就是IDistributedCache
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session活期时间
                options.Cookie.HttpOnly = true;//设为httponly
            });
            //注册mvc服务
            services.AddMvc().AddJsonOptions(options =>
             {
                 //设计全局JSON返回格式,用于datatables数据显示
                 options.SerializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                 IsoDateTimeConverter timeFormate = new IsoDateTimeConverter();
                 timeFormate.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                 options.SerializerSettings.Converters.Add(timeFormate);
                 options.SerializerSettings.Formatting = Formatting.Indented;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Include;//必须包含


             }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            

            
            //数据库连接对象
            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
        }
        //Autofac注册
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterModule(new ServiceModules());

            this.RegisterAutomapper(builder);

        }
        private void RegisterAutomapper(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(configuration =>
            {
                foreach (var profile in context.Resolve<IEnumerable<AutoMapper.Profile>>())
                {
                    configuration.AddProfile(profile);
                }
            }))
           .AsSelf()
           .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                .CreateMapper(context.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
        //此方法由运行时调用。使用此方法配置HTTP请求管道。
        
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
            app.UseAuthentication(); //身份验证中间件
            //使用会话
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }

        
    }
}
