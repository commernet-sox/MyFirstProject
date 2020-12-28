using Newtonsoft.Json;
using System.Collections.Generic;

namespace CPC.EventBus
{
    public class RabbitIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 消息头
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, object> Headers { get; set; }

        /// <summary>
        /// 消息优先级(0-9)
        /// </summary>
        [JsonIgnore]
        public byte Priority { get; set; } = 5;
    }
}
