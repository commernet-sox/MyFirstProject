using CPC.EventBus;
using RabbitMQ.Client;

namespace CPC.Logger
{
    public class RabbitHelper
    {
        public static void QueueDeclare(RabbitContext sc)
        {
            var conn = RabbitConnectionPool.TryGet(sc.ServerOption);
            conn.Operator(c =>
            {

                c.ExchangeDeclare(sc.ExchangeName, sc.ExchangeType == DestinationType.Dead ? DestinationType.Direct.ToString() : sc.ExchangeType.ToString(), sc.Durable, sc.AutoDelete, sc.Arguments);


                sc.Queues.ForEach(t =>
                {
                    c.QueueDeclare(t.QueueName, t.Durable, t.Exclusive, t.AutoDelete, t.Arguments);
                    c.QueueBind(t.QueueName, sc.ExchangeName, t.RouterKey);
                });



            });
        }


        public static RabbitContext GetService(RabbitExternal logExternal, string fileType)
        {

            var serviceContext = new RabbitContext(logExternal, fileType, DestinationType.Fanout);
            return serviceContext;
        }


    }
}
