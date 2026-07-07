using Microsoft.AspNetCore.Http;

namespace HR_AUTOMATION.Infrastructure.Middlewares
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        public Middleware(RequestDelegate next)
        {
            _next = next;
            
        }
        public async Task InvokeAsync(HttpContext context) {
            var task = context.Request.Headers;
            await _next(context);
        }
    }
}
