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
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ע��mvc����
            services.AddMvc();
            //���ݿ����Ӷ���
            //var connectionstring = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            //Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(connectionstring);
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
        }
        //�˷���������ʱ���á�ʹ�ô˷�������HTTP����ܵ���
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
