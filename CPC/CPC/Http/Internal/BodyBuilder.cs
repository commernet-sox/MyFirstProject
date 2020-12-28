using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CPC.Http
{
    internal class BodyBuilder : IBodyBuilder
    {
        private readonly IRequest _request;

        public BodyBuilder(IRequest request) => _request = request;

        public HttpContent FormUrlEncoded(object arguments) => FormUrlEncoded(arguments.GetKeyValueArguments());

        public HttpContent FormUrlEncoded(IDictionary<string, string> arguments) => new FormUrlEncodedContent(
                from pair in arguments
                where pair.Value != null || _request.Options.IgnoreNullArguments != true
                select new KeyValuePair<string, string>(pair.Key, pair.Value));

        public HttpContent FormUrlEncoded(IEnumerable<KeyValuePair<string, object>> arguments)
        {
            var pairs = from pair in arguments
                        where pair.Value != null || _request.Options.IgnoreNullArguments != true
                        select $"{StringUtility.UrlEncode(pair.Key)}={StringUtility.UrlEncode(pair.Value?.ToString())}";
            return new StringContent(string.Join("&", pairs), Encoding.UTF8, "application/x-www-form-urlencoded");
        }

        public HttpContent Model<T>(T body, string mediaType = "application/json") => Model(body, Encoding.UTF8, mediaType);

        public HttpContent Model<T>(T body, Encoding encoding, string mediaType = "application/json") => new StringContent(body.ToJsonEx(), encoding, mediaType);
    }
}
