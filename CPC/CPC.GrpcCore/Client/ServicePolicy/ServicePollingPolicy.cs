using System.Collections.Generic;
using System.Threading;

namespace CPC.GrpcCore
{
    internal class ServicePollingPolicy
    {
        private static long _times = 0;

        /// <summary>
        /// 轮询策略
        /// </summary>
        /// <param name="callInvokers"></param>
        /// <returns></returns>
        public static ServerCallInvoker Random(List<ServerCallInvoker> callInvokers)
        {
            if ((callInvokers?.Count ?? 0) <= 0)
            {
                return null;
            }

            return callInvokers[(int)(Interlocked.Increment(ref _times) % callInvokers.Count)];
        }
    }
}
