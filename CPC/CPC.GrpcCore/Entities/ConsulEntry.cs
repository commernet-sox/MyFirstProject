using System;

namespace CPC.GrpcCore
{
    public sealed class ConsulEntry : IDisposable
    {
        public string ServiceId { get; private set; }

        private readonly ServerRegister _serverRegister;

        public ConsulEntry(ServerRegister serverRegister, string serviceId)
        {
            ServiceId = serviceId;
            _serverRegister = serverRegister;
        }

        public void Dispose() => _serverRegister.Deregister(ServiceId);
    }
}
