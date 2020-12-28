using AspectCore.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CPC.Logger
{
    public static partial class LoggerExtensions
    {
        public static IServiceContext AddEsLogger(this IServiceContext services, IEnumerable<Uri> uris, EsLoggerProcess process, string name = "")
        {
            var logger = new EsLogger(process, name);
            services.AddLogger(logger);
            EsPool.Initialize(uris);
            return services;
        }
    }
}
