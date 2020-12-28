using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CPC.Http
{
    public interface IClient : IDisposable
    {
        /// <summary>
        /// the underlying HTTP client.
        /// </summary>
        HttpClient BaseClient { get; }

        /// <summary>
        /// the base url
        /// </summary>
        Uri BaseUrl { get; }

        /// <summary>
        /// interceptors which can read and modify HTTP requests and responses.
        /// </summary>
        ICollection<IHttpFilter> Filters { get; }

        /// <summary>
        /// the request coordinator
        /// </summary>
        IRequestCoordinator RequestCoordinator { get; }


        /// <summary>
        /// create an asynchronous HTTP request message (but don't dispatch it yet).
        /// </summary>
        /// <param name="message">the HTTP request message to send.</param>
        /// <returns>returns a request builder.</returns>
        IRequest SendAsync(HttpRequestMessage message);

        /// <summary>
        /// set HTTP client timeout
        /// </summary>
        /// <param name="timeout"></param>
        IClient SetTimeout(TimeSpan timeout);

        /// <summary>
        /// specify the authentication that will be used with every request.
        /// </summary>
        /// <param name="scheme">the scheme to use for authorization. e.g.: "Basic", "Bearer".</param>
        /// <param name="parameter">the credentials containing the authentication information.</param>
        IClient SetAuthentication(string scheme, string parameter);

        /// <summary>
        /// set default options for all requests.
        /// </summary>
        /// <param name="options">the options to set. (Fields set to <c>null</c> won't change the current value.)</param>
        IClient SetOptions(RequestOptions options);

        /// <summary>
        /// set the default user agent header.
        /// </summary>
        /// <param name="userAgent">the user agent header value.</param>
        IClient SetUserAgent(string userAgent);

        /// <summary>
        /// set the default request coordinator.
        /// </summary>
        /// <param name="requestCoordinator">the request coordinator (or null to use the default behaviour).</param>
        IClient SetRequestCoordinator(IRequestCoordinator requestCoordinator);

        /// <summary>
        /// add a default behaviour for all subsequent HTTP requests.
        /// </summary>
        /// <param name="apply">the default behaviour to apply.</param>
        IClient AddDefault(Func<IRequest, IRequest> apply);
    }
}
