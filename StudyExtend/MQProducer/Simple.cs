using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace MQProducer
{
    public class Simple
    {
        public static void Test(string[] args)
        {
            Console.WriteLine("Start");
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "guest",//用户账号
                Password = "guest"//用户密码
            };
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    String queueName = String.Empty;
                    if (args.Length > 0)
                        queueName = args[0];
                    else
                        queueName = "queue1";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    channel.BasicQos(0, 1, false);
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
        /// <summary>
        /// 订阅发布模式
        /// </summary>
        /// <param name="args"></param>
        public static void Exchange(string[] args)
        {
            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "guest",//用户账号
                Password = "guest"//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = String.Empty;
                    if (args.Length > 0)
                        exchangeName = args[0];
                    else
                        exchangeName = "exchange1";
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
        /// <summary>
        /// 路由模式
        /// </summary>
        /// <param name="args"></param>
        public static void Exchange1(string[] args)
        {
            if (args.Length == 0) throw new ArgumentException("args");
            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "guest",//用户账号
                Password = "guest"//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = "exchange2";
                    //路由名称
                    String routeKey = args[0];
                    //声明交换机   路由交换机类型direct
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息  发送到路由匹配的消息队列中
                        channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
        /// <summary>
        /// 通配符模式
        /// </summary>
        /// <param name="args"></param>
        public static void Exchange2(string[] args)
        {
            //if (args.Length == 0) throw new ArgumentException("args");
            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "guest",//用户账号
                Password = "guest"//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = "exchange3";
                    //路由名称
                    String routeKey = args[0];
                    //声明交换机   通配符类型为topic
                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息  发送到路由匹配的消息队列中
                        channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
