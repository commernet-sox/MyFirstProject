using Grpc.Core;
using System;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public class GrpcClientFactory<T> : IGrpcClientFactory<T>
        where T : ClientBase
    {
        private readonly IClientTracer _tracer;

        private readonly GrpcDiscovery _discovery;

        public GrpcClientFactory(GrpcDiscovery discovery, IClientTracer tracer = null)
        {
            _discovery = discovery;
            _tracer = tracer;
        }

        public T Get(string serviceName)
        {
            var callInvoker = GetCallInvoker(serviceName);
            var client = (T)Activator.CreateInstance(typeof(T), callInvoker);
            return client;
        }

        /// <summary>
        /// 获取CallInvoker
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private ClientCallInvoker GetCallInvoker(string serviceName)
        {
            var exitus = StrategyFactory.Get<T>(_discovery, serviceName);
            var callInvoker = new ClientCallInvoker(exitus.EndpointStrategy, exitus.ServiceName, exitus.RetryCount, _tracer);
            return callInvoker;
        }
    }
}