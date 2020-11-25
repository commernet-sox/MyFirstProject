using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Bootstrap;
using CPC;
using AMS.WebCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using CPC.Service;
using AspectCore.DependencyInjection;
using System;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;

namespace AMS.Web
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration)
        {
            base.Set(configuration);

            AppSettingsUtil.GatewayUrl = configuration.GetSection("ApiBasePath").Value;
            AppSettingsUtil.AppKey = configuration.GetSection("AppKey").Value;
            AppSettingsUtil.AppSecret = configuration.GetSection("AppSecret").Value;
            AppSettingsUtil.AuthUrl = "os/1.0/OAuth2";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            // services.AddHttpReportsMiddleware(WebType.MVC, DBType.SqlServer);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = "/home/login";
                option.LogoutPath = "/home/login";
                option.AccessDeniedPath = "/home/login?type=logout";
                option.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = ValidatePrincipal
                };
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }
            )
            .AddDefaultReportingControllers()
            .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
        }

        public override void ConfigureContainer(IServiceContext builder)
        {
            PlugInServices.Register(builder);

            base.ConfigureContainer(builder);

            //GlobalToken.Initialize("", "", "", "0 0 0/1 * * ? ");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseHttpReportsMiddleware();

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseCors("stdio");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseDevExpressControls();
        }

        public async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var principal = context.Principal;
            var checkLogin = principal.Claims.FirstOrDefault(item => item.Type.ToLower() == "loginresult" &&
            item.Value.ToLower() == "success");
            if (checkLogin == null)
            {
                context.RejectPrincipal();
                await AuthenticationHttpContextExtensions.SignOutAsync(context.HttpContext,
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

    }
}
