using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;

namespace CPC.GrpcCore
{
    /// <summary>
    /// GRPC 服务管理
    /// </summary>
    public static class GrpcServiceManager
    {
        private static Server _server;
        private static ConsulEntry _discoveryEntry;
        private static ServerRegister _serverRegister;

        public static void Start(IEnumerable<ServerServiceDefinition> services, GrpcServiceSettings settings, string consulAddress, IServerTracer tracer = null, ServerCredentials credentials = null, List<Interceptor> interceptors = null, List<ChannelOption> channelOptions = null, Action<Exception> whenException = null)
        {
            try
            {
                if (tracer != null)
                {
                    tracer.ServiceName = settings.ServiceName;
                    interceptors = interceptors ?? new List<Interceptor>();
                    interceptors.Add(new ServerTracerInterceptor(tracer));
                }

                _server = new Server(channelOptions)
                {
                    Ports = { new ServerPort("0.0.0.0", settings.Port, credentials ?? ServerCredentials.Insecure) }
                };

                foreach (var service in services)
                {
                    if (interceptors?.Count > 0)
                    {
                        _server.Services.Add(service.Intercept(interceptors.ToArray()));
                    }
                    else
                    {
                        _server.Services.Add(service);
                    }
                }
                _server.Start();

                if (consulAddress.IsNull())
                {
                    return;
                }

                _serverRegister = new ServerRegister(consulAddress);
                _serverRegister.Register(settings, entry => _discoveryEntry = entry);
            }
            catch (Exception ex)
            {
                Stop();
                InvokeException(ex, whenException);
            }
        }

        /// <summary>
        /// Grpc服务停止
        /// </summary>
        /// <param name="whenException">==null => throw</param>
        public static void Stop(Action<Exception> whenException = null)
        {
            try
            {
                _serverRegister?.Deregister(_discoveryEntry?.ServiceId);
                _server?.ShutdownAsync().Wait();
            }
            catch (Exception ex)
            {
                InvokeException(ex, whenException);
            }
        }

        /// <summary>
        /// 执行异常
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="whenException"></param>
        private static void InvokeException(Exception exception, Action<Exception> whenException = null)
        {
            if (whenException != null)
            {
                whenException.Invoke(exception);
            }
            else
            {
                throw exception;
            }
        }
    }
}
