using System.Threading.Tasks;

namespace CPC.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
