using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public class LoggerFactory
    {
        public static ILogger Logger
        {
            get
            {
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
