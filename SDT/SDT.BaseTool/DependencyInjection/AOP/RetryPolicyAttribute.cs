using AspectCore.DynamicProxy;
using Polly;
using System;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    /// <summary>
    /// 重试策略
    /// </summary>
    public class RetryPolicyAttribute : BasePolicyAttribute
    {
        #region Constructors
        public RetryPolicyAttribute(int retryTimes, int retryInterval = 1000)
        {
            RetryTimes = retryTimes;
            RetryInterval = TimeSpan.FromMilliseconds(retryInterval);
        }
        #endregion

        /// <summary>
        /// 重试次数，为0这表示不启动重试
        /// </summary>
        public int RetryTimes { get; set; } = 0;

        /// <summary>
        /// 每次重试的间隔时间
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ServiceMethod;

            if (!Polices.TryGetValue(method, out var policy))
            {
                if (RetryTimes > 0)
                {
                    policy = Policy.Handle<Exception>().WaitAndRetryAsync(RetryTimes, t => RetryInterval);
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
