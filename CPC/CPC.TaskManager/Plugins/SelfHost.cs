using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CPC.TaskManager.Plugins
{
    public class SelfHost
    {
        public static async Task StartAsync(string url = "http://*:18081", Action<ViewOptions> setup = null)
        {
            var view = new ViewOptions();
            setup?.Invoke(view);

            var host = WebHost.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddView();
            }).Configure(app =>
            {
                app.UseView(view);
            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            }).UseUrls(url).Build();

            await host.RunAsync();
        }

    }
}
