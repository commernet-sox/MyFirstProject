using Microsoft.AspNetCore.Builder;

namespace SDT.Service
{
    /// <summary>
    /// doc to https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIpRateLimiting(this IApplicationBuilder builder) => builder.UseMiddleware<IpRateLimitMiddleware>();

        public static IApplicationBuilder UseClientRateLimiting(this IApplicationBuilder builder) => builder.UseMiddleware<ClientRateLimitMiddleware>();
    }
}
