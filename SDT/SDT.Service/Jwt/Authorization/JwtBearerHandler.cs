using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SDT.BaseTool;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SDT.Service
{
    public class JwtBearerHandler : AuthenticationHandler<JwtBearerOptions>
    {
        public JwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected new JwtBearerEvents Events
        {
            get => (JwtBearerEvents)base.Events;
            set => base.Events = value;
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new JwtBearerEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var messageReceivedContext = new JwtMessageReceivedContext(Context, Scheme, Options);

                // event can set the token
                await Events.OnMessageReceived?.Invoke(messageReceivedContext);
                if (messageReceivedContext.Result != null)
                {
                    return messageReceivedContext.Result;
                }

                var token = messageReceivedContext.Token;
                if (token.IsNull())
                {
                    string authorization = Request.Headers["Authorization"];

                    // If no authorization header found, nothing to process further
                    if (string.IsNullOrEmpty(authorization))
                    {
                        return AuthenticateResult.NoResult();
                    }

                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authorization.Substring("Bearer ".Length).Trim();
                    }

                    // if no token found, no further work possible
                    if (string.IsNullOrEmpty(token))
                    {
                        return AuthenticateResult.NoResult();
                    }
                }

                var jwt = new JwtManager(Options.JwtSettings);
                var oc = jwt.ValidateToken(token, Options.JwtValidation, out var principal);
                if (oc.Code == ApiCode.Success)
                {
                    var tokenValidatedContext = new JwtTokenValidatedContext(Context, Scheme, Options)
                    {
                        Principal = principal,
                        Token = token
                    };

                    await Events.OnTokenValidated?.Invoke(tokenValidatedContext);
                    if (tokenValidatedContext.Result != null)
                    {
                        return tokenValidatedContext.Result;
                    }

                    tokenValidatedContext.Success();
                    return tokenValidatedContext.Result;
                }

                var authenticationFailedContext = new JwtAuthenticationFailedContext(Context, Scheme, Options)
                {
                    Token = token,
                    Error = oc
                };

                await Events.OnAuthenticationFailed?.Invoke(authenticationFailedContext);
                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                return AuthenticateResult.Fail(authenticationFailedContext.Error.Message);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
