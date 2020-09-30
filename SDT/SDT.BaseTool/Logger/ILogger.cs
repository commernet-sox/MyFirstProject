using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public interface ILogger
    {
        string Name { get; }

        ILogger Clone(string name);

        void Write(LogLevel level, Exception ex, string message, params object[] args);
    }
}
