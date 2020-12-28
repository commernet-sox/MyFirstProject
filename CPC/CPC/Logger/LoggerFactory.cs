namespace CPC.Logger
{
    public class LoggerFactory
    {
        public static ILogger Logger
        {
            get
            {
                if (!EngineContext.Initialized)
                {
                    return null;
                }

                var logger = GlobalContext.Resolve<ILogger>();
                if (logger == null)
                {
                    return new NLogger();
                }
                return logger;
            }
        }

        public static ILogger CreateLogger(string name = "") => Logger.Clone(name);
    }
}
