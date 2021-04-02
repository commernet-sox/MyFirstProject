using CPC.TaskManager;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuartzPro.Jobs
{
    public class SyncTestApiJob : BaseJob
    {
        public override async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("定时任务开始执行...");
            await DoJob();

        }
        public async Task DoJob()
        {
            Console.WriteLine("定时任务正在执行...");
        }
    }
}
