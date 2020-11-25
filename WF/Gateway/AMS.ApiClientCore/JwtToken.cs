namespace CPC.Http
{
    public class JwtToken
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
        /// refresh
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        public string Scope { get; set; }
    }
}
