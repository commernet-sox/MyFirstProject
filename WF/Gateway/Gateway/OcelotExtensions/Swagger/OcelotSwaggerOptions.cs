using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.OcelotExtensions.Swagger
{
    public class OcelotSwaggerOptions
    {
        public OcelotSwaggerCacheOptions Cache { get; set; } = new OcelotSwaggerCacheOptions();

        public List<SwaggerEndPoint> SwaggerEndPoints { get; set; } = new List<SwaggerEndPoint>();
    }
}
