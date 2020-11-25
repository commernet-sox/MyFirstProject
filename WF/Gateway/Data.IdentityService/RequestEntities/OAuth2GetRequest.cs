using System.ComponentModel.DataAnnotations;

namespace Data.IdentityService
{
    public class OAuth2GetRequest
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "应用ID不能为空")]
        public string ClientId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        [Required(ErrorMessage = "密钥不能为空")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// 授权类型（client_credential,password_credential,authorization_code,refresh_token）
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        public string GrantType { get; set; } = "client_credential";


        /// <summary>
        /// 授权范围（默认为：scope）
        /// </summary>
        public string Scope { get; set; } = "scope";

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// authorization_code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// refresh_token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
