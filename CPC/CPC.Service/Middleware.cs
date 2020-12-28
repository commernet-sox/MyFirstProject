using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CPC.Service
{
    public abstract class Middleware
    {
        protected readonly RequestDelegate _next;

        protected Middleware(RequestDelegate next) => _next = next;

        public abstract Task Invoke(HttpContext context);
    }
}
