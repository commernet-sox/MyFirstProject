using CPC;
using System;
using Topshelf;

namespace QuartzPro
{
    class Program
    {
        public static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.UseNLog();

                x.Service<ServiceStartup>(s =>
                {
                    s.ConstructUsing(name => new ServiceStartup(args));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());

                });
                x.RunAsLocalSystem();
                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(1);
                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                });
                x.SetStartTimeout(TimeSpan.FromMinutes(5));
                x.SetStopTimeout(TimeSpan.FromMinutes(5));
                x.OnException((exception) =>
                {
                    LogUtility.Error("异常 - " + exception.Message);
                });
                x.SetDescription("定时任务 -" + "");
                x.StartAutomaticallyDelayed();
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

            Console.ReadLine();
        }
    }
}
