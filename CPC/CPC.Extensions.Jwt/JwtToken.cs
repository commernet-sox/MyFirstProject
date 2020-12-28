namespace CPC.Extensions
{
    public class JwtToken
    {
        public string Token { get; set; }

        public int Expires { get; set; }

        public string GrantType { get; set; }

        public string Scope { get; set; }
    }

    public class JwtRefreshToken : JwtToken
    {
        public string RefreshToken { get; set; }
    }
}
