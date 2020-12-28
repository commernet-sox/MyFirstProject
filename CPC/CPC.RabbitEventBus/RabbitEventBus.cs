using CPC.Logger;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CPC.EventBus
{
    public class RabbitEventBus : IEventBus, IDisposable
    {
        #region Members
        private const string BrokerName = "CPC_event_bus";

        private readonly IRabbitConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly int _retryCount;
        private readonly ILogger _logger;

        private IModel _consumerChannel;
        private string _destinationName;
        #endregion

        #region Constructors
        public RabbitEventBus(IRabbitConnection persistentConnection, IEventBusSubscriptionsManager subsManager, string queueName = null, int retryCount = 5, ILogger logger = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _destinationName = queueName;
            _retryCount = retryCount;
            _subsManager.OnEventRemoved += OnEventRemoved;
            _logger = logger ?? new NLogger();
        }
        #endregion

        #region Methods

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(BrokerName, "direct");

            channel.QueueDeclare(_destinationName, true, false, false);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.Warn(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(_destinationName, BrokerName, eventName);

                if (_subsManager.IsEmpty)
                {
                    _destinationName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public Task<TResponse> Send<TEvent, TResponse>(TEvent @event)
            where TEvent : IntegrationEvent => throw new NotSupportedException(nameof(RabbitEventBus));

        public Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (ex, time) =>
                {
                    _logger.Warn(ex, "could not publish event: {id} after {time}s ({message})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(BrokerName, "direct");

                var body = @event.SerializeEx();

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(BrokerName, eventName, true, properties, body);
                });

                return Task.CompletedTask;
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler
        {
            if (_subsManager.CheckEventHandler<TH>())
            {
                throw new NotSupportedException("subscription synchronization is not supported");
            }

            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(_destinationName, BrokerName, eventName);
                }
            }
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel == null)
            {
                _consumerChannel = CreateConsumerChannel();
            }

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += async (sender, e) =>
                {
                    var eventName = e.RoutingKey;
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());

                    try
                    {
                        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        {
                            throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                        }

                        await ProcessEvent(eventName, message);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn(ex, "Error Processing message: {msg}", message);
                    }

                    // Even on exception we take the message off the queue.
                    // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
                    // For more information see: https://www.rabbitmq.com/dlx.html
                    _consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
                };

                _consumerChannel.BasicConsume(_destinationName, false, consumer);
            }
            else
            {
                _logger.Error("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = GlobalContext.TryResolve(subscription.HandlerType);

                    if (handler == null)
                    {
                        continue;
                    }

                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType, JsonHelper.CommonSetting);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
            else
            {
                _logger.Warn("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler => _subsManager.RemoveSubscription<T, TH>();

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }
        #endregion
    }
}
