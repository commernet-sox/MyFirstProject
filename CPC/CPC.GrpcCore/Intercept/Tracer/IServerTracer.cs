﻿using Grpc.Core;
using System;

namespace CPC.GrpcCore
{
    public interface IServerTracer
    {
        string ServiceName { get; set; }

        void Request<TRequest>(TRequest request, ServerCallContext context);

        void Response<TResponse>(TResponse response, ServerCallContext context);

        void Finish(ServerCallContext context);

        void Exception<TRequest>(ServerCallContext context, Exception exception, TRequest request = default);
    }
}
