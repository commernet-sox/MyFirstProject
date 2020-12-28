using CPC.Logger;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace CPC.EventBus
{
    public class RabbitConnection : IRabbitConnection
    {
        #region Members
        private IConnection _connection;
        private bool _disposed = false;
        private readonly object _root = new object();

        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private readonly ILogger _logger;

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
        #endregion

        #region Constructors
        public RabbitConnection(IConnectionFactory connectionFactory, int retryCount = 5, ILogger logger = null)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _retryCount = retryCount;
            _logger = logger ?? new NLogger();
        }

        public RabbitConnection(RabbitSettings settings, Action<IConnectionFactory> setup = null, ILogger logger = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _connectionFactory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                Password = settings.Password,
                AutomaticRecoveryEnabled = true,
                VirtualHost = settings.Virtual,
                UserName = settings.User,
                DispatchConsumersAsync = true
            };

            setup?.Invoke(_connectionFactory);

            _retryCount = settings.RetryCount;
            _logger = logger ?? new NLogger();
        }
        #endregion

        #region Methods
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            catch (IOException ex)
            {
                _logger.Fatal(ex);
            }
        }

        public bool TryConnect()
        {
            lock (_root)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (ex, time) =>
                    {
                        _logger.Warn(ex, "RabbitMQ Client could not connect after {0}s ({1})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                if (IsConnected)
                {
                    return true;
                }

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                          .CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    return true;
                }
                else
                {
                    _logger.Fatal("Fatal Error: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
        #endregion
    }
}
