using RabbitMQ.Client;
using System;

namespace CPC.EventBus
{
    public interface IRabbitConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
