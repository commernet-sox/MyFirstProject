using System;

namespace MQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("消费者启动...");
            //Simple.Test(new string[] { });
            //Simple.Exchange(new string[] { });
            Simple.Exchange1(new string[] { "Exchange1" });
            //Simple.Exchange2(new string[] { "Exchange2" });
        }
    }
}
