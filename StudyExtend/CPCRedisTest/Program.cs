using AspectCore.DependencyInjection;
using System;
using CPC.Redis;
using CPC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using CPC.DependencyInjection;

namespace CPCRedisTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //测试redis分布式锁
            //AddCount.test();

            IServiceContext services = new ServiceContext();
            
            services.AddRedisClient("47.98.229.13:6379,defaultDatabase=10");
            EngineContext.Initialize(services);
            var redisClient = EngineContext.Resolve<RedisClient>();
            using (redisClient)
            {
                //    //string操作
                //    //redisClient.SetNx<string>("key","111",TimeSpan.FromSeconds(60));
                //    //redisClient.SetEx<string>("stringKey", "222",TimeSpan.FromSeconds(60));
                //    //redisClient.Set("stringKey",123);
                //    //redisClient.Incr("stringKey");
                //    //redisClient.IncrBy("stringKey", 10);
                //    //redisClient.DecrBy("stringKey",3);

                //    //List操作
                //    //List<STU> sTUs = new List<STU>();
                //    //STU s = new STU() { Name = "111", Desc = "111" };
                //    //STU s1 = new STU() { Name = "222", Desc = "222" };
                //    //sTUs.Add(s);
                //    //sTUs.Add(s1);
                //    //redisClient.LPush<STU>("listKey",sTUs.ToArray());
                //    //STU s2 = new STU() { Name = "333", Desc = "333" };
                //    //redisClient.LSet<STU>("listKey",s2,0);
                //    //var stu = redisClient.LIndex<STU>("listKey",0);
                //    //var res = redisClient.LLen("listKey");
                //    //Console.WriteLine(stu.Name);
                //    //var res = redisClient.LRange<STU>("listKey",0,1);
                //    //var res = redisClient.LPop<STU>("listKey");
                //    //var res = redisClient.LPush<STU>("listKey",new[] { s2});
                //    //var res = redisClient.LRem<STU>("listKey",s2);
                //    //redisClient.RPush<STU>("listKey", sTUs.ToArray());
                //    //redisClient.RPop<STU>("listKey");

                //    //Hash操作
                //    //redisClient.HSet<string, object>("hashKey","hkey","hvalue");
                //    //var res = redisClient.HLen("hashKey");
                //    //var res = redisClient.HKeys<string>("hashKey");
                //    //var res = redisClient.HGetAll<string, object>("hashKey");
                //    //var res = redisClient.HGet<string,object>("hashKey","hkey");
                //    //var res = redisClient.HExists<string>("hashKey","hkey");
                //    //var res = redisClient.HDel<string>("hashKey",new[] { "hkey"});

                //    //Set操作
                //    //List<STU> sTUs = new List<STU>();
                //    //STU s = new STU() { Name = "111", Desc = "111" };
                //    //STU s1 = new STU() { Name = "222", Desc = "222" };
                //    //sTUs.Add(s);
                //    //sTUs.Add(s1);
                //    //var res = redisClient.SAdd<STU>("setKey",sTUs.ToArray());
                //    //var res = redisClient.SCard("setKey");
                //    //var res = redisClient.SDiff<STU>(new[] { "setKey","set1"});
                //    //var res = redisClient.SInter<STU>(new[] { "setKey"});
                //    //var res = redisClient.SIsMember<STU>("setKey",s);
                //    //var res = redisClient.SMembers<STU>("setKey");
                //    //var res = redisClient.SRandMember<STU>("setKey");

                //    //SortSet操作
                //    //Dictionary<STU, double> keyValuePairs = new Dictionary<STU, double>();
                //    //STU s = new STU() { Name = "111", Desc = "111" };
                //    //STU s1 = new STU() { Name = "222", Desc = "222" };
                //    //keyValuePairs.Add(s,1);
                //    //keyValuePairs.Add(s1,2);
                //    ////var res = redisClient.ZAdd<STU>("ZKey", keyValuePairs);
                //    //var res = redisClient.ZScore<STU>("ZKey",s);

                //    //Other
                //    //redisClient.Subscriber("111", (t1, t2) => { Console.WriteLine(t1+t2); });
                //    //redisClient.Publish("111","wangfeng");
                //    //Console.ReadKey();

                //    //分布式锁
                //    //var sw = Stopwatch.GetTimestamp();
                //    //int i = 0;
                //    //foreach (var item in Enumerable.Range(1, 10))
                //    //{
                //    //    Task.Factory.StartNew(() =>
                //    //    {
                //    //        foreach (var it in Enumerable.Range(1, 50))
                //    //        {
                //    //            i++;
                //    //            Console.WriteLine($"当前线程{Thread.CurrentThread.ManagedThreadId},i={i}");
                //    //        }
                //    //    });
                //    //}
                //    //Console.ReadKey();
                //    //var sw1 = Stopwatch.GetTimestamp();
                //    //var ticks = (sw1 - sw) * (long)(TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency);
                //    //Console.WriteLine(new TimeSpan(ticks).TotalSeconds);

                //    //Console.WriteLine(res);
            }
        }
    }
    public class STU
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }

}
