using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CPC.Http
{
    public class Client : IClient
    {
        #region Members
        public HttpClient BaseClient { get; protected set; }

        public Uri BaseUrl { get; protected set; }

        public ICollection<IHttpFilter> Filters { get; protected set; } = new List<IHttpFilter> { new DefaultErrorFilter() };

        public IRequestCoordinator RequestCoordinator { get; protected set; }

        protected readonly RequestOptions Options = new RequestOptions();

        protected bool IsDisposed { get; set; }

        /// <summary>
        /// the default behaviours to apply to all requests.
        /// </summary>
        protected readonly IList<Func<IRequest, IRequest>> Defaults = new List<Func<IRequest, IRequest>>();
        #endregion

        #region Constructors
        ~Client()
        {
            Dispose(false);
        }

        public Client(string url) : this(new HttpClient(), url) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        public Client(HttpClient client, string url) : this(client, new Uri(url)) { }


        public Client(HttpClient client, Uri uri)
        {
            BaseClient = client;
            BaseUrl = uri;
            //BaseClient.BaseAddress = uri;
        }

        #endregion

        #region Methods
        protected virtual void Dispose(bool isDisposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                BaseClient.Dispose();
            }

            IsDisposed = true;
        }


        public IClient AddDefault(Func<IRequest, IRequest> apply)
        {
            Defaults.Add(apply);
            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task<HttpResponseMessage> SendImplAsync(IRequest request)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Client));
            }

            //clone request (to avoid issues when resending messages)
            var requestMessage = await request.Message.CloneAsync().ConfigureAwait(false);

            //dispatch request
            return await BaseClient
                .SendAsync(requestMessage, request.CancellationToken)
                .ConfigureAwait(false);
        }

        public IRequest SendAsync(HttpRequestMessage message)
        {
            //clone the underlying message because HttpClient doesn't normally allow re-sending the same request, which would break IRequestCoordinator
            var request = new Request(message, async req => await SendImplAsync(req).ConfigureAwait(false), Filters)
                .WithRequestCoordinator(RequestCoordinator)
                .WithOptions(Options);

            foreach (var apply in Defaults)
            {
                request = apply?.Invoke(request);
            }
            return request;
        }

        public IClient SetTimeout(TimeSpan timeout)
        {
            BaseClient.Timeout = timeout;
            return this;
        }

        public IClient SetAuthentication(string scheme, string parameter)
        {
            BaseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            return this;
        }

        public IClient SetOptions(RequestOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.IgnoreHttpErrors.HasValue)
            {
                Options.IgnoreHttpErrors = options.IgnoreHttpErrors;
            }

            if (options.IgnoreNullArguments.HasValue)
            {
                Options.IgnoreNullArguments = options.IgnoreNullArguments;
            }

            return this;
        }

        public IClient SetRequestCoordinator(IRequestCoordinator requestCoordinator)
        {
            RequestCoordinator = requestCoordinator;
            return this;
        }

        public IClient SetUserAgent(string userAgent)
        {
            BaseClient.DefaultRequestHeaders.Remove("User-Agent");
            BaseClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            return this;
        }
        #endregion

    }
}
