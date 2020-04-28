using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WFWebProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WFWebProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostEnvironment environment)
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
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册mvc服务
            services.AddMvc();
            //数据库连接对象
            //var connectionstring = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            //Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(connectionstring);
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
        }
        //此方法由运行时调用。使用此方法配置HTTP请求管道。
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc((RouteBuilder) =>
            //{
            //    RouteBuilder.MapRoute("SinDynasty", "{Controller}/{Action}/{Parameter}", new { @Controller = "Home", @Action = "Index", @Parameter = string.Empty });
            //});
        }

        //private void InitializeDatabase()
        //{
        //    var contextbuilder= new DbContextOptionsBuilder<DataContext>();
        //    contextbuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(contextbuilder.Options.ContextType.ToString()));
        //    using (var db = new DataContext(contextbuilder.Options))
        //    {
        //        db.Database.Migrate();
        //    }
        //}
    }
}
