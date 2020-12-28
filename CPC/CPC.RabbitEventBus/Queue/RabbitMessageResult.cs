namespace CPC.EventBus
{
    public enum RabbitMessageResult : int
    {
        /// <summary>
        /// 数据处理成功,直接丢弃队列中的数据
        /// </summary>
        Success = 0,

        /// <summary>
        /// 重新进入队列
        /// </summary>
        Requeue = 1,

        /// <summary>
        /// 进入死信队列
        /// </summary>
        DeadQueue = 2
    }
}
