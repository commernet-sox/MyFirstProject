using CPC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Core
{
    public class HeadersHandler : IAuthenticationHandler
    {
        public const string SchemeName = "Headers";

        private HttpContext _context;
        private AuthenticationScheme _scheme;

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var keys = new[] { "CfgId" };
            foreach (var key in keys)
            {
                var val = string.Empty;
                if (_context.Request.Headers.ContainsKey(key))
                {
                    val = _context.Request.Headers[key].ToString();
                }

                if (val.IsNull())
                {
                    return Task.FromResult(AuthenticateResult.Fail($"{key} Headers参数未提供"));
                }
            }

            var claimsIdentity = new ClaimsIdentity(new Claim[]{
                new Claim("UserId", "headers_UId")}, SchemeName);

            var principal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, _scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;
            return Task.CompletedTask;
        }
    }
}
