﻿using Grpc.Core.Interceptors;
using System;

namespace CPC.GrpcCore
{
    public class ClientMockTracer : IClientTracer
    {
        public string ServiceName { get; set; }

        public void Exception<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, Exception exception, TRequest request = null)
            where TRequest : class
            where TResponse : class => Console.WriteLine("some exception");

        public void Finish<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class => Console.WriteLine("finished request");

        public void Request<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class => Console.WriteLine("start request");

        public void Response<TRequest, TResponse>(TResponse response, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class => Console.WriteLine("end response");
    }
}
