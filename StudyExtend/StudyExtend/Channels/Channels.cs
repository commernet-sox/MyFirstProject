using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace StudyExtend.Channels
{
    /// <summary>
    /// 生产者消费者模型
    /// </summary>
    public static class Channels
    {
        public static void SingleProducerSingleConsumer()
        {
            var channel = Channel.CreateUnbounded<int>();
            var reader = channel.Reader;
            Thread thread = new Thread(async () =>
            {
                await Consumer(1, reader);
            });
            Thread thread1 = new Thread(async () =>
            {
                await Consumer(2, reader);
            });
            thread.Start();
            thread1.Start();
            Thread pro1 = new Thread(async () => 
            { 
                await Producer(1, channel); 
            });
            pro1.Start();
        }
        /// <summary>
        /// 生产者方法
        /// </summary>
        /// <param name="number"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static async Task Producer(int number, Channel<int> channel)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"生产者{number}生产了{i + 1}");
                await channel.Writer.WriteAsync(i + 1);
            }
            sw.Stop();
            Console.WriteLine($"---生产者{number}耗时{sw.ElapsedMilliseconds}---");
        }
        /// <summary>
        /// 消费者方法
        /// </summary>
        /// <param name="num"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task Consumer(int num, ChannelReader<int> reader)
        {
            var sw = Stopwatch.StartNew();
            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var number))
                {
                    Console.WriteLine($"消费者{num}消费了{number}");
                }
            }
            sw.Stop();
            Console.WriteLine($"---消费者{num}耗时{sw.ElapsedMilliseconds}---");
        }
    }
}
