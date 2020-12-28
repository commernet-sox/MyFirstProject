using System;

namespace CPC.Logger
{
    public class EsLogger : ILogger
    {
        private readonly EsLoggerProcess _process;
        public string Name { get; private set; }

        public EsLogger(EsLoggerProcess process, string name = "")
        {
            Name = name;
            _process = process;
        }

        public ILogger Clone(string name) => new EsLogger(_process, name);

        public void Write(LogLevel level, Exception ex, string message, params object[] args)
        {
            var es = _process?.Invoke(Name, level, ex, message, args);
            EsPool.Write(es);
        }
    }

    public delegate EsExternal EsLoggerProcess(string name, LogLevel level, Exception ex, string message, params object[] args);
}
