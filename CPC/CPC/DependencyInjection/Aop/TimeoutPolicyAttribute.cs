using AspectCore.DynamicProxy;
using Polly;
using Polly.Timeout;
using System;
using System.Threading.Tasks;

namespace CPC
{
    /// <summary>
    /// 超时策略
    /// </summary>
    public class TimeoutPolicyAttribute : BasePolicyAttribute
    {
        #region Constructors
        public TimeoutPolicyAttribute(int milliseconds = 10000) => Timeout = TimeSpan.FromMilliseconds(milliseconds);
        #endregion

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ServiceMethod;

            if (!Polices.TryGetValue(method, out var policy))
            {
                if (Timeout.TotalMilliseconds > 0)
                {
                    policy = Policy.TimeoutAsync(Timeout, TimeoutStrategy.Pessimistic);
                }
                else
                {
                    policy = Policy.NoOpAsync();
                }
                Polices.TryAdd(method, policy);
            }

            await policy.ExecuteAsync(() => next(context));
        }
    }
}
