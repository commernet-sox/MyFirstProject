using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SDT.Service
{
    public class JwtMessageReceivedContext : ResultContext<JwtBearerOptions>
    {
        public JwtMessageReceivedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        /// <summary>
        /// Bearer Token. This will give the application an opportunity to retrieve a token from an alternative location.
        /// </summary>
        public string Token { get; set; }
    }
}