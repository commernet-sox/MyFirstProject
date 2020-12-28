using System;

namespace CPC.Extensions
{
    public class JwtSettings
    {
        /// <summary>
        /// 签发人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 签发密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 允许的服务器时间偏移量(默认10s)
        /// </summary>
        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(10);
    }
}
