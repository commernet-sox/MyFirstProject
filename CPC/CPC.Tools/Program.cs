using AspectCore.DependencyInjection;
using CPC.EventBus;
using CPC.TaskManager;
using CPC.TaskManager.Plugins;
using Quartz;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CPC.Tools
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //EngineContext.Initialize(true, null, new ServiceContext());
            //var redis = new RedisClient("10.27.1.190:6379,password=123123");
            ////var json = File.ReadAllText("json.txt", Encoding.UTF8);
            ////redis.Set("ABCD", json, TimeSpan.FromDays(1));
            //redis.CreateLock("AAAA", 10);
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd.StartsWith("del"))
                {
                    Delete();
                }
            }



            var service = new ServiceContext();
            service.AddViewPlugin();
            EngineContext.Initialize(true, null, service);

            TaskPool.StartAsync(new JobContext { JobType = typeof(Job), CronExpression = "0/5 * * * * ? *" }).Wait();

            Console.ReadLine();
        }

        public static void Queue()
        {
            var set = new RabbitSettings() { Host = "10.27.1.28", Port = 5672, Password = "123123", User = "admin", Virtual = "HD-HZ-01" };
            var conn = RabbitConnectionPool.TryGet(set);
            conn.Operator(c =>
            {
                c.ExchangeDeclare("queue", "direct");
                c.QueueDeclare("q_001", true, false, false);
                c.QueueBind("q_001", "queue", "");
            });

            var key = Console.ReadLine().ToLowerInvariant();
            if (key == "c")
            {
                IQueueConsumer c = new RabbitConsumer(conn, "q_001");
                c.Subscribe<TestData, TestHandler>();
            }
            else if (key == "p")
            {
                var i = 0;
                IQueueProducer p = new RabbitProducer(conn, "queue", "");
                while (i < 10000)
                {
                    p.Publish(new TestData { Name = "n:" + i, SeqId = RandomUtility.Next(10000) });
                    i++;
                }
            }
        }

        public class TestData : RabbitIntegrationEvent
        {
            public int SeqId { get; set; }

            public string Name { get; set; }
        }

        public class TestHandler : IRabbitIntegrationEventHandler<TestData>
        {
            public RabbitMessageResult HandlerResult { get; set; }

            public Task Handle(TestData @event)
            {
                Console.WriteLine("接收到消息：" + @event.ToJsonEx());
                HandlerResult = RabbitMessageResult.Success;
                return Task.FromResult(0);
            }
        }

        public class Job : BaseJob
        {
            public override Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine("任务已触发" + DateTime.Now);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 修改版本号
        /// </summary>
        /// <param name="version"></param>
        private static void Version(string version)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dir = new DirectoryInfo(dir).Parent.Parent.Parent.Parent.FullName;
            var dirs = Directory.GetDirectories(dir, "cpc*");
            var suc = 0;
            foreach (var item in dirs)
            {
                var file = Directory.GetFiles(item, "*.csproj").FirstOrDefault();
                if (file == null)
                {
                    continue;
                }
                var name = Path.GetFileName(file);

                try
                {
                    var tent = File.ReadAllText(file, Encoding.UTF8);
                    var xml = XDocument.Parse(tent);
                    var elVer = xml.Root.Element("PropertyGroup")?.Element("Version")?.Value;
                    if (elVer == null)
                    {
                        continue;
                    }
                    var orig = $"<Version>{elVer}</Version>";
                    var @new = $"<Version>{version}</Version>";
                    tent = tent.Replace(orig, @new);
                    File.WriteAllText(file, tent, Encoding.UTF8);
                    suc++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(name + "出错:" + ex.ToString());
                }
            }
            Console.WriteLine(version + "版本已被修改：" + suc);
        }

        private static void Delete()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dir = new DirectoryInfo(dir).Parent.Parent.Parent.Parent.FullName;
            dir = Path.Combine(dir, "bin");
            if (!Directory.Exists(dir))
            {
                return;
            }

            foreach (var item in Directory.GetDirectories(dir))
            {
                Directory.Delete(item, true);
            }

            foreach (var item in Directory.GetFiles(dir))
            {
                if (!item.Contains("bat"))
                {
                    File.Delete(item);
                }
            }

            Console.WriteLine("delete success");

        }
    }
}
