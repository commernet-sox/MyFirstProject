using Grpc.Core;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 抽象类
    /// </summary>
    public interface IGrpcClient<T>
        where T : ClientBase
    {
        /// <summary>
        /// 客户端对象
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        T Create(string serviceName = "");
    }
}
