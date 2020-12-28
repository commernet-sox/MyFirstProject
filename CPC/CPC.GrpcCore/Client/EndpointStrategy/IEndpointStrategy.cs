﻿namespace CPC.GrpcCore
{
    /// <summary>
    /// 单例使用
    /// </summary>
    public interface IEndpointStrategy
    {
        /// <summary>
        /// 添加服务发现
        /// </summary>
        /// <param name="serviceDiscovery"></param>
        void AddServiceDiscovery(IEndpointDiscovery serviceDiscovery);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        ServerCallInvoker Get(string serviceName);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="failedCallInvoker"></param>
        void Revoke(string serviceName, ServerCallInvoker failedCallInvoker);

        /// <summary>
        /// 定时检测
        /// </summary>
        void InitCheckTimer();
    }
}