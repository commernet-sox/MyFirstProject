using CSharpBasic.Basic;
using System;

namespace CSharpBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //TypeTrans typeTrans = new TypeTrans();
            //typeTrans.Trans();

            //SwitchCase switchCase = new SwitchCase();
            //switchCase.index();

            //StaticVar s = new StaticVar();
            //s.count();
            //s.count();
            //s.count();
            //Console.WriteLine("变量 num： {0}", StaticVar.getNum());

            //Tabletop t = new Tabletop(4.5, 7.5);
            //t.Display();

            //Threads threads = new Threads();
            //threads.CreateSingleThread();
            //threads.CreateThreadPools();
            //threads.CreateTaskThread();

            DoCallBack doCallBack = new DoCallBack();
            CallBack callBack = new CallBack();
            //callBack.SetAddCallBack(doCallBack.add);//设置回调
            //callBack.CallAdd();//触发回调
            //匿名方法
            callBack.add = delegate (int p, int q)
            {
                Console.WriteLine("调用匿名委托方法：p={0},q={1}", p, q);
            };
            callBack.add(1,2);
            //UnsafeCode unsafeCode = new UnsafeCode();
            //unsafeCode.Unsafe2();

            Console.ReadKey();
        }
    }
}
