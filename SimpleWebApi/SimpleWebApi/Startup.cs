using AspectCore.DependencyInjection;
using CPC;
using CPC.DBCore;
using CPC.Redis;
using CPC.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SimpleWebApi.Application.Cache;
using SimpleWebApi.Application.Core;
using SimpleWebApi.Application.Db;
using SimpleWebApi.Application.Service;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Middleware;
using System;
using System.Collections.Generic;
using System.IO;
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
            //cookie策略
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //注册Session服务
            //基于内存的缓存
            services.AddDistributedMemoryCache();
            
            //默认过期时间为20分钟，每次访问都会重置
            services.AddSession(options=> {
                //Session的过期时间(多次访问将会被重置，默认过期时间为20分钟)
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //设置为true表示前端js等脚本无法读取cookie，防止了xss攻击(默认是true)
                options.Cookie.HttpOnly = true;
                //Cookie是必须的(默认是false),可以覆盖上面的cookie策略
                options.Cookie.IsEssential = true;
            });
            //控制器授权过滤
            //添加序列化规则
            services.AddControllers(c=>c.Filters.Add(new AuthorizeFilter())).AddJsonEx(s =>
            {
                s.Converters.Add(new Newtonsoft.Json.Converters.DataSetConverter());
                s.Converters.Add(new Newtonsoft.Json.Converters.DataTableConverter());
            });
            //数据保护
            services.AddDataProtection();
            //请求头验证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = HeadersHandler.SchemeName;
                o.DefaultAuthenticateScheme = HeadersHandler.SchemeName;
                o.AddScheme<HeadersHandler>(HeadersHandler.SchemeName, HeadersHandler.SchemeName);
            });
            //http上下文访问
            services.AddHttpContextAccessor();
            //接口文档配置
            services.AddOpenService(new OpenServiceOptions
            {
                CreateForApiDesc = (d, o) =>
                {
                    o.Title = "SimpleWebApi API";
                    o.Description = "SimpleWebApi 接口服务";
                },
                SwaggerGenSetup = (o) =>
                {
                    o.OperationFilter<SwaggerParameter>();
                },
                XmlComments = new[] { "SimpleWebApi.xml", "SimpleWebApi.Data.xml" },
                IgnoreApiDoc = false
            });
            //services.AddHostedService<DbHost>();
            //数据连接
            //services.AddNoLockDb<SimpleWebApiContext>(o => o.UseSqlServer(Configuration.GetConnectionString("TMSConnection")).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })));
        }
        /// <summary>
        /// 服务注册方法
        /// </summary>
        /// <param name="services"></param>
        protected override void ConfigureContext(IServiceContext services)
        {
            //注入仓储模式
            services.AddType<IOperate, Operate>();
            services.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            services.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //多数据库模式
            services.AddDelegate<SimpleWebApiContext>(s =>
            {
                return s.Resolve<IHWMSDbFactory>().Create();
            }, Lifetime.Scoped);
            services.AddType<IHWMSDbFactory, HWMSDbFactory>(Lifetime.Scoped);
            //注入Redis缓存
            services.AddDelegate<ISeedCache>(s => new RedisCache(s.Resolve<RedisClient>(), s.Resolve<IHttpContextAccessor>(), new[] { 1 }));

            services.AddDelegate(_ => new RedisClient(Configuration["RedisConnection"]));
            //注入业务代码
            services.AddType<TestApiService>(Lifetime.Scoped);

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 自定义中间件，拦截非法ip
            app.UseMiddleware<SafeIpMiddleware>(Configuration["IllegalIp"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用cookie策略
            app.UseCookiePolicy();
            //启用Session管道
            app.UseSession();
            //id随机生成算法设置
            IdGen.SetDefault( 1, 5);
            
            //使用路由
            app.UseRouting();
            //1. 启用静态资源
            app.UseStaticFiles();
            //自定义文件夹,开启DownLoad文件夹，便于下载相关的请求的进行访问(发布的时候，发布目录中必须要有下面的文件夹！！！)
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "DownLoad")),
                //配置相对路径（建议和前面的名起个一样的，当然也可以起别的，注意前面要有/）
                RequestPath = "/DownLoad"
            });
            //跨域
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
            });
            
            //使用身份验证
            app.UseAuthentication();
            app.UseAuthorization();
            //添加接口文档中间件
            app.UseOpenService();
            //添加终结点映射
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
