using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace SDT.BaseTool
{
    public class RetryProfile: IRetryProfile
    {
        #region Members
        public int MaxRetries { get; }

        /// <summary>
        /// a method which indicates whether a request should be retried.
        /// </summary>
        private readonly Func<HttpResponseMessage, bool> _shouldRetryCallback;

        /// <summary>
        /// a method which returns the time to wait until the next retry. This is passed the retry index (starting at 1) and the last HTTP response received.
        /// </summary>
        private readonly Func<int, HttpResponseMessage, TimeSpan> _getDelayCallback;
        #endregion

        #region Constructors
        /// <summary>
        /// initializes a new instance of the class.
        /// </summary>
        /// <param name="maxRetries">the maximum number of retries.</param>
        /// <param name="shouldRetry">a method which indicates whether a request should be retried.</param>
        /// <param name="getDelay">a method which returns the time to wait until the next retry. This is passed the retry index (starting at 1) and the last HTTP response received.</param>
        public RetryProfile(int maxRetries, Func<HttpResponseMessage, bool> shouldRetry, Func<int, HttpResponseMessage, TimeSpan> getDelay)
        {
            MaxRetries = maxRetries;
            _shouldRetryCallback = shouldRetry;
            _getDelayCallback = getDelay;
        }
        #endregion

        public TimeSpan GetDelay(int retry, HttpResponseMessage response) => _getDelayCallback.Invoke(retry, response);

        public bool ShouldRetry(HttpResponseMessage response) => _shouldRetryCallback.Invoke(response);

        /// <summary>Get a retry config indicating no request should be retried.</summary>
        public static IRetryProfile None() => new RetryProfile(
                maxRetries: 0,
                shouldRetry: response => false,
                getDelay: (attempt, response) => TimeSpan.Zero
            );
    }
}
