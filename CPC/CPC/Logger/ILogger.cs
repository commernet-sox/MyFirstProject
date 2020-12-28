using System;

namespace CPC.Logger
{
    public interface ILogger
    {
        string Name { get; }

        ILogger Clone(string name);

        void Write(LogLevel level, Exception ex, string message, params object[] args);
    }
}
