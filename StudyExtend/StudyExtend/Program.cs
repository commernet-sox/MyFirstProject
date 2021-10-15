﻿using AspectCore.DependencyInjection;
using AutoMapper;
using NLog;
using StudyExtend.AspectCore;
using StudyExtend.AutoMapper;
using StudyExtend.Polly;
using StudyExtend.Redis;
using StudyExtend.Channels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyExtend.ReptileTool;
using StudyExtend.CSharp8;
using StudyExtend.HtmlToImage;
using StudyExtend.Multithreading;
using StudyExtend.AGVPath;
using StudyExtend.AutoJson;
using StudyExtend.WebSocket;
using SuperSocket;
using SuperSocket.ProtoBase;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Hosting;
using StudyExtend.RequestProvider;
using System.IO;

namespace StudyExtend
{
    public class Program
    {
        public static Logger logger = LogManager.GetLogger("SimpleDemo");
        public static async Task Main(string[] args)
        {
            //AOP动态代码植入
            //DI
            //var services = new ServiceContext();
            //services.AddType<SampleService>(Lifetime.Scoped);
            //var scope = services.Build();
            //scope.Resolve<SampleService>().GetCount();
            //Console.WriteLine("控制台正在运行...");
            //Console.ReadKey();

            //AutoMapper实体映射
            //Order order = new Order() {Id=1,Name="wangfeng" };
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>());
            //var mapper = config.CreateMapper();
            //var orderDto = mapper.Map<OrderDTO>(order);
            //Console.WriteLine($"映射前{order.Id},{order.Name},{order.Type}");
            //Console.WriteLine($"映射后{orderDto.Id},{orderDto.Name},{orderDto.Salary}");
            //Console.ReadKey();

            //NLog日志记录
            //Console.WriteLine("执行开始");
            //logger.Error(DateTime.Now + " | NLog Hello World");
            //Console.WriteLine("执行结束");
            //Console.ReadKey();

            //Polly策略
            //Console.WriteLine("Polly开始执行...");
            ////PollyTest pollyTest = new PollyTest();
            ////var res = pollyTest.HttpInvokeAsync();
            ////Console.WriteLine(res.Result);

            //Policys policys = new Policys();
            //policys.Test();
            //Console.WriteLine("Polly执行结束...");
            //Console.ReadKey();

            //Redis
            //Console.WriteLine("Redis开始执行...");
            //RedisHelper redisHelper = new RedisHelper("10.27.1.28:6379,password=123123");
            //string value = "abcdefg";
            //bool r1 = redisHelper.SetValue("mykey", value);
            //string saveValue = redisHelper.GetValue("mykey");
            //Console.WriteLine(saveValue);
            //bool r2 = redisHelper.SetValue("mykey", "NewValue");
            //saveValue = redisHelper.GetValue("mykey");
            //Console.WriteLine(saveValue);
            //bool r3 = redisHelper.DeleteKey("mykey");
            //string uncacheValue = redisHelper.GetValue("mykey");
            //Console.WriteLine(uncacheValue);
            //Console.ReadKey();

            //Swagger接口在PollyTest1项目中展示

            //生产者-消费者 实现方式之Channels
            //Channels.Channels.SingleProducerSingleConsumer();
            //Console.ReadKey();

            //ReptileTool爬虫
            //ShenZhengCompanyInfo sz = new ShenZhengCompanyInfo();
            //sz.RequestUrl();

            //CSharp8新用法
            //UseRange.GetRange();

            //htmlToImage
            //DemoImage demoImage = new DemoImage();
            //demoImage.Demo();

            //Task
            //string content = Async.GetContent(Environment.CurrentDirectory+@"/test.txt");
            //string content = Async.GetContentAsync(Environment.CurrentDirectory + @"/test.txt").Result;
            //Console.WriteLine(content);
            //Console.WriteLine("主线程...");

            //ThreadPools.test();

            //Tasks.Tasks.test();


            //Multithreading
            //AddCount.test();


            //DownLoad
            //DownLoad.DYVideo.DownLoad_Video();


            //AGVPath
            //var res =Floyd.GetRes(17,4);
            //while (D[start, end].pre != start)
            //{
            //    Console.Write("->" + D[start, end].pre);
            //    start = D[start, end].pre;
            //}
            //Console.Write("->" + end);


            //自动生成json数据
            //RCSJson.json(5,5);


            //webSocket


            //RequestProvider

            //RequestProvider.RequestProvider requestProvider = new RequestProvider.RequestProvider();
            //TestApi testApi=new TestApi();
            //testApi.Id = 14;
            //testApi.Name = "123456";
            //testApi.CreateTime = DateTime.Now;
            //testApi.CreateBy = "wf";
            ////var res = requestProvider.GetAsync<List<TestApi>>("http://localhost:3003/TestApi/GetTestApis");

            //var res = requestProvider.PutAsync<TestApi>("http://localhost:3003/TestApi/UpdateTestApis", testApi);

            //var data = res.Result;


            //File Exist
            //var res = Directory.Exists(@"D:\MyCodes\MyFirstProject\StudyExtend");

            //基金数据
            //jijingInfo.RequestUrl();

            //file write
            File.WriteAllText("D:\\02\\test.txt","abcd");

            //执行完成
            Console.WriteLine("执行结束...");
            Console.ReadKey();
        }

        
    }
    public class PollyTest
    {
        public List<string> services = new List<string> { "localhost:3003", "localhost:3004" };
        public int serviceIndex = 0;
        public HttpClient client = new HttpClient();

        public Task<string> HttpInvokeAsync()
        {
            if (serviceIndex >= services.Count)
            {
                serviceIndex = 0;
            }
            var service = services[serviceIndex++];
            Console.WriteLine(DateTime.Now.ToString() + "开始服务：" + service);
            return client.GetStringAsync("http://" + service + "/weatherforecast");
        }
    }
}
