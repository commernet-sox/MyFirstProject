using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public interface IMessager
    {
        string Name { get; }

        IMessager Clone(string name);

        void Write<T>(T message) where T : IntegrationEvent;
    }
}
