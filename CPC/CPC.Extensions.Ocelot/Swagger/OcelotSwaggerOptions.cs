using System.Collections.Generic;

namespace CPC.Extensions
{
    public class OcelotSwaggerOptions
    {
        public OcelotSwaggerCacheOptions Cache { get; set; } = new OcelotSwaggerCacheOptions();

        public List<SwaggerEndPoint> SwaggerEndPoints { get; set; } = new List<SwaggerEndPoint>();
    }
}
