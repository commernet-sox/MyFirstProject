using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 客户端拦截器
    /// </summary>
    internal class ClientTracerInterceptor : Interceptor
    {
        private readonly IClientTracer _tracer;

        public ClientTracerInterceptor(IClientTracer tracer) => _tracer = tracer;

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation) => new InterceptedClientHandler<TRequest, TResponse>(_tracer, context)
                .BlockingUnaryCall(request, continuation);

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation) => new InterceptedClientHandler<TRequest, TResponse>(_tracer, context)
                .AsyncUnaryCall(request, continuation);

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation) => new InterceptedClientHandler<TRequest, TResponse>(_tracer, context)
                .AsyncServerStreamingCall(request, continuation);

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation) => new InterceptedClientHandler<TRequest, TResponse>(_tracer, context)
                .AsyncClientStreamingCall(continuation);

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation) => new InterceptedClientHandler<TRequest, TResponse>(_tracer, context)
                .AsyncDuplexStreamingCall(continuation);
    }
}
