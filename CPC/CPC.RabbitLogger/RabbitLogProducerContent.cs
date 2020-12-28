using CPC.EventBus;
using System;

namespace CPC.Logger
{
    public class RabbitLogProducerContent<T> where T : IntegrationEvent
    {
        private readonly RabbitExternal _external;
        public CPC.Logger.ILogger _dlogger;
        public RabbitLogProducerContent(RabbitExternal external) => _external = external;


        public bool Send(T data)
        {
            var fileType = _external.FileType;
            var watch = new System.Diagnostics.Stopwatch(); //testmonica
            watch.Start();//开始计时 testmonica
            var serviceContext = RabbitHelper.GetService(_external, fileType);
            RabbitHelper.QueueDeclare(serviceContext);
            //自动建立对应的死信队列
            //ServiceContext deadServiceContext = serviceContext.GetDeadQueueServiceContext();
            // QueueDeclare(deadServiceContext);
            var result = false;
            var conn = RabbitConnectionPool.TryGet(serviceContext.ServerOption);
            using (var currentProducer = new RabbitProducer(conn, serviceContext.ExchangeName, ""))
            {
                try
                {
                    currentProducer?.Publish(data);
                    result = true;
                }
                catch (Exception ex)
                {
                    _dlogger = new NLogger();
                    _dlogger.Error(ex);
                    result = false;
                    return result;
                }
            }

            return result;
        }




    }
}
