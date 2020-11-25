using CPC.Http;

namespace AMS.WebCore
{
    public class AppSettingsUtil
    {
        //public static ApiClientSettings Settings { get; set; }

        public static string GatewayUrl { get; set; }
        public static string AppKey { get; set; }
        public static string AppSecret { get; set; }
        public static string AuthUrl { get; set; }

        public static ApiClientSettings GetApiClientSettings(string userId,string userPwd)
        {
            return new ApiClientSettings
            {
                ClientId = AppKey,
                ClientSecret = AppSecret,
                UserName = userId,
                Password = userPwd,
                GrantType = "password_credential",
                OAuth2Address = AuthUrl,
                GatewayUrl=GatewayUrl
            };
        }
    }
}
