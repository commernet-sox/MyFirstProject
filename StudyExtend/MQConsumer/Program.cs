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
            Simple.Exchange2(args);
        }
    }
}
