using System.Collections.Generic;

namespace CPC.GrpcCore
{
    public class GrpcDiscovery
    {
        /// <summary>
        /// Consul服务地址
        /// </summary>
        public string ConsulAddress { get; set; }

        /// <summary>
        /// 是否使用Conusl作为服务发现
        /// </summary>
        public bool EnableConsul { get; set; } = true;

        /// <summary>
        /// 地址集合（不使用Conusl给对象赋值）
        /// </summary>
        public List<GrpcEndpoint> EndPoints { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; } = 5;
    }
}
