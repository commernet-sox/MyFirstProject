using CPC;
using CPC.Service;
using Gateway.Extensions;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Middlerware
{
    public class JwtMiddleware: OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtManager _jwtManager;

        public JwtMiddleware(RequestDelegate next,
            IOcelotLoggerFactory loggerFactory, JwtManager jwtManager)
            : base(loggerFactory.CreateLogger<JwtMiddleware>())
        {
            _next = next;
            _jwtManager = jwtManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToUpper() != "OPTIONS")
            {
                var request = context.Request;
                var url = request.Path.Value.ToLowerInvariant();
                if ((url.StartsWith("/os/1.0/oauth2") && request.Method.ToLowerInvariant() == "get") || (url.EndsWith("doc") && request.Method.ToLowerInvariant() == "get"))
                {
                    await _next.Invoke(context);
                    return;
                }

                //var uri = new Uri(request.Path.Value.ToLowerInvariant());
                //var abPath = uri.AbsolutePath;
                //var method = request.Method.ToLowerInvariant();

                var token = request.Headers.GetJwtToken();

                var oc = _jwtManager.ValidateToken(token, null, out var principal);
                if (oc.Code == ApiCode.Success)
                {
                    context.User = principal;
                    var downstreamRequest = context.Items.DownstreamRequest();

                    foreach (var claim in oc.Data)
                    {
                        downstreamRequest.Headers.Add(claim.Type, claim.Value);
                    }
                    await _next.Invoke(context);
                }
                else
                {
                    var error = new UnauthenticatedError(
                        $"Request for authenticated route {context.Request.Path} was unauthenticated");
                    Logger.LogWarning($"Client has NOT been authenticated for {context.Request.Path} and pipeline error set. {error}");
                    context.Items.SetError(error);
                }
            }
            else
            {
                Logger.LogInformation($"No authentication needed for {context.Request.Path}");

                await _next.Invoke(context);
            }
        }
    }
}
