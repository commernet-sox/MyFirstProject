using CPC;
using CPC.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Extensions
{
    public static class CommonExtensions
    {
        public static string GetJwtToken(this IHeaderDictionary headers)
        {
            var token = string.Empty;
            var authHeader = headers["Authorization"];
            if (authHeader.IsNull())
            {
                return token;
            }

            var bearer = JwtCustomNames.Bearer + " ";
            foreach (var auth in authHeader)
            {
                if (auth.StartsWith(bearer))
                {
                    token = auth.Substring(bearer.Length);
                }
            }
            return token;
        }
    }
}
