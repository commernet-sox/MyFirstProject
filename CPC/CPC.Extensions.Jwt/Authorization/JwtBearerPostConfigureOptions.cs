using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CPC.Extensions
{
    public class JwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            if (options.JwtSettings == null)
            {
                options.JwtSettings = Singleton<IConfiguration>.Instance.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? new JwtSettings();
            }
        }
    }
}
