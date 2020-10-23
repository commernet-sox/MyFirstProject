using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    /// <summary>
    /// builds and dispatches an asynchronous HTTP request, and asynchronously parses the response.
    /// </summary>
    public interface IRequest
    {
        #region Members
        /// <summary>
        /// the underlying HTTP request message.
        /// </summary>
        HttpRequestMessage Message { get; }

        /// <summary>
        /// the optional token used to cancel async operations.
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// middleware classes which can intercept and modify HTTP requests and responses.
        /// </summary>
        ICollection<IHttpFilter> Filters { get; }

        /// <summary>
        /// the request options.
        /// </summary>
        RequestOptions Options { get; }

        /// <summary>
        /// the request coordinator
        /// </summary>
        IRequestCoordinator RequestCoordinator { get; }
        #endregion

        #region Methods
        /// <summary>
        /// set the body content of the HTTP request.
        /// </summary>
        /// <param name="bodyBuilder">the HTTP body builder.</param>
        /// <returns>returns the request builder for chaining.</returns>
        IRequest WithBody(Func<IBodyBuilder, HttpContent> bodyBuilder);

        /// <summary>
        /// set an HTTP header.
        /// </summary>
        /// <param name="key">the key of the HTTP header.</param>
        /// <param name="value">the value of the HTTP header.</param>
        /// <returns>returns the request builder for chaining.</returns>
        IRequest WithHeader(string key, string value);

        /// <summary>
        /// add an HTTP query string argument.
        /// </summary>
        /// <param name="key">the key of the query argument.</param>
        /// <param name="value">the value of the query argument.</param>
        /// <returns>returns the request builder for chaining.</returns>
        IRequest WithArgument(string key, object value);

        /// <summary>
        /// add HTTP query string arguments.
        /// </summary>
        /// <param name="arguments">the arguments to add.</param>
        /// <returns>returns the request builder for chaining.</returns>
        /// <example>
        /// <code>client.WithArguments(new[] { new KeyValuePair&lt;string, string&gt;("genre", "drama"), new KeyValuePair&lt;string, int&gt;("genre", "comedy") })</code>
        /// </example>
        IRequest WithArguments<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> arguments);

        /// <summary>
        /// add HTTP query string arguments.
        /// </summary>
        /// <param name="arguments">an anonymous object where the property names and values are used.</param>
        /// <returns>
        /// returns the request builder for chaining.
        /// </returns>
        /// <example>
        /// <code>client.WithArguments(new { id = 14, name = "Joe" })</code>
        /// </example>
        IRequest WithArguments(object arguments);

        /// <summary>
        /// customize the underlying HTTP request message.
        /// </summary>
        /// <param name="request">the HTTP request message.</param>
        /// <returns>returns the request builder for chaining.</returns>
        IRequest WithCustom(Action<HttpRequestMessage> request);

        /// <summary>
        /// specify the token that can be used to cancel the async operation.
        /// </summary>
        /// <param name="cancellationToken">the cancellationtoken.</param>
        /// <returns>returns the request builder for chaining.</returns>
        IRequest WithCancellationToken(CancellationToken cancellationToken);

        /// <summary>
        /// add an authentication header for this request.
        /// </summary>
        /// <param name="scheme">the authentication header scheme to use for authorization (like 'basic' or 'bearer').</param>
        /// <param name="parameter">the authentication header value (e.g. the bearer token).</param>
        IRequest WithAuthentication(string scheme, string parameter);

        /// <summary>
        /// set options for this request.
        /// </summary>
        /// <param name="options">the options to set. (Fields set to <c>null</c> won't change the current value.)</param>
        IRequest WithOptions(RequestOptions options);

        /// <summary>
        /// set the request coordinator for this request.
        /// </summary>
        /// <param name="requestCoordinator">the request coordinator (or null to use the default behaviour).</param>
        IRequest WithRequestCoordinator(IRequestCoordinator requestCoordinator);


        /// <summary>
        /// get an object that waits for the completion of the request. This enables support for the <c>await</c> keyword.
        /// </summary>
        /// <example>
        /// <code>await client.GetAsync("api/ideas").AsString();</code>
        /// <code>await client.PostAsync("api/ideas", idea);</code>
        /// </example>
        TaskAwaiter<IResponse> GetAwaiter();

        /// <summary>
        /// asynchronously retrieve the HTTP response. This method exists for discoverability but isn't strictly needed; you can just await the request (like <c>await GetAsync()</c>) to get the response.
        /// </summary>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<IResponse> AsResponse();

        /// <summary>
        /// asynchronously retrieve the HTTP response message.
        /// </summary>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<HttpResponseMessage> AsMessage();

        /// <summary>
        /// asynchronously retrieve the response body as a deserialized model.
        /// </summary>
        /// <typeparam name="T">the response model to deserialize into.</typeparam>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<T> As<T>();

        /// <summary>
        /// asynchronously retrieve the response body as a list of deserialized models.
        /// </summary>
        /// <typeparam name="T">the response model to deserialize into.</typeparam>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<T[]> AsArray<T>();

        /// <summary>
        /// asynchronously retrieve the response body as an array of <see cref="byte"/>.
        /// </summary>
        /// <returns>returns the response body, or <c>null</c> if the response has no body.</returns>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<byte[]> AsByteArray();

        /// <summary>
        /// asynchronously retrieve the response body as a <see cref="string"/>.
        /// </summary>
        /// <returns>returns the response body, or <c>null</c> if the response has no body.</returns>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<string> AsString();

        /// <summary>
        /// asynchronously retrieve the response body as a <see cref="Stream"/>.
        /// </summary>
        /// <returns>returns the response body, or <c>null</c> if the response has no body.</returns>
        /// <exception cref="HttpException">an error occurred processing the response.</exception>
        Task<Stream> AsStream();

        /// <summary>
        /// get a raw JSON representation of the response, which can also be accessed as a <c>dynamic</c> value.
        /// </summary>
        Task<JToken> AsRawJson();

        /// <summary>
        /// get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.
        /// </summary>
        Task<JObject> AsRawJsonObject();

        /// <summary>
        /// get a raw JSON array representation of the response, which can also be accessed as a <c>dynamic</c> value.
        /// </summary>
        Task<JArray> AsRawJsonArray();
        #endregion
    }
}
