using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public static class ClientExtensions
    {
        #region Internal Methods
        /// <summary>
        /// get a copy of the request content.
        /// </summary>
        /// <param name="content">the content to copy.</param>
        /// <remarks>note that cloning content isn't possible after it's dispatched, because the stream is automatically disposed after the request.</remarks>
        internal static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null)
            {
                return null;
            }

            var stream = new MemoryStream();
            await content.CopyToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;

            var clone = new StreamContent(stream);
            foreach (var header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// get a copy of the request.
        /// </summary>
        /// <param name="request">the request to copy.</param>
        /// <remarks>note that cloning a request isn't possible after it's dispatched, because the content stream is automatically disposed after the request.</remarks>
        internal static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync().ConfigureAwait(false),
                Version = request.Version
            };

            foreach (var prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        internal static HttpResponseMessage CreateResponse(this HttpRequestMessage request, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                RequestMessage = request ?? throw new ArgumentNullException("request")
            };
            return response;
        }

        /// <summary>
        /// resolve the final URL for a request.
        /// </summary>
        /// <param name="baseUrl">the base URL.</param>
        /// <param name="resource">the requested resource.</param>
        private static Uri ResolveFinalUrl(this Uri baseUrl, string resource)
        {
            //ignore if empty or already absolute
            if (string.IsNullOrWhiteSpace(resource))
            {
                return baseUrl;
            }

            if (Uri.TryCreate(resource, UriKind.Absolute, out var absoluteUrl))
            {
                return absoluteUrl;
            }

            //parse URLs
            resource = resource.Trim();
            var builder = new UriBuilder(baseUrl);

            //special case: combine if either side is a fragment
            if (!string.IsNullOrWhiteSpace(builder.Fragment) || resource.StartsWith("#"))
            {
                return new Uri(baseUrl + resource);
            }

            //special case: if resource is a query string, validate and append it
            if (resource.StartsWith("?") || resource.StartsWith("&"))
            {
                var baseHasQuery = !string.IsNullOrWhiteSpace(builder.Query);
                if (baseHasQuery && resource.StartsWith("?"))
                {
                    throw new FormatException($"Can't add resource name '{resource}' to base URL '{baseUrl}' because the latter already has a query string.");
                }

                if (!baseHasQuery && resource.StartsWith("&"))
                {
                    throw new FormatException($"Can't add resource name '{resource}' to base URL '{baseUrl}' because the latter doesn't have a query string.");
                }

                return new Uri(baseUrl + resource);
            }

            //else make absolute URL
            if (!builder.Path.EndsWith("/"))
            {
                builder.Path += "/";
                baseUrl = builder.Uri;
            }
            return new Uri(baseUrl, resource);
        }
        #endregion

        #region Public Methods
        /// <summary>remove all HTTP filters of the specified type.</summary>
        /// <typeparam name="TFilter">the filter type.</typeparam>
        /// <param name="filters">the filters to adjust.</param>
        /// <returns>returns whether a filter was removed.</returns>
        public static bool Remove<TFilter>(this ICollection<IHttpFilter> filters)
            where TFilter : IHttpFilter
        {
            var remove = filters.OfType<TFilter>().ToArray();
            foreach (var filter in remove)
            {
                filters.Remove(filter);
            }

            return remove.Any();
        }

        /// <summary>
        /// create an asynchronous HTTP request message (but don't dispatch it yet).
        /// </summary>
        /// <param name="client">the client.</param>
        /// <param name="method">the HTTP method.</param>
        /// <param name="resource">the URI to send the request to.</param>
        /// <returns>returns a request builder.</returns>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public static IRequest SendAsync(this IClient client, HttpMethod method, string resource)
        {
            var uri = client.BaseUrl.ResolveFinalUrl(resource);
            var request = new HttpRequestMessage(method, uri);
            return client.SendAsync(request);
        }

        /// <summary>
        /// set the default authentication header using basic auth.
        /// </summary>
        /// <param name="client">the client.</param>
        /// <param name="username">the username.</param>
        /// <param name="password">the password.</param>
        public static IClient SetBasicAuthentication(this IClient client, string username, string password) => client.SetAuthentication("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(username, ":", password))));

        /// <summary>
        /// set the default authentication header using a bearer token.
        /// </summary>
        /// <param name="client">the client.</param>
        /// <param name="token">the bearer token (typically an API key).</param>
        public static IClient SetBearerAuthentication(this IClient client, string token) => client.SetAuthentication("Bearer", token);

        /// <summary>
        /// set the default request coordinator.
        /// </summary>
        /// <param name="client">the client.</param>
        /// <param name="config">the retry configuration (or null for the default coordinator).</param>
        public static IClient SetRequestCoordinator(this IClient client, IRetryProfile config) => client.SetRequestCoordinator(new RetryCoordinator(config));

        public static IRequest GetAsync(this IClient client, string resource = "") => client.SendAsync(HttpMethod.Get, resource);

        public static IRequest DeleteAsync(this IClient client, string resource = "") => client.SendAsync(HttpMethod.Delete, resource);

        public static IRequest PostAsync<TBody>(this IClient client, TBody body, string resource = "") => client.SendAsync(HttpMethod.Post, resource).WithBody(body);

        public static IRequest PutAsync(this IClient client, string resource = "") => client.SendAsync(HttpMethod.Put, resource);

        /// <summary>
        /// add an authentication header using basic auth.
        /// </summary>
        /// <param name="request">the request.</param>
        /// <param name="username">the username.</param>
        /// <param name="password">the password.</param>
        public static IRequest WithBasicAuthentication(this IRequest request, string username, string password) => request.WithAuthentication("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(username, ":", password))));

        /// <summary>
        /// add an authentication header using a bearer token.
        /// </summary>
        /// <param name="request">the request.</param>
        /// <param name="token">the bearer token (typically an API key).</param>
        public static IRequest WithBearerAuthentication(this IRequest request, string token) => request.WithAuthentication("Bearer", token);

        /// <summary>set the body content of the HTTP request.</summary>
        /// <param name="request">the request.</param>
        /// <param name="body">the model to serialize into the HTTP body content, or an <c>HttpContent</c> instance.</param>
        /// <param name="mediaType">media type</param>
        /// <returns>returns the request builder for chaining.</returns>
        public static IRequest WithBody<T>(this IRequest request, T body, string mediaType = "application/json")
        {
            if (body == null)
            {
                return request;
            }

            // HttpContent
            if (typeof(HttpContent).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                return request.WithBody(p => (HttpContent)(object)body);
            }

            // model
            return request.WithBody(p => p.Model(body, mediaType));
        }

        public static IClient UriEx(this HttpClient client, string url) => new Client(client, url);
        #endregion
    }
}
