using System.Threading.Tasks;

namespace SDT.Service
{
    public interface IClientPolicyStore : IRateLimitStore<ClientRateLimitPolicy>
    {
        Task SeedAsync();
    }
}
