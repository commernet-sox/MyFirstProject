using System;

namespace CPC.EventBus
{
    public class SubscriptionInfo
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// 是否为同步的处理方式
        /// </summary>
        public bool Sync { get; }

        private SubscriptionInfo(bool sync, Type handlerType)
        {
            Sync = sync;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo SyncTyped(Type handlerType) => new SubscriptionInfo(true, handlerType);

        public static SubscriptionInfo AsyncTyped(Type handlerType) => new SubscriptionInfo(false, handlerType);
    }
}
