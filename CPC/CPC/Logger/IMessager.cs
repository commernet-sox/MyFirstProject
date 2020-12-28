using CPC.EventBus;

namespace CPC.Logger
{
    public interface IMessager
    {
        string Name { get; }

        IMessager Clone(string name);

        void Write<T>(T message) where T : IntegrationEvent;
    }
}
