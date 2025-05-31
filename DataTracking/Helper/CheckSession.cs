using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DataTracking.Helper
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckSession
    {
        private readonly RequestDelegate _next;

        public CheckSession(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue("uid", out var session);
            if (string.IsNullOrEmpty(session))
            {
                session = Guid.NewGuid().ToString();
                httpContext.Response.Cookies.Append("uid", session, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(30)
                });
                return _next(httpContext);
            }
            httpContext.Items["users"] = session;
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckSessionExtensions
    {
        public static IApplicationBuilder UseCheckSession(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckSession>();
        }
    }
}
