using Newtonsoft.Json;
using System;

namespace CPC.EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent() => Id = Guid.NewGuid();

        [JsonConstructor]
        public IntegrationEvent(Guid id) => Id = id;

        [JsonProperty]
        public Guid Id { get; private set; }
    }
}
