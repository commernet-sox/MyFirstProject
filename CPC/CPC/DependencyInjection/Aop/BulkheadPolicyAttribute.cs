using AspectCore.DynamicProxy;
using Polly;
using System.Threading.Tasks;

namespace CPC
{
    /// <summary>
    /// 隔板隔离策略
    /// </summary>
    public class BulkheadPolicyAttribute : BasePolicyAttribute
    {
        #region Constructors
        public BulkheadPolicyAttribute(int maxParallelization, int maxQueuingActions)
        {
            MaxParallelization = maxParallelization;
            MaxQueuingActions = maxQueuingActions;
        }
        #endregion

        /// <summary>
        /// 最大线程数(并发数)
        /// </summary>
        public int MaxParallelization { get; set; } = 0;

        /// <summary>
        /// 正在在排队的队列数
        /// </summary>
        public int MaxQueuingActions { get; set; } = 0;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ServiceMethod;

            if (!Polices.TryGetValue(method, out var policy))
            {
                if (MaxParallelization > 0 && MaxQueuingActions > 0)
                {
                    policy = Policy.BulkheadAsync(MaxParallelization, MaxQueuingActions);
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
