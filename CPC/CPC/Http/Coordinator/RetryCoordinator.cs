using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPC.Http
{
    public class RetryCoordinator : IRequestCoordinator
    {
        #region Members
        private readonly IRetryProfile _profile;

        private const HttpStatusCode TimeoutStatusCode = (HttpStatusCode)589;
        #endregion

        #region Constructors
        public RetryCoordinator(IRetryProfile profile = null) => _profile = profile ?? RetryProfile.None();
        #endregion

        public async Task<HttpResponseMessage> ExecuteAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
        {
            var attempt = 0;
            var maxAttempt = 1 + _profile.MaxRetries;
            while (true)
            {
                // dispatch request
                attempt++;
                HttpResponseMessage response;
                try
                {
                    response = await dispatcher(request).ConfigureAwait(false);
                }
                catch (TaskCanceledException) when (!request.CancellationToken.IsCancellationRequested)
                {
                    response = request.Message.CreateResponse(TimeoutStatusCode);
                }

                //exit if done
                if (!_profile.ShouldRetry(response))
                {
                    return response;
                }

                if (attempt >= maxAttempt)
                {
                    throw new HttpException(new Response(response), $"The HTTP request {(response != null ? "failed" : "timed out")}, and the retry coordinator gave up after the maximum {_profile.MaxRetries} retries");
                }

                //set up retry
                var delay = _profile.GetDelay(attempt, response);
                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }
        }
    }
}
