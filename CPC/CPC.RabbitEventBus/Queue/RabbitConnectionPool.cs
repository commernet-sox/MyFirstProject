using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;

namespace CPC.EventBus
{
    public class RabbitConnectionPool
    {
        #region Members
        private static readonly ConcurrentDictionary<string, IRabbitConnection> _connectionPool = new ConcurrentDictionary<string, IRabbitConnection>();
        #endregion

        public static IRabbitConnection TryGet(RabbitSettings settings, Action<IConnectionFactory> setup = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var key = GetKey(settings.Host, settings.Port, settings.Virtual);
            if (_connectionPool.TryGetValue(key, out var conn))
            {
                return conn;
            }

            var connectionFactory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                Password = settings.Password,
                AutomaticRecoveryEnabled = true,
                VirtualHost = settings.Virtual,
                UserName = settings.User,
                DispatchConsumersAsync = true
            };

            setup?.Invoke(connectionFactory);

            conn = new RabbitConnection(connectionFactory, settings.RetryCount);
            _connectionPool.TryAdd(key, conn);
            return conn;
        }

        public static IRabbitConnection TryGet(ConnectionFactory connectionFactory, int retryCount = 5)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            var key = GetKey(connectionFactory.HostName, connectionFactory.Port, connectionFactory.VirtualHost);

            if (_connectionPool.TryGetValue(key, out var conn))
            {
                return conn;
            }

            conn = new RabbitConnection(connectionFactory, retryCount);
            _connectionPool.TryAdd(key, conn);
            return conn;
        }

        private static string GetKey(string host, int port, string virual) => $"{host}:{port},{virual}";

        public static void Dispose()
        {
            foreach (var item in _connectionPool)
            {
                item.Value.Dispose();
            }
            _connectionPool.Clear();
        }
    }
}
