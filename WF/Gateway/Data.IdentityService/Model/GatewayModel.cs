using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService.Model
{
    public class GatewayModel
    {
        public string ServiceName { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public string UpstreamPathTemplate { get; set; }

        public bool UseServiceDiscovery { get; set; } = true;
        public object LoadBalancerOptions { get; set; } = new { Type = "RoundRobin" };
        public Array UpstreamHttpMethod { get; set; } =new[] { "Get", "Post", "Put", "Delete" };
        public bool ReRouteIsCaseSensitive { get; set; } = false;
    }
}
