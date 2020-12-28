using Grpc.Core;

namespace CPC.GrpcCore
{
    /// <summary>
    /// 工厂类接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGrpcClientFactory<T>
        where T : ClientBase
    {
        /// <summary>
        /// 获取Client对象
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        T Get(string serviceName);
    }
}
