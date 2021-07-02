using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CPC.Redis;

namespace StudyExtend.Multithreading
{
    public class AddCount
    {
        public static int num = 1;
        public static void test()
        {
            

            for (int i = 1; i <= 10; i++)
            {
                Task<string> task3 = Task.Run<string>(() =>
                {
                    for (int j = 1; j <= 50; j++)
                    {
                        //num++;
                        //Console.WriteLine($"当前线程为{ Thread.CurrentThread.ManagedThreadId}，值为：{num}");

                        //redis锁
                        RedisClient redisClient = new RedisClient("47.98.229.13:6379,defaultDatabase=10");
                        var res = redisClient.TryLock("CountTest",20,20,()=>{
                            //直接生成数据库操作对象
                            var optionsBuilder = new DbContextOptionsBuilder<TestContext>();
                            var connectionString = "server=47.98.229.13;database=Test;uid=sa;password=123qwe!@#";
                            var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
                            var dbContext = new TestContext(optionBuilder.Options);
                            var testApi = dbContext.MultiThread.FirstOrDefault();
                            Console.WriteLine(testApi.Id + ":" + testApi.Count);
                            dbContext.MultiThread.AsNoTracking();

                            var multi = dbContext.MultiThread.FirstOrDefault();
                            multi.Count++;
                            var res = dbContext.MultiThread.Update(multi);
                            dbContext.SaveChanges();
                            Console.WriteLine($"线程Id={ Thread.CurrentThread.ManagedThreadId},当前count= {res.Entity.Count}");
                        });
                        Console.WriteLine(res?"":$"{ Thread.CurrentThread.ManagedThreadId}---------------{res}");
                    }

                    return $"hello, task3的ID为{ Thread.CurrentThread.ManagedThreadId}";
                });
            }

        }
    }
}
