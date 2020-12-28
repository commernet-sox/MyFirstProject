﻿using System;
using System.Collections.Generic;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 重点服务发现接口
    /// </summary>
    public interface IEndpointDiscovery
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; set; }

        /// <summary>
        /// 监听变动的方法
        /// </summary>
        Action Watched { get; set; }

        /// <summary>
        /// 获取服务可连接终结点
        /// </summary>
        /// <returns></returns>
        List<string> FindServiceEndpoints(bool filterBlack = true);
    }
}
