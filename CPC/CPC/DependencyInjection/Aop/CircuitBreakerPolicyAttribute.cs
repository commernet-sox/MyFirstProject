using AspectCore.DynamicProxy;
using Polly;
using System;
using System.Threading.Tasks;

namespace CPC
{
    /// <summary>
    /// 断路器策略
    /// </summary>
    public class CircuitBreakerPolicyAttribute : BasePolicyAttribute
    {
        #region Constructors
        public CircuitBreakerPolicyAttribute(int exceptionsAllowed, int breakTime = 1000)
        {
            ExceptionsAllowed = exceptionsAllowed;
            BreakTime = TimeSpan.FromMilliseconds(breakTime);
        }
        #endregion

        /// <summary>
        /// 进行断路保护之前，允许发生错误的次数
        /// </summary>
        public int ExceptionsAllowed { get; set; } = 0;

        /// <summary>
        /// 断路保护的时间
        /// </summary>
        public TimeSpan BreakTime { get; set; } = TimeSpan.FromSeconds(0);

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ServiceMethod;

            if (!Polices.TryGetValue(method, out var policy))
            {
                if (ExceptionsAllowed > 0 && BreakTime.TotalMilliseconds > 0)
                {
                    policy = Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowed, BreakTime);
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
