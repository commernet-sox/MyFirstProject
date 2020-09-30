using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
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
