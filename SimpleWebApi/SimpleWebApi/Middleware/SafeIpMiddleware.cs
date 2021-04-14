using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleWebApi.Middleware
{
    /// <summary>
    /// 非法ip拦截中间件
    /// </summary>
    public class SafeIpMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _illegalIpList;
        public SafeIpMiddleware(RequestDelegate next, string IllegalIpList)
        {
            _illegalIpList = IllegalIpList;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "GET" || context.Request.Method == "POST")
            {
                var remoteIp = context.Connection.RemoteIpAddress;    //获取远程访问IP
                string[] ip = _illegalIpList.Split(';');
                var bytes = remoteIp.GetAddressBytes();
                var badIp = false;
                foreach (var address in ip)
                {
                    var testIp = IPAddress.Parse(address);
                    if (testIp.GetAddressBytes().SequenceEqual(bytes))
                    {
                        badIp = true;
                        break;    //直接跳出ForEach循环
                    }
                }
                if (badIp)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}
