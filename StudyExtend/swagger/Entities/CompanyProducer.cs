using CPC.EventBus;

namespace swagger.Entities
{
    public class CompanyProducer: IEvent
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
