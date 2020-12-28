using System.Collections.Generic;

namespace CPC.Logger
{
    public class QueueOption
    {
        public string QueueName { get; set; }
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;

        public string RouterKey { get; set; } = string.Empty;

        public IDictionary<string, object> Arguments { get; set; }

    }
}
