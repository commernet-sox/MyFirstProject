using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SDT.BaseTool;

namespace SDT.Service
{
    public class JwtAuthenticationFailedContext : ResultContext<JwtBearerOptions>
    {
        public JwtAuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        public string Token { get; set; }

        public Outcome Error { get; set; }
    }
}