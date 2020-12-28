using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using System;

namespace CPC.Extensions
{
    public static partial class OcelotBuilderExtensions
    {
        public static IOcelotAdministrationBuilder AddAdministration(this IOcelotBuilder builder, string path, JwtSettings settings = null) => builder.AddAdministration(path, option => option.JwtSettings = settings);

        public static IOcelotAdministrationBuilder AddAdministration(this IOcelotBuilder builder, string path, Action<JwtBearerOptions> configureOptions)
        {
            var administrationPath = new AdministrationPath(path);

            builder.Services.AddSingleton(JwtMiddlewareConfigurationProvider.Get);

            builder.Services.AddAuthentication(JwtCustomNames.Bearer).AddJwtBearer(configureOptions);

            builder.Services.AddSingleton<IAdministrationPath>(administrationPath);

            return new OcelotAdministrationBuilder(builder.Services, builder.Configuration);
        }
    }
}
