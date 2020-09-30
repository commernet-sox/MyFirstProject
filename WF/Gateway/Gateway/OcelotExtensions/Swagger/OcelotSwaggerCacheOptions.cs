using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.OcelotExtensions.Swagger
{
    public class OcelotSwaggerCacheOptions
    {
        public bool Enabled { get; set; }

        public string KeyPrefix { get; set; } = "OcelotSwagger#";

        public int SlidingExpirationInSeconds { get; set; } = 300;
    }
}
