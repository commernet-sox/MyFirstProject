using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDT.Service
{
    public interface IIpPolicyStore : IRateLimitStore<List<IpRateLimitPolicy>>
    {
        Task SeedAsync();
    }
}
