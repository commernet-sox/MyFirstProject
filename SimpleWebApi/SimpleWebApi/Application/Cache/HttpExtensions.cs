using CPC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Cache
{
    public static class HttpExtensions
    {
        public static string GetHeaderValue(this HttpContext context, string name)
        {
            if (name.IsNull())
            {
                return string.Empty;
            }

            return context.Request.Headers[name].ConvertString();
        }

        public static string GetClientId(this HttpContext context) => context.GetHeaderValue(IdentityConsts.ClientId);

        public static string GetUserId(this HttpContext context) => context.GetHeaderValue(IdentityConsts.UserId);

        public static string GetAppClass(this HttpContext context) => context.GetHeaderValue(IdentityConsts.AppClass);

        public static string GetScope(this HttpContext context) => context.GetHeaderValue(IdentityConsts.Scope);
    }
}
