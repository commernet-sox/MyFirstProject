namespace SDT.Service
{
    public static class JwtGrantType
    {
        /// <summary>
        /// the Client Credentials grant type is used by clients to obtain an access token outside of the context of a user.
        /// this is typically used by clients to access resources about themselves rather than to access a user's resources.
        /// </summary>
        public const string ClientCredential = "client_credential";

        /// <summary>
        /// the Authorization Code grant type is used by confidential and public clients to exchange an authorization code for an access token.
        /// after the user returns to the client via the redirect URL, the application will get the authorization code from the URL and use it to request an access token.
        /// </summary>
        public const string AuthorizationCode = "authorization_code";

        /// <summary>
        /// the Refresh Token grant type is used by clients to exchange a refresh token for an access token when the access token has expired.
        /// </summary>
        public const string RefreshToken = "refresh_token";

        /// <summary>
        /// 
        /// </summary>
        public const string PasswordCredential = "password_credential";

        /// <summary>
        /// 
        /// </summary>
        public const string Implicit = "implicit";
    }
}
