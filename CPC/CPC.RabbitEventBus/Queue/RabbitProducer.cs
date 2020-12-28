using CPC.Logger;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace CPC.EventBus
{
    public class RabbitProducer : IQueueProducer
    {
        #region Members
        private readonly IRabbitConnection _persistentConnection;
        private readonly string _exchange;
        private readonly string _routingKey;
        private readonly int _retryCount;
        private readonly ILogger _logger;

        private IModel _channel;
        #endregion

        #region Constructors
        public RabbitProducer(IRabbitConnection connection, string exchange, string routingKey, int retryCount = 5, ILogger logger = null)
        {
            _persistentConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _exchange = exchange;
            _routingKey = routingKey;
            _retryCount = retryCount;
            _logger = logger ?? new NLogger();
        }
        #endregion

        #region Methods
        public void Publish<T>(T @event)
            where T : IntegrationEvent
        {
            CreateChannel();
            var policy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>().WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (ex, time) =>
            {
                _logger.Warn(ex, "could not publish event: {id} after {time}s ({message})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
            });

            var body = @event.SerializeEx();

            policy.Execute(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                if (@event is RabbitIntegrationEvent rabbitEvent)
                {
                    properties.Priority = rabbitEvent.Priority;
                    properties.Headers = rabbitEvent.Headers;
                }

                _channel.BasicPublish(_exchange, _routingKey, true, properties, body);
            });
        }

        private void CreateChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _persistentConnection.CreateModel();
            }
        }

        public void Dispose() => _channel?.Dispose();
        #endregion

    }
}
