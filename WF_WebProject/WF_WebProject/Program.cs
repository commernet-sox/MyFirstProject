using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace WFWebProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //应用程序的启动
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel();
                    //webBuilder.ConfigureServices(services=>services.AddAutofac());
                    //webBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());
                    //webBuilder.UseIISIntegration().UseUrls("http://*:5001/");
                    //webBuilder.UseNLog();
                    //webBuilder.UseStartup("WFWebProject")/*.CaptureStartupErrors(true)*/;
                    webBuilder.UseStartup<Startup>();
                });
    }
}
