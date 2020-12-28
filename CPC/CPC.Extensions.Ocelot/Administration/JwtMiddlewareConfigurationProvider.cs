using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Repository;
using Ocelot.Middleware;
using System.Threading.Tasks;

namespace CPC.Extensions
{
    public static class JwtMiddlewareConfigurationProvider
    {
        public static OcelotMiddlewareConfigurationDelegate Get = builder =>
        {
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();

            var config = internalConfigRepo.Get();

            if (!string.IsNullOrEmpty(config.Data.AdministrationPath))
            {
                builder.Map(config.Data.AdministrationPath, app =>
                {
                    app.UseAuthentication();
                    app.UseRouting();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapControllers();
                    });
                });
            }

            return Task.CompletedTask;
        };
    }
}
