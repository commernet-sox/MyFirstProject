using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.RequestEntities
{
    public class OAuth2GetResponse
    {
        /// <summary>
        /// access token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 刷新Token，当授权类型为password_credential和refresh_token返回
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        public string Scope { get; set; } = "scope";
    }
}
