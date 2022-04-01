using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudyExtend.Tasks
{
    public class Tasks
    {
        /// <summary>
        /// task不获取结果不会阻塞主线程
        /// </summary>
        public static void test()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            //注册任务取消的事件
            source.Token.Register(() =>
            {
                Console.WriteLine("任务取消后执行的事件...");
            });
            //source.Cancel();
            //1.new方式实例化一个Task，需要通过Start方法启动
            Task task = new Task(() =>
            {
                if (!source.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                    Console.WriteLine($"hello, task1的线程ID为{Thread.CurrentThread.ManagedThreadId}");
                }
                
            });
            task.Start();
            //task.RunSynchronously();
            //2.Task.Factory.StartNew(Action action)创建和启动一个Task
            Task task2 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"hello, task2的线程ID为{ Thread.CurrentThread.ManagedThreadId}");
            });

            //3.Task.Run(Action action)将任务放在线程池队列，返回并启动一个Task
            Task task3 = Task.Run(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"hello, task3的线程ID为{ Thread.CurrentThread.ManagedThreadId}");
            });
            
            //阻塞主线程
            //Task.WaitAll(new Task[] { task2,task3});
            //Task.WaitAny(new Task[] { task2,task3});

            //延续操作
            Task.WhenAll(task2, task3).ContinueWith((t) =>
            {
                Thread.Sleep(100);
                Console.WriteLine("执行后续操作...");
            });
            Console.WriteLine("执行主线程！");
        }
        public static void Test2()
        {
            ////1.new方式实例化一个Task，需要通过Start方法启动
            Task<string> task = new Task<string>(() =>
            {
                return $"hello, task1的ID为{Thread.CurrentThread.ManagedThreadId}";
            });
            task.Start();

            ////2.Task.Factory.StartNew(Func func)创建和启动一个Task
            Task<string> task2 = Task.Factory.StartNew<string>(() =>
            {
                return $"hello, task2的ID为{ Thread.CurrentThread.ManagedThreadId}";
            });

            ////3.Task.Run(Func func)将任务放在线程池队列，返回并启动一个Task
            Task<string> task3 = Task.Run<string>(() =>
            {
                return $"hello, task3的ID为{ Thread.CurrentThread.ManagedThreadId}";
            });

            Console.WriteLine("执行主线程！");
            Console.WriteLine(task.Result);
            Console.WriteLine(task2.Result);
            Console.WriteLine(task3.Result);
        }
    }
}
