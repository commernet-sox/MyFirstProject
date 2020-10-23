using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace SDT.BaseTool
{
    public interface IRetryProfile
    {
        /// <summary>
        ///the maximum number of times to retry a request before failing
        /// </summary>
        int MaxRetries { get; }

        /// <summary>
        /// get whether a request should be retried.
        /// </summary>
        /// <param name="response">the last HTTP response received.</param>
        bool ShouldRetry(HttpResponseMessage response);

        /// <summary>
        /// get the time to wait until the next retry.
        /// </summary>
        /// <param name="retry">the retry index (starting at 1).</param>
        /// <param name="response">the last HTTP response received.</param>
        TimeSpan GetDelay(int retry, HttpResponseMessage response);
    }
}
