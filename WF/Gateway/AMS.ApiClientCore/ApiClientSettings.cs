using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace CPC.Http
{
    public class ApiClientSettings
    {
        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayUrl { get; set; }

        /// <summary>
        /// OAuth2认证服务地址（相对地址）
        /// </summary>
        public string OAuth2Address { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 密钥(C/S工程请勿泄漏在外部)
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 授权凭证类型
        /// </summary>
        public string GrantType { get; set; } = "client_credential";

        /// <summary>
        /// 用户名（非必要情况不要使用配置，请在内部赋值）
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码（非必要情况不要使用配置，请在内部赋值）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 刷新令牌，（内部赋值，此令牌使用一次就失效,需要缓存需要传UserName）
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public string Scope { get; set; } = "scope";

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}
