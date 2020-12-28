using Grpc.Core;
using Grpc.Core.Utils;

namespace CPC.GrpcCore
{
    public class ServerCallInvoker : CallInvoker
    {
        public Channel Channel { get; private set; }

        public ServerCallInvoker(Channel channel) => Channel = GrpcPreconditions.CheckNotNull(channel);

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request) => Calls.BlockingUnaryCall(CreateCall(method, host, options), request);

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request) => Calls.AsyncUnaryCall(CreateCall(method, host, options), request);

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request) => Calls.AsyncServerStreamingCall(CreateCall(method, host, options), request);

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options) => Calls.AsyncClientStreamingCall(CreateCall(method, host, options));

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options) => Calls.AsyncDuplexStreamingCall(CreateCall(method, host, options));

        protected virtual CallInvocationDetails<TRequest, TResponse> CreateCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
            where TRequest : class
            where TResponse : class => new CallInvocationDetails<TRequest, TResponse>(Channel, method, host, options);
    }
}