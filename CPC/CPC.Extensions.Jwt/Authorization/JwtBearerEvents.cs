using System;
using System.Threading.Tasks;

namespace CPC.Extensions
{
    public class JwtBearerEvents
    {
        public Func<JwtMessageReceivedContext, Task> OnMessageReceived { get; set; } = context => Task.CompletedTask;

        public Func<JwtAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        public Func<JwtTokenValidatedContext, Task> OnTokenValidated { get; set; } = context => Task.CompletedTask;
    }
}
