using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.Service.RateLimit
{
    public interface IRateLimitProcessor
    {
        Task<IEnumerable<RateLimitRule>> GetMatchingRulesAsync(ClientRequestIdentity identity, CancellationToken cancellationToken = default);

        RateLimitHeaders GetRateLimitHeaders(RateLimitCounter? counter, RateLimitRule rule, CancellationToken cancellationToken = default);

        Task<RateLimitCounter> ProcessRequestAsync(ClientRequestIdentity requestIdentity, RateLimitRule rule, CancellationToken cancellationToken = default);

        bool IsWhitelisted(ClientRequestIdentity requestIdentity);
    }
}
