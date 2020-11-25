using System.Collections.Generic;
using System.Net.Http;

namespace CPC.Http
{
    public interface IApiClient
    {

        /// <summary>
        /// 创建请求客户端
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bearer"></param>
        /// <returns></returns>
        IClient CreateClient(string url, bool bearer = true);

        /// <summary>
        /// 生成文件主体（注意需要using此对象）
        /// </summary>
        /// <param name="fileBuffer">文件流</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="args">其它参数</param>
        /// <returns></returns>
        MultipartFormDataContent GenFileContext(byte[] fileBuffer, string fileName, Dictionary<string, object> args);

        /// <summary>
        /// 获取访问凭证
        /// </summary>
        /// <returns></returns>
        Outcome<string> GetToken();
    }
}
