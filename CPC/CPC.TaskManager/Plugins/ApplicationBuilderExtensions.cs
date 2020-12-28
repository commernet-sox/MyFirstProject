using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace CPC.TaskManager.Plugins
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceContext AddViewPlugin(this IServiceContext services, int port = 18081, Action<ViewOptions> setup = null) => services.AddTaskPlugin(new ViewPlugin { Url = $"http://*:{port}", Setup = setup });

        internal static void UseView(this IApplicationBuilder app, ViewOptions options, Action<Services> configure = null)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            app.UseFileServer(options);

            var services = Services.Create(options);
            configure?.Invoke(services);

            app.Use(async (context, next) =>
            {
                context.Items[typeof(Services)] = services;
                await next.Invoke();
            });

            app.UseDeveloperExceptionPage();

            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        var ex = context.Features.Get<IExceptionHandlerFeature>().Error;
            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "text/html";
            //        await context.Response.WriteAsync(services.ViewEngine.ErrorPage(ex));
            //    });
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: nameof(CPC.TaskManager.Plugins),
                    template: "{controller=Scheduler}/{action=Index}");
            });
        }

        private static void UseFileServer(this IApplicationBuilder app, ViewOptions options)
        {
            IFileProvider fs;
            if (string.IsNullOrEmpty(options.ContentRootDirectory))
            {
                fs = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "Plugins/Content");
            }
            else
            {
                fs = new PhysicalFileProvider(options.ContentRootDirectory);
            }

            var fsOptions = new FileServerOptions()
            {
                RequestPath = new PathString("/Content"),
                EnableDefaultFiles = false,
                EnableDirectoryBrowsing = false,
                FileProvider = fs
            };

            app.UseFileServer(fsOptions);
        }

        internal static void AddView(this IServiceCollection services) => services.AddMvcCore(t => t.EnableEndpointRouting = false)
                .AddApplicationPart(Assembly.GetExecutingAssembly()).AddJsonFormatters();

    }
}