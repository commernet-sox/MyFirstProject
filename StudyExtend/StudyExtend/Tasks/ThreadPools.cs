using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StudyExtend.Tasks
{
    public class ThreadPools
    {
        /// <summary>
        /// 我们不能控制线程池中线程的执行顺序，也不能获取线程池内线程取消/异常/完成的通知
        /// </summary>
        public static void test()
        {
            for (int i = 1; i <= 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    Console.WriteLine($"第{obj}个任务执行。。。");
                }), i);
            }
        }
            
    }
}
