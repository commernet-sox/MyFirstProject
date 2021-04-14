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
            //cookie����
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //ע��Session����
            //�����ڴ�Ļ���
            services.AddDistributedMemoryCache();
            
            //Ĭ�Ϲ���ʱ��Ϊ20���ӣ�ÿ�η��ʶ�������
            services.AddSession(options=> {
                //Session�Ĺ���ʱ��(��η��ʽ��ᱻ���ã�Ĭ�Ϲ���ʱ��Ϊ20����)
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //����Ϊtrue��ʾǰ��js�Ƚű��޷���ȡcookie����ֹ��xss����(Ĭ����true)
                options.Cookie.HttpOnly = true;
                //Cookie�Ǳ����(Ĭ����false),���Ը��������cookie����
                options.Cookie.IsEssential = true;
            });
            //��������Ȩ����
            //������л�����
            services.AddControllers(c=>c.Filters.Add(new AuthorizeFilter())).AddJsonEx(s =>
            {
                s.Converters.Add(new Newtonsoft.Json.Converters.DataSetConverter());
                s.Converters.Add(new Newtonsoft.Json.Converters.DataTableConverter());
            });
            //���ݱ���
            services.AddDataProtection();
            //����ͷ��֤
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = HeadersHandler.SchemeName;
                o.DefaultAuthenticateScheme = HeadersHandler.SchemeName;
                o.AddScheme<HeadersHandler>(HeadersHandler.SchemeName, HeadersHandler.SchemeName);
            });
            //http�����ķ���
            services.AddHttpContextAccessor();
            //�ӿ��ĵ�����
            services.AddOpenService(new OpenServiceOptions
            {
                CreateForApiDesc = (d, o) =>
                {
                    o.Title = "SimpleWebApi API";
                    o.Description = "SimpleWebApi �ӿڷ���";
                },
                SwaggerGenSetup = (o) =>
                {
                    o.OperationFilter<SwaggerParameter>();
                },
                XmlComments = new[] { "SimpleWebApi.xml", "SimpleWebApi.Data.xml" },
                IgnoreApiDoc = false
            });
            //services.AddHostedService<DbHost>();
            //��������
            //services.AddNoLockDb<SimpleWebApiContext>(o => o.UseSqlServer(Configuration.GetConnectionString("TMSConnection")).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })));
        }
        /// <summary>
        /// ����ע�᷽��
        /// </summary>
        /// <param name="services"></param>
        protected override void ConfigureContext(IServiceContext services)
        {
            //ע��ִ�ģʽ
            services.AddType<IOperate, Operate>();
            services.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            services.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //�����ݿ�ģʽ
            services.AddDelegate<SimpleWebApiContext>(s =>
            {
                return s.Resolve<IHWMSDbFactory>().Create();
            }, Lifetime.Scoped);
            services.AddType<IHWMSDbFactory, HWMSDbFactory>(Lifetime.Scoped);
            //ע��Redis����
            services.AddDelegate<ISeedCache>(s => new RedisCache(s.Resolve<RedisClient>(), s.Resolve<IHttpContextAccessor>(), new[] { 1 }));

            services.AddDelegate(_ => new RedisClient(Configuration["RedisConnection"]));
            //ע��ҵ�����
            services.AddType<TestApiService>(Lifetime.Scoped);

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // �Զ����м�������طǷ�ip
            app.UseMiddleware<SafeIpMiddleware>(Configuration["IllegalIp"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //ʹ��cookie����
            app.UseCookiePolicy();
            //����Session�ܵ�
            app.UseSession();
            //id��������㷨����
            IdGen.SetDefault( 1, 5);
            
            //ʹ��·��
            app.UseRouting();
            //1. ���þ�̬��Դ
            app.UseStaticFiles();
            //�Զ����ļ���,����DownLoad�ļ��У�����������ص�����Ľ��з���(������ʱ�򣬷���Ŀ¼�б���Ҫ��������ļ��У�����)
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "DownLoad")),
                //�������·���������ǰ��������һ���ģ���ȻҲ�������ģ�ע��ǰ��Ҫ��/��
                RequestPath = "/DownLoad"
            });
            //����
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
            });
            
            //ʹ�������֤
            app.UseAuthentication();
            app.UseAuthorization();
            //��ӽӿ��ĵ��м��
            app.UseOpenService();
            //����ս��ӳ��
            app.UseEndpoints(endpoints =>
            {
                //�����ַӳ��
                endpoints.MapControllers();
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
