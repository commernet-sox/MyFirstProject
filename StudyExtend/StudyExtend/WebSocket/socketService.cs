using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.ProtoBase;

namespace StudyExtend.WebSocket
{
    public class socketService
    {
        public static async void test()
        {
            var host = SuperSocketHostBuilder
            .Create<StringPackageInfo, CommandLinePipelineFilter>()
            .UsePackageHandler(async (session, package) =>
            {
                var result = 0;
                try
                {
                    switch (package.Key.ToUpper())
                    {
                        case ("ADD"):
                            result = package.Parameters
                                .Select(p => int.Parse(p))
                                .Sum();
                            break;

                        case ("SUB"):
                            result = package.Parameters
                                .Select(p => int.Parse(p))
                                .Aggregate((x, y) => x - y);
                            break;

                        case ("MULT"):
                            result = package.Parameters
                                .Select(p => int.Parse(p))
                                .Aggregate((x, y) => x * y);
                            break;
                        default:
                            result = -1;
                            break;
                    }
                }
                catch (Exception ex)
                {
                }
                await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
            })
            //.ConfigureLogging((hostCtx, loggingBuilder) =>
            //{
            //    loggingBuilder.AddConsole();
            //})
            .ConfigureSuperSocket(options =>
            {
                options.Name = "My Server";
                options.Listeners = new System.Collections.Generic.List<ListenOptions> {
                    new ListenOptions
                    {
                        Ip = "Any",
                        Port = 8888
                    }
                };
            }).Build();
            await host.RunAsync();
        }
    }
}
