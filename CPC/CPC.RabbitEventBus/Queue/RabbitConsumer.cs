using CPC.Logger;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CPC.EventBus
{
    public class RabbitConsumer : IQueueConsumer
    {
        #region Members
        private readonly IRabbitConnection _persistentConnection;
        private readonly string _queueName;
        private readonly ILogger _logger;

        private IModel _consumerChannel;
        private string _consumerTag;
        private object _handler;
        #endregion

        #region Constructors
        public RabbitConsumer(IRabbitConnection connection, string queueName, ILogger logger = null)
        {
            _persistentConnection = connection ?? throw new ArgumentNullException(nameof(connection));

            if (queueName.IsNull())
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            _queueName = queueName;
            _logger = logger ?? new NLogger();
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (_consumerChannel != null)
            {
                throw new InvalidOperationException("this consumer has already subscribed");
            }

            if (!typeof(RabbitIntegrationEvent).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException(nameof(T));
            }

            _handler = GlobalContext.TryResolve(typeof(TH));

            if (_handler == null || !(_handler is IRabbitIntegrationEventHandler<T>))
            {
                throw new InvalidCastException(nameof(TH));
            }

            _consumerChannel = CreateConsumerChannel<T>();

            StartBasicConsume<T>();
        }

        private IModel CreateConsumerChannel<T>()
            where T : IntegrationEvent
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.CallbackException += (sender, ea) =>
            {
                _logger.Warn(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel<T>();
                StartBasicConsume<T>();
            };

            channel.BasicQos(0, 1, false);
            return channel;
        }

        private void StartBasicConsume<T>()
            where T : IntegrationEvent
        {
            if (_consumerChannel == null)
            {
                _logger.Error("StartBasicConsume can't call on _consumerChannel == null");
                return;
            }

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

            consumer.Received += async (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                try
                {
                    var integrationEvent = message.ToDataEx<T>();

                    if (integrationEvent != null)
                    {
                        integrationEvent.SetProperty(new Dictionary<string, object> { { nameof(RabbitIntegrationEvent.Headers), e.BasicProperties.Headers }, { nameof(RabbitIntegrationEvent.Priority), e.BasicProperties.Priority } });
                    }

                    await Task.Yield();
                    var handler = _handler as IRabbitIntegrationEventHandler<T>;
                    await handler.Handle(integrationEvent);
                    var result = handler.HandlerResult;
                    switch (result)
                    {
                        case RabbitMessageResult.DeadQueue:
                            _consumerChannel.BasicReject(e.DeliveryTag, false);
                            break;
                        case RabbitMessageResult.Requeue:
                            _consumerChannel.BasicReject(e.DeliveryTag, true);
                            break;
                        case RabbitMessageResult.Success:
                            _consumerChannel.BasicAck(e.DeliveryTag, false);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _consumerChannel.BasicReject(e.DeliveryTag, true);
                    _logger.Error(ex, "Error Processing message: {msg}", message);
                }
            };

            _consumerTag = _consumerChannel.BasicConsume(_queueName, false, consumer);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (string.IsNullOrWhiteSpace(_consumerTag))
            {
                throw new NullReferenceException("消费者标识为空");
            }

            _consumerChannel?.BasicCancel(_consumerTag);
        }
        #endregion
    }
}
