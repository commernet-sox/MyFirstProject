using CPC.EventBus;
using System;
using System.Threading.Tasks;

namespace swagger.Entities
{
    public class CompanyConsumer:IEventHandler<CompanyProducer>
    {

        public Task Handle(CompanyProducer @event)
        {
            Console.WriteLine(@event.Name);
            return Task.CompletedTask;
        }
    }
}
