using CPC.EventBus;
using System.Collections.Generic;

namespace CPC.Logger
{
    public class RabbitContext
    {
        public string LogType { get; set; }

        public string FileType { get; set; }

        public RabbitSettings ServerOption { get; set; }
        public string ExchangeName { get; set; }
        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; }
        public string ExchangeType { get; set; } = DestinationType.Direct;
        public List<QueueOption> Queues { get; set; }

        // public Target Target { get; set; }

        /// <summary>
        /// 生产者 消费者初始化
        /// </summary>
        /// <param name="ml"></param>
        /// <param name="fileType"></param>
        /// <param name="destination"></param>
        public RabbitContext(RabbitExternal ml, string fileType, string destination = DestinationType.Direct)
        {
            // Target = tg;
            ExchangeType = destination;
            if (ml != null)
            {
                ServerOption = new RabbitSettings
                {
                    Host = ml.Host,
                    Password = ml.Password,
                    Port = ml.Port,
                    User = ml.User,
                    Virtual = ml.Virtual

                };
            }
            ServerOption.Virtual = $"{fileType}{ServerOption.Virtual}";
            Queues = new List<QueueOption>();
            ExchangeName = $"{ml.Exchangename.Trim()}{fileType}";
            Queues.Add(new QueueOption
            {
                QueueName = string.IsNullOrWhiteSpace(ml.Queuename) ? ExchangeName : ml.Queuename,
                Arguments = new Dictionary<string, object>()
                    {
                         { "x-dead-letter-exchange","dead_ex_"+ExchangeName },
                         { "x-dead-letter-routing-key","dead_rt_"+ExchangeName},
                         {"x-max-priority",100 }
                    }
            });

        }
    }
}
