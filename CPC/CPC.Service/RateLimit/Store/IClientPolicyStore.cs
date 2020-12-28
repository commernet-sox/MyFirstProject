using System.Threading.Tasks;

namespace CPC.Service.RateLimit
{
    public interface IClientPolicyStore : IRateLimitStore<ClientRateLimitPolicy>
    {
        Task SeedAsync();
    }
}
