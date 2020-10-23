using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SDT.BaseTool;

namespace SDT.Service
{
    public class JwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options) => options.JwtSettings ??= Singleton<IConfiguration>.Instance.Bind<JwtSettings>();
    }
}
