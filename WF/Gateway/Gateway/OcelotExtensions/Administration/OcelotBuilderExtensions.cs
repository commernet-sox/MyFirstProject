using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPC.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using CPC;

namespace Gateway.OcelotExtensions.Administration
{
    public static partial class OcelotBuilderExtensions
    {
        public static IOcelotAdministrationBuilder AddAdministration(this IOcelotBuilder ocelotBuilder, string path)
        {
            var administrationPath = new AdministrationPath(path);
            ocelotBuilder.Services.AddSingleton(JwtMiddlewareConfigurationProvider.Get);
            ocelotBuilder.Services.AddAuthentication(JwtCustomNames.Bearer).AddJwtBearer(o =>
            {
                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = c =>
                    {
                        var jwt = new JwtManager(Singleton<IConfiguration>.Instance.Bind<JwtSettings>());
                        var token = jwt.CreateToken(TimeSpan.FromDays(100), "client", "scope");
                        c.Token = token.Token;
                        return Task.CompletedTask;
                    }
                };
            });

            ocelotBuilder.Services.AddSingleton<IAdministrationPath>(administrationPath);

            return new OcelotAdministrationBuilder(ocelotBuilder.Services, ocelotBuilder.Configuration);
        }
    }
}
