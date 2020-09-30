using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.OcelotExtensions
{
    public class OcelotPipelineConfigurationEx: OcelotPipelineConfiguration
    {
        public Action<IApplicationBuilder> Extend { get; set; }
    }
}
