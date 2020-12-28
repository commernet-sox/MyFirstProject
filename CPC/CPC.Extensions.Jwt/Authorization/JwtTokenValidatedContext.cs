using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CPC.Extensions
{
    public class JwtTokenValidatedContext : ResultContext<JwtBearerOptions>
    {
        public JwtTokenValidatedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        public string Token { get; set; }
    }
}
