namespace SDT.Service
{
    public static class JwtScopes
    {
        public const string Scope = "scope";

        /// <summary>
        /// informs the Authorization Server that the Client is making an OpenID Connect request. If the openid scope value is not present, the behavior is entirely unspecified.
        /// </summary>
        public const string OpenId = "openid";

        /// <summary>
        /// this scope value requests access to the End-User's default profile Claims, which are: name, family_name, given_name, middle_name, nickname, preferred_username,profile, picture, website, gender, birthdate, zoneinfo, locale, and updated_at.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// this scope value requests access to the email and email_verified Claims.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        ///this scope value requests access to the address Claim.
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// this scope value requests access to the phone_number and phone_number_verified Claims.
        /// </summary>
        public const string Phone = "phone";

        /// <summary>
        ///  this scope value MUST NOT be used with the OpenID Connect Implicit Client Implementer's
        ///  guide 1.0. See the OpenID Connect Basic Client Implementer's Guide 1.0 (http://openid.net/specs/openid-connect-implicit-1_0.html#OpenID.Basic) for its usage in that subset of OpenID Connect.
        /// </summary>
        public const string OfflineAccess = "offline_access";
    }
}
