using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Northwind.Api.Exceptions.Handlers;

namespace Northwind.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

        public ExceptionHandlerMiddleware(RequestDelegate next, IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _next = next;
            _exceptionHandlers = exceptionHandlers;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                foreach (var handler in _exceptionHandlers)
                {
                    if (await handler.HandleExceptionAsync(e, context))
                    {
                        return;
                    }
                }

                throw;
            }
        }
    }
}
