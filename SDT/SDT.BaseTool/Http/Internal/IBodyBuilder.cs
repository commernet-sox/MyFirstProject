using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SDT.BaseTool
{
    public interface IBodyBuilder
    {
        /// <summary>
        /// get a form URL-encoded body.
        /// </summary>
        /// <param name="arguments">An anonymous object containing the property names and values to set.</param>
        /// <example>
        /// <code>client.WithArguments(new { id = 14, name = "Joe" })</code>
        /// </example>
        HttpContent FormUrlEncoded(object arguments);

        /// <summary>
        /// get a form URL-encoded body.
        /// </summary>
        /// <param name="arguments">An anonymous object containing the property names and values to set.</param>
        HttpContent FormUrlEncoded(IDictionary<string, string> arguments);

        /// <summary>
        /// get a form URL-encoded body.
        /// </summary>
        /// <param name="arguments">an anonymous object containing the property names and values to set.</param>
        /// <example>
        /// <code>client.WithArguments(new[] { new KeyValuePair&lt;string, string&gt;("genre", "drama"), new KeyValuePair&lt;string, int&gt;("genre", "comedy") })</code>
        /// </example>
        HttpContent FormUrlEncoded(IEnumerable<KeyValuePair<string, object>> arguments);

        /// <summary>
        /// get a serialized model body.
        /// </summary>
        /// <param name="body">the value to serialize into the HTTP body content.</param>
        /// <param name="mediaType">the request body format (or <c>null</c> to use the first supported Content-Type in the client's formatter).</param>
        /// <returns>returns the request builder for chaining.</returns>
        HttpContent Model<T>(T body, string mediaType = "application/json");

        /// <summary>
        /// get a serialized model body.
        /// </summary>
        /// <param name="body">the value to serialize into the HTTP body content.</param>
        /// <param name="encoding"></param>
        /// <param name="mediaType">the request body format (or <c>null</c> to use the first supported Content-Type in the client's formatter).</param>
        /// <returns>returns the request builder for chaining.</returns>
        HttpContent Model<T>(T body, Encoding encoding, string mediaType = "application/json");
    }
}
