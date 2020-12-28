using Grpc.Core;
using System;
using System.Collections.Concurrent;

namespace CPC.GrpcCore
{
    public static class GrpcClientManager
    {
        private static readonly ConcurrentDictionary<Type, string> _clientCache = new ConcurrentDictionary<Type, string>();

        public static void PreloadService<T>(string serviceName)
            where T : ClientBase
        {
            if (serviceName.IsNull())
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            _clientCache.TryAdd(typeof(T), serviceName);
        }

        internal static string GetService<T>()
            where T : ClientBase
        {
            _clientCache.TryGetValue(typeof(T), out var result);
            return result;
        }
    }
}
