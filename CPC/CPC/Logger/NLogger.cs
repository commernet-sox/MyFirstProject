using NLog;
using System;
using System.Diagnostics;

namespace CPC.Logger
{
    public class NLogger : ILogger
    {
        #region Members
        public string Name { get; private set; }

        private NLog.ILogger Log
        {
            get
            {
                if (Name.IsNull())
                {
                    var st = new StackTrace();
                    var type = st.GetFrame(st.FrameCount - 1).GetMethod().DeclaringType;
                    return LogManager.GetLogger(type.FullName, type);
                }
                else
                {
                    LogManager.GetCurrentClassLogger();
                    return LogManager.GetLogger(Name);
                }
            }
        }
        #endregion

        #region Constructors
        public NLogger(string name = "") => Name = name;
        #endregion

        #region Methods
        public void Write(LogLevel level, Exception ex, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    Log.Trace(ex, message, args);
                    break;
                case LogLevel.Debug:
                    Log.Debug(ex, message, args);
                    break;
                case LogLevel.Info:
                    Log.Info(ex, message, args);
                    break;
                case LogLevel.Warn:
                    Log.Warn(ex, message, args);
                    break;
                case LogLevel.Error:
                    Log.Error(ex, message, args);
                    break;
                case LogLevel.Fatal:
                    Log.Fatal(ex, message, args);
                    break;
            }
        }

        public ILogger Clone(string name) => new NLogger() { Name = name };
        #endregion
    }
}
