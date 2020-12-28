using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware;
using System;

namespace CPC.Extensions
{
    public class OcelotPipelineConfigurationEx : OcelotPipelineConfiguration
    {
        public Action<IApplicationBuilder> Extend { get; set; }
    }
}
