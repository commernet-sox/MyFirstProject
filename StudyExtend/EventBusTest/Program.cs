using System;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using CPC;
using CPC.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CPC.Redis;

namespace EventBusTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var services = new ServiceContext();
            //RedisClient redisClient = new RedisClient("47.98.229.13:6379,defaultDatabase=10");
            services.AddHereEventBus(typeof(Program).Assembly);
            services.AddBatchEventBus(typeof(Program).Assembly);
            services.AddRedisEventBus();
            services.AddRedisClient("47.98.229.13:6379,defaultDatabase=10");
            EngineContext.Initialize(services);
            var hereEvent = EngineContext.Resolve<IHereEventBus>();
            var batchEvent= EngineContext.Resolve<IBatchEventBus>();
            var redisEvent = EngineContext.Resolve<RedisEventBus>();
            int i = 0;
            redisEvent.Subscribe<Stu, School>();
            foreach (var item in Enumerable.Range(0, 100))
            {
                //Stu stu = new Stu();
                //stu.Name = item.ToString();
                //异步处理
                //hereEvent.Publish(stu);

                //同步处理
                //Company company = new Company();
                //company.Id = item.ToString();
                //var res = hereEvent.Send<int>(company);
                //Console.WriteLine(res.Result);

                //批量处理
                //batchEvent.Publish(stu);
                //i++;
                //if (i%2==1)
                //{
                //    Thread.Sleep(200);
                //}

                //redis Event

                Stu stu = new Stu();
                stu.Name = item.ToString();
                School school = new School();
                redisEvent.Publish(stu);
            }
            
        }
        

    }
}

public class Stu:IEvent
{
    public string Name { get; set; }
}

public class School : IEventHandler<Stu>
{
    public Task Handle(Stu @event)
    {
        Console.WriteLine($"{@event.Name}处理中...");
        return Task.CompletedTask;
    }
}

public class Company : IEvent<int>
{
    public string Id { get; set; }
}

public class Emp : IEventHandler<Company, int>
{
    public Task<int> Handle(Company @event)
    {
        Console.WriteLine($"{@event.Id}处理中。。。");
        return Task.FromResult(0);
    }
}

public class A : IBatchEventHandler<Stu>
{
    public void Handle(IEnumerable<Stu> eventBatch)
    {
        
        Console.WriteLine($"{eventBatch.Count()}处理中...");
        foreach (var item in eventBatch)
        {
            Console.WriteLine($"{item.Name}正在处理。。。");
        }
    }
}