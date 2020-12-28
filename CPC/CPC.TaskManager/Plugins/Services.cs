using HandlebarsDotNet;
using Quartz;

namespace CPC.TaskManager.Plugins
{
    public class Services
    {
        internal const string ContextKey = "cpc.task.services";

        public ViewOptions Options { get; set; }

        public ViewEngine ViewEngine { get; set; }

        public IHandlebars Handlebars { get; set; }

        public TypeHandlerService TypeHandlers { get; set; }

        public IScheduler Scheduler => TaskPool.Scheduler;

        internal Cache Cache { get; private set; }

        public static Services Create(ViewOptions options)
        {
            var handlebarsConfiguration = new HandlebarsConfiguration()
            {
                FileSystem = ViewFileSystemFactory.Create(options),
                ThrowOnUnresolvedBindingExpression = true,
            };

            var services = new Services()
            {
                Options = options,
                Handlebars = HandlebarsDotNet.Handlebars.Create(handlebarsConfiguration),
            };

            HandlebarsHelpers.Register(services);

            services.ViewEngine = new ViewEngine(services);
            services.TypeHandlers = new TypeHandlerService(services);
            services.Cache = new Cache(services);

            return services;
        }
    }
}
