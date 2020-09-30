using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.OcelotExtensions.Administration
{
    public static class JwtCustomNames
    {
        public const string GrantType = "grant_type";
        public const string Bearer = "Bearer";
        public const string Scope = "scope";
        public const string OpenIdConnect = "oidc";
        public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
    }
}
