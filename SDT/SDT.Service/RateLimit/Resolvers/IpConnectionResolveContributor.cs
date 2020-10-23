using Microsoft.AspNetCore.Http;

namespace SDT.Service
{
    public class IpConnectionResolveContributor : IIpResolveContributor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpConnectionResolveContributor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string ResolveIp() => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
