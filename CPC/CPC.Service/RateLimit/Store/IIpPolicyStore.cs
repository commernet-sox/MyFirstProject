using System.Collections.Generic;
using System.Threading.Tasks;

namespace CPC.Service.RateLimit
{
    public interface IIpPolicyStore : IRateLimitStore<List<IpRateLimitPolicy>>
    {
        Task SeedAsync();
    }
}
