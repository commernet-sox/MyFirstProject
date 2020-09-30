using AspectCore.DynamicProxy;
using Polly;
using Polly.Timeout;
using System;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public class WrapPolicyAttribute : BasePolicyAttribute
    {
        #region Members
        #region Retry
        /// <summary>
        /// 重试次数，为0这表示不启动重试
        /// </summary>
        public int RetryTimes { get; set; } = 0;

        /// <summary>
        /// 每次重试的间隔时间
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(0);
        #endregion

        #region Timeout
        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(0);
        #endregion

        #region CircuitBreaker
        /// <summary>
        /// 进行断路保护之前，允许发生错误的次数
        /// </summary>
        public int ExceptionsAllowed { get; set; } = 0;

        /// <summary>
        /// 断路保护的时间
        /// </summary>
        public TimeSpan BreakTime { get; set; } = TimeSpan.FromSeconds(0);
        #endregion

        #region Bulkhead
        /// <summary>
        /// 最大线程数(并发数)
        /// </summary>
        public int MaxParallelization { get; set; } = 0;

        /// <summary>
        /// 正在在排队的队列数
        /// </summary>
        public int MaxQueuingActions { get; set; } = 0;
        #endregion
        #endregion

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ServiceMethod;

            if (!Polices.TryGetValue(method, out var policy))
            {
                policy = Policy.NoOpAsync();

                //熔断策略
                if (ExceptionsAllowed > 0 && BreakTime.TotalMilliseconds > 0)
                {
                    policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowed, BreakTime));
                }

                //超时策略
                if (Timeout.TotalMilliseconds > 0)
                {
                    policy = policy.WrapAsync(Policy.TimeoutAsync(Timeout, TimeoutStrategy.Pessimistic));
                }

                //重试策略
                if (RetryTimes > 0)
                {
                    policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(RetryTimes, t => RetryInterval));
                }

                //隔板隔离策略
                if (MaxParallelization > 0 && MaxQueuingActions > 0)
                {
                    policy = policy.WrapAsync(Policy.BulkheadAsync(MaxParallelization, MaxQueuingActions));
                }

                Polices.TryAdd(method, policy);
            }

            await policy.ExecuteAsync(() => next(context));
        }
    }
}
