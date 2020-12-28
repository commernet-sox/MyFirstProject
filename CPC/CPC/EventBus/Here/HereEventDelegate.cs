using System.Threading.Tasks;

namespace CPC.EventBus
{
    public delegate Task<TResponse> HereEventDelegate<TResponse>();

    public delegate Task HereEventDelegate();
}
