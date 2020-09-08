using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace MiddleWareTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CreateHostBuilder(args).Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) 
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            //顺序执行管道
            //MiddleWare A
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("A in");
            //    await next();
            //    Console.WriteLine("A out");
            //});
            ////MiddleWare B
            //app.Use(async (context, next) => {
            //    Console.WriteLine("B in");
            //    await next();
            //    Console.WriteLine("B out");
            //});
            ////MiddleWare C
            //app.Run(async context => {
            //    Console.WriteLine("C");
            //    await context.Response.WriteAsync("Hello World from the terminal middleware");
            //});

            //条件执行管道
            //MiddleWare A
            app.Use(async (context, next) =>
            {
                Console.WriteLine("A in");
                await next();
                Console.WriteLine("A out");
            });
            //MiddleWare B
            //app.Map(
            //        new PathString("/foo"),
            //        a=>a.Use(async (context, next) => 
            //        {
            //            Console.WriteLine("B in");
            //            await next();
            //            Console.WriteLine("B out");
            //        }));
            app.UseWhen(
                context => context.Request.Path.StartsWithSegments(new PathString("/foo")),
                a => a.Use(async (context, next) => {
                Console.WriteLine("B in");
                    await next();
                    Console.WriteLine("B out");
                }));
            //使用自定义中间件
            app.UseCustomMiddle();
            //app.UseMiddleware<CustomMiddleWare>();

            //MiddleWare C
            app.Run(async context => {
                Console.WriteLine("C");
                await context.Response.WriteAsync("Hello World from the terminal middleware");
            });
        }
    }

    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;
        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        //业务逻辑
        public async Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine("CustomMiddleWare in");
            if (httpContext.Request.Path.Value.ToLower() == "/foo/denied")
            {
                Console.WriteLine("denied");
            }
            await _next.Invoke(httpContext);
            Console.WriteLine("CustomMiddleWare out");
        }
    }
    public static class CustomMiddleWareExtension
    {
        public static IApplicationBuilder UseCustomMiddle(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<CustomMiddleWare>();
        }
    }
}
