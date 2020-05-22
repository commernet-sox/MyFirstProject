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
            //�����������ȡ·��
            var configurationbuilder = new ConfigurationBuilder();
            configurationbuilder.SetBasePath(environment.ContentRootPath);
            configurationbuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configurationbuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json",optional:true,reloadOnChange:true);
            configurationbuilder.AddEnvironmentVariables();
            Core.Infrastructure.Global.Configuration= Configuration =configurationbuilder.Build();
            Console.WriteLine("������Startup");
        }

        public IConfigurationRoot Configuration { get; }
        //�˷���������ʱ���á�ʹ�ô˷�����������ӵ�������
        
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
            //�����Ȩ֧�֣������ʹ��Cookie�ķ�ʽ�����õ�¼ҳ���û��Ȩ��ʱ����תҳ��
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Home/Login");            //��¼·�������ǵ��û���ͼ������Դ��δ���������֤ʱ�����򽫻Ὣ�����ض���������·����
                    o.AccessDeniedPath = new PathString("/Home/Error");     //��ֹ����·�������û���ͼ������Դʱ����δͨ������Դ���κ���Ȩ���ԣ����󽫱��ض���������·����
                    o.SlidingExpiration = true; //Cookie���Է�Ϊ�����Եĺ���ʱ�Եġ� ��ʱ�Ե���ָֻ�ڵ�ǰ�������������Ч�������һ���رվ�ʧЧ���������ɾ������ �����Ե���ָCookieָ����һ������ʱ�䣬�����ʱ�䵽��֮ǰ����cookieһֱ��Ч�������һֱ��¼�Ŵ�cookie�Ĵ��ڣ��� slidingExpriation�������ǣ�ָʾ�������cookie��Ϊ������cookie�洢�����ǻ��Զ����Ĺ���ʱ�䣬��ʹ�û������ڵ�¼��һֱ�������һ��ʱ���ȴ�Զ�ע����Ҳ����˵����10���¼�ˣ������������õ�TimeOutΪ30���ӣ����slidingExpriationΪfalse,��ô10: 30�Ժ���ͱ������µ�¼�����Ϊtrue�Ļ�����10: 16��ʱ����һ����ҳ�棬�������ͻ�֪ͨ��������ѹ���ʱ���޸�Ϊ10: 46��
                });

            services.AddDistributedMemoryCache();//����session֮ǰ����������ڴ�

            services.AddSession(options =>
            {
                //�������ĵײ㻺�淽��ʹ�õľ���IDistributedCache
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session����ʱ��
                options.Cookie.HttpOnly = true;//��Ϊhttponly
            });
            //ע��mvc����
            services.AddMvc().AddJsonOptions(options =>
             {
                 //���ȫ��JSON���ظ�ʽ,����datatables������ʾ
                 options.SerializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                 IsoDateTimeConverter timeFormate = new IsoDateTimeConverter();
                 timeFormate.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                 options.SerializerSettings.Converters.Add(timeFormate);
                 options.SerializerSettings.Formatting = Formatting.Indented;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Include;//�������


             }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            

            
            //���ݿ����Ӷ���
            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
        }
        //Autofacע��
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
        //�˷���������ʱ���á�ʹ�ô˷�������HTTP����ܵ���
        
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
            app.UseAuthentication(); //�����֤�м��
            //ʹ�ûỰ
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
