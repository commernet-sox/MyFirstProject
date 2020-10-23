using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public sealed class Request:IRequest
    {
        #region Members
        private readonly Func<IRequest, Task<HttpResponseMessage>> _dispatcher;

        public HttpRequestMessage Message { get; }

        public CancellationToken CancellationToken { get; private set; }

        public ICollection<IHttpFilter> Filters { get; }

        public RequestOptions Options { get; }

        public IRequestCoordinator RequestCoordinator { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// construct an instance.
        /// </summary>
        /// <param name="message">the underlying HTTP request message.</param>
        /// <param name="dispatcher">executes an HTTP request.</param>
        /// <param name="filters">middleware classes which can intercept and modify HTTP requests and responses.</param>
        public Request(HttpRequestMessage message, Func<IRequest, Task<HttpResponseMessage>> dispatcher, ICollection<IHttpFilter> filters)
        {
            Message = message;
            _dispatcher = dispatcher;
            Filters = filters;
            CancellationToken = CancellationToken.None;
            RequestCoordinator = null;
            Options = new RequestOptions();
        }
        #endregion

        #region Methods
        private async Task<IResponse> Execute()
        {
            // apply request filters
            foreach (var filter in Filters)
            {
                filter.OnRequest(this);
            }

            // execute the request
            var responseMessage = RequestCoordinator != null
                ? await RequestCoordinator.ExecuteAsync(this, _dispatcher).ConfigureAwait(false)
                : await _dispatcher(this).ConfigureAwait(false);
            IResponse response = new Response(responseMessage);

            // apply response filters
            foreach (var filter in Filters)
            {
                filter.OnResponse(response, !(Options.IgnoreHttpErrors ?? true));
            }

            return response;
        }

        public async Task<T> As<T>()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.As<T>().ConfigureAwait(false);
        }

        public Task<T[]> AsArray<T>() => As<T[]>();

        public async Task<byte[]> AsByteArray()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsByteArray().ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> AsMessage()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return response.Message;
        }

        public async Task<JToken> AsRawJson()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsRawJson().ConfigureAwait(false);
        }

        public async Task<JArray> AsRawJsonArray()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsRawJsonArray().ConfigureAwait(false);
        }

        public async Task<JObject> AsRawJsonObject()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsRawJsonObject().ConfigureAwait(false);
        }

        public Task<IResponse> AsResponse() => Execute();

        public async Task<Stream> AsStream()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsStream().ConfigureAwait(false);
        }

        public async Task<string> AsString()
        {
            var response = await AsResponse().ConfigureAwait(false);
            return await response.AsString().ConfigureAwait(false);
        }

        public TaskAwaiter<IResponse> GetAwaiter()
        {
            async Task<IResponse> Waiter() => await Execute().ConfigureAwait(false);

            return Waiter().GetAwaiter();
        }

        public IRequest WithArgument(string key, object value)
        {
            Message.RequestUri = Message.RequestUri.WithArguments(Options.IgnoreNullArguments ?? false, new KeyValuePair<string, object>(key, value));
            return this;
        }

        public IRequest WithArguments<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> arguments)
        {
            if (arguments == null)
            {
                return this;
            }

            var args = (
                from arg in arguments
                let key = arg.Key?.ToString()
                where !string.IsNullOrWhiteSpace(key)
                select new KeyValuePair<string, object>(key, arg.Value)
            ).ToArray();
            Message.RequestUri = Message.RequestUri.WithArguments(Options.IgnoreNullArguments ?? false, args);
            return this;
        }

        public IRequest WithArguments(object arguments)
        {
            if (arguments == null)
            {
                return this;
            }

            var args = arguments.GetKeyValueArguments().ToArray();

            Message.RequestUri = Message.RequestUri.WithArguments(Options.IgnoreNullArguments ?? false, args);
            return this;
        }

        public IRequest WithAuthentication(string scheme, string parameter)
        {
            Message.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            return this;
        }

        public IRequest WithBody(Func<IBodyBuilder, HttpContent> bodyBuilder)
        {
            Message.Content = bodyBuilder(new BodyBuilder(this));
            return this;
        }

        public IRequest WithCancellationToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }

        public IRequest WithCustom(Action<HttpRequestMessage> request)
        {
            request?.Invoke(Message);
            return this;
        }

        public IRequest WithHeader(string key, string value)
        {
            Message.Headers.Add(key, value);
            return this;
        }

        public IRequest WithOptions(RequestOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.IgnoreHttpErrors.HasValue)
            {
                Options.IgnoreHttpErrors = options.IgnoreHttpErrors.Value;
            }

            if (options.IgnoreNullArguments.HasValue)
            {
                Options.IgnoreNullArguments = options.IgnoreNullArguments.Value;
            }

            return this;
        }

        public IRequest WithRequestCoordinator(IRequestCoordinator requestCoordinator)
        {
            RequestCoordinator = requestCoordinator;
            return this;
        }
        #endregion
    }
}
