using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Northwind.Web.Middleware
{
    public class ResponseBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalStream = context.Response.Body;

            var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await _next(context);

            buffer.Seek(0, SeekOrigin.Begin);
            await buffer.CopyToAsync(originalStream);
        }
    }
}
