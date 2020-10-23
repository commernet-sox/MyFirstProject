using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    /// <summary>
    /// asynchronously parses an HTTP response.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// whether the HTTP response was successful.
        /// </summary>
        bool IsSuccessStatusCode { get; }

        /// <summary>
        /// the HTTP status code.
        /// </summary>
        HttpStatusCode Status { get; }

        /// <summary>
        /// the underlying HTTP response message.
        /// </summary>
        HttpResponseMessage Message { get; }

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
    }
}
