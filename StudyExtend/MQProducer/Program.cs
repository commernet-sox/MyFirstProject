using System;

namespace MQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("生产者启动...");
            //Simple.Test(new string[] { });
            //Simple.Exchange(new string[] { });
            Simple.Exchange2(args);
        }
    }
}
