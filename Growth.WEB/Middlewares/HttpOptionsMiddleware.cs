using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Growth.WEB.Middlewares
{
    /// <summary>
    /// Middleware for handling http requests with OPTIONS method
    /// </summary>
    public class HttpOptionsMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Next middleware</param>
        public HttpOptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes middleware
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "OPTIONS")
            {
                return _next(context);
            }

            context.Response.Headers.Add("Cache-Control", "no-cache");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");

            return context.Response.WriteAsync("OK");
        }
    }
}
