using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpBasic.Basic
{
    public class Threads
    {
        public void CostTime()
        {
            Console.WriteLine($"当前时间{DateTime.Now}");
        }
        public void ParamCostTime(string type,string author)
        {
            Console.WriteLine($"{type}时间{DateTime.Now}-{author}");
        }
        //独立创建线程
        public void CreateSingleThread()
        {
            //Thread thread = new Thread(CostTime);
            //thread.Start();

            //Thread thread = new Thread(()=>ParamCostTime("北京","wangfeng"));//lambda表达式
            Thread thread = new Thread(delegate () { ParamCostTime("北京", "wangfeng"); });//匿名委托
            thread.Start();
        }
        //创建线程池
        public void CreateThreadPools()
        {
            //ThreadPool.QueueUserWorkItem((state) =>  CostTime() );

            ThreadPool.QueueUserWorkItem((state) => ParamCostTime("美国","wangfeng"));
        }
        //Task方式创建线程
        public void CreateTaskThread()
        {
            //Task.Factory.StartNew(()=>CostTime());
            Task.Factory.StartNew(() => ParamCostTime("新加坡","wangfeng"));
        }
    }
}
