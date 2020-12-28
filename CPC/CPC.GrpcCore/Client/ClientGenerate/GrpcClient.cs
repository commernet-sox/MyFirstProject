using Grpc.Core;
using System;
using System.Collections.Concurrent;

namespace CPC.GrpcCore
{
    /// <summary>
    /// GrpcClient管理类
    /// </summary>
    public class GrpcClient<T> : IGrpcClient<T>
        where T : ClientBase
    {
        private readonly IGrpcClientFactory<T> _factory;

        private readonly ConcurrentDictionary<Type, T> _clientCache = new ConcurrentDictionary<Type, T>();

        public GrpcClient(IGrpcClientFactory<T> factory) => _factory = factory;

        public T Create(string serviceName = "")
        {
            if (serviceName.IsNull())
            {
                serviceName = GrpcClientManager.GetService<T>();

                if (serviceName.IsNull())
                {
                    throw new ArgumentNullException(nameof(serviceName));
                }
            }

            return _clientCache.GetOrAdd(typeof(T), key => _factory.Get(serviceName));
        }
    }
}