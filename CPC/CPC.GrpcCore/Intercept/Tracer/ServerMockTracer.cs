using Grpc.Core;
using System;

namespace CPC.GrpcCore
{
    public class ServerMockTracer : IServerTracer
    {
        public string ServiceName { get; set; }

        public void Exception<TRequest>(ServerCallContext context, Exception exception, TRequest request = default) => Console.WriteLine("some exception");

        public void Finish(ServerCallContext context) => Console.WriteLine("finished request");

        public void Request<TRequest>(TRequest request, ServerCallContext context) => Console.WriteLine("start request");

        public void Response<TResponse>(TResponse response, ServerCallContext context) => Console.WriteLine("end response");
    }
}
