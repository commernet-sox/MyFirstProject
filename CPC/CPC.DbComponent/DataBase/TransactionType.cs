namespace CPC.DbComponent
{
    public enum TransactionType
    {
        /// <summary>
        /// 常规事务
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 分布式事务
        /// </summary>
        Distributed = 1
    }
}
