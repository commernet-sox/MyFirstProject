using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;

namespace CPC.Extensions
{
    public class JwtBearerOptions : AuthenticationSchemeOptions
    {
        public new JwtBearerEvents Events
        {
            get => (JwtBearerEvents)base.Events;
            set => base.Events = value;
        }

        public JwtSettings JwtSettings { get; set; }

        public Action<TokenValidationParameters> JwtValidation { get; set; }
    }
}
