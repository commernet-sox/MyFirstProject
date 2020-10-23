using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public sealed class Response:IResponse
    {
        #region Members
        public bool IsSuccessStatusCode => Message.IsSuccessStatusCode;

        public HttpStatusCode Status => Message.StatusCode;

        public HttpResponseMessage Message { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// construct an instance.
        /// </summary>
        /// <param name="message">The underlying HTTP request message.</param>
        public Response(HttpResponseMessage message) => Message = message;
        #endregion

        public Task<string> AsString() => Message.Content.ReadAsStringAsync();

        public async Task<T> As<T>()
        {
            var content = await AsString();
            return content.ToDataEx<T>();
        }

        public Task<T[]> AsArray<T>() => As<T[]>();

        public Task<byte[]> AsByteArray() => Message.Content.ReadAsByteArrayAsync();

        public async Task<JToken> AsRawJson()
        {
            var content = await AsString();
            return JToken.Parse(content);
        }

        public async Task<JArray> AsRawJsonArray()
        {
            var content = await AsString();
            return JArray.Parse(content);
        }

        public async Task<JObject> AsRawJsonObject()
        {
            var content = await AsString();
            return JObject.Parse(content);
        }

        public async Task<Stream> AsStream()
        {
            var stream = await Message.Content.ReadAsStreamAsync().ConfigureAwait(false);
            stream.Position = 0;
            return stream;
        }
    }
}
