using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using CPC;
using System.Threading;
using Core;

namespace StudyExtend.Multithreading
{
    public class ForChnTest
    {
        public static System.Threading.SemaphoreSlim slim1 = new SemaphoreSlim(1);
        public static System.Threading.SemaphoreSlim slim2 = new SemaphoreSlim(1);
        public static int number = 0;
        public static Task A(int num)
        {

            return Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < num; i++)
                {
                    if (number == 0)
                    {
                        slim2.Wait();
                    }
                    

                    Console.Write("For");

                    number++;

                    if (number == 2)
                    {
                        number = 0;
                        slim1.Release();
                    }
                    
                    
                }
            });

        }
        public static Task B(int num)
        {
            return Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < num; i++)
                {
                    slim1.Wait();
                    Console.WriteLine("Chn");
                    slim2.Release();
                }
            });
        }

        public static void Test()
        {
            slim1.Wait();
            int n = 1;
            foreach (var item in Enumerable.Range(1, 15))
            {
                //Console.WriteLine($"----------------{item}---------------");
                //Task.WaitAll(
                //    ForChnTest.A(item),
                //    ForChnTest.B(item)
                //);
                ForChnTest.A(item);
                ForChnTest.B(item);

            }
        }

        public static Task AA(int num)
        {
            return Task.Factory.StartNew(() => {
                for (var i = 0; i < num; i++)
                {
                    
                    Console.Write("For");

                    

                }
            });
        }

        public static Task BB(int num)
        {
            return Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < num; i++)
                {
                   
                    
                    Console.WriteLine("Chn");
                    
                }
            });
        }

        public static void Test1()
        {
            for (var i = 1; i < 10; i++)
            {
                //Task.WaitAll(AA(i),BB(i));
                Task task1 = Task.Run(() => { Console.Write("For"); });
                task1.Wait();
                Task task2 = Task.Run(() => { Console.WriteLine("Chn"); });
            }
        }
    }
}
