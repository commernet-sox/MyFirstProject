using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Core.Utils;
using System;

namespace CPC.GrpcCore
{
    public class InterceptedServerCallInvoker : ServerCallInvoker
    {
        private readonly IClientTracer _tracer;

        public InterceptedServerCallInvoker(Channel channel, IClientTracer tracer)
            : base(channel) => _tracer = GrpcPreconditions.CheckNotNull(tracer);

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
        {
            options = options.Headers == null ? options.WithHeaders(new Metadata()) : options;
            var context = new ClientInterceptorContext<TRequest, TResponse>(method, host, options);
            try
            {
                _tracer.Request(request, context);
                var response = Calls.BlockingUnaryCall(CreateCall(method, host, options), request);
                _tracer.Response(response, context);
                _tracer.Finish(context);
                return response;
            }
            catch (Exception ex)
            {
                _tracer.Exception(context, ex, request);
                throw;
            }
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
        {
            options = options.Headers == null ? options.WithHeaders(new Metadata()) : options;
            var context = new ClientInterceptorContext<TRequest, TResponse>(method, host, options);
            _tracer.Request(request, context);
            var rspCnt = Calls.AsyncUnaryCall(CreateCall(method, host, options), request);
            var rspAsync = rspCnt.ResponseAsync.ContinueWith(rspTask =>
            {
                try
                {
                    var response = rspTask.Result;
                    _tracer.Response(response, context);
                    _tracer.Finish(context);
                    return response;
                }
                catch (AggregateException ex)
                {
                    _tracer.Exception(context, ex.InnerException, request);
                    throw ex.InnerException;
                }
            });
            return new AsyncUnaryCall<TResponse>(rspAsync, rspCnt.ResponseHeadersAsync, rspCnt.GetStatus, rspCnt.GetTrailers, rspCnt.Dispose);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
        {
            options = options.Headers == null ? options.WithHeaders(new Metadata()) : options;
            var context = new ClientInterceptorContext<TRequest, TResponse>(method, host, options);
            _tracer.Request(request, context);
            var rspCnt = Calls.AsyncServerStreamingCall(CreateCall(method, host, options), request);
            var tracingResponseStream = new TracingAsyncClientStreamReader<TResponse, TRequest>(rspCnt.ResponseStream, context, _tracer.Response, _tracer.Finish, _tracer.Exception);
            return new AsyncServerStreamingCall<TResponse>(tracingResponseStream, rspCnt.ResponseHeadersAsync, rspCnt.GetStatus, rspCnt.GetTrailers, rspCnt.Dispose);
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
        {
            options = options.Headers == null ? options.WithHeaders(new Metadata()) : options;
            var context = new ClientInterceptorContext<TRequest, TResponse>(method, host, options);
            var rspCnt = Calls.AsyncClientStreamingCall(CreateCall(method, host, options));
            var tracingRequestStream = new TracingClientStreamWriter<TRequest, TResponse>(rspCnt.RequestStream, context, _tracer.Request);
            var rspAsync = rspCnt.ResponseAsync.ContinueWith(rspTask =>
            {
                try
                {
                    var response = rspTask.Result;
                    _tracer.Response(response, context);
                    _tracer.Finish(context);
                    return response;
                }
                catch (AggregateException ex)
                {
                    _tracer.Exception(context, ex.InnerException, null);
                    throw ex.InnerException;
                }
            });
            return new AsyncClientStreamingCall<TRequest, TResponse>(tracingRequestStream, rspAsync, rspCnt.ResponseHeadersAsync, rspCnt.GetStatus, rspCnt.GetTrailers, rspCnt.Dispose);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
        {
            options = options.Headers == null ? options.WithHeaders(new Metadata()) : options;
            var context = new ClientInterceptorContext<TRequest, TResponse>(method, host, options);
            var rspCnt = Calls.AsyncDuplexStreamingCall(CreateCall(method, host, options));
            var tracingRequestStream = new TracingClientStreamWriter<TRequest, TResponse>(rspCnt.RequestStream, context, _tracer.Request);
            var tracingResponseStream = new TracingAsyncClientStreamReader<TResponse, TRequest>(rspCnt.ResponseStream, context, _tracer.Response, _tracer.Finish, _tracer.Exception);
            return new AsyncDuplexStreamingCall<TRequest, TResponse>(tracingRequestStream, tracingResponseStream, rspCnt.ResponseHeadersAsync, rspCnt.GetStatus, rspCnt.GetTrailers, rspCnt.Dispose);
        }
    }
}