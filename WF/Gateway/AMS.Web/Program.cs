using CPC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(null).Build().Run();
            return;
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                //LogUtil.Write(ex.ToString());
            };
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                e.Exception.Flatten().Handle(c =>
                {
                    // LogUtil.Write(e.ToString());
                    return true;
                });
            };
            if (!Environment.UserInteractive)
            {
                //HWMSAgentService.ServiceMain();
                return;
            }
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("please operate as indicated...");
                Console.WriteLine("service name:" + ConfigurationManager.AppSettings["ServiceName"]);
                Console.WriteLine("-[s]: press s to config the windows service");
                Console.WriteLine("-[r]: press r to run on the console");
                while (true)
                {
                    var cmd = Console.ReadLine();
                    Console.WriteLine();
                    if (InvokeCommand(cmd))
                    {
                        break;
                    }
                }
            }
            else
            {
                var cmd = args[0];
                if (!string.IsNullOrWhiteSpace(cmd))
                {
                    cmd = cmd.TrimStart('-');
                }
                InvokeCommand(cmd);
            }
            Console.ReadKey();

        }

        static bool InvokeCommand(string cmd)
        {
            var result = true;
            switch (cmd)
            {
                case "s":
                    {
                        break;
                    }
                case "r":
                    {
                        CreateHostBuilder(null).Build().Run();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("ÎÞÐ§ÊäÈë£¡");
                        result = false;
                        break;
                    }
            }
            return result;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("host.json")
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddEnvironmentVariables();
                })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseConfiguration(configuration);
                     webBuilder.UseStartup<Startup>();
                 }).UseEngine();
        }
    }
}
