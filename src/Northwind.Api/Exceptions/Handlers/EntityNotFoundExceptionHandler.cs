using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Northwind.Core.Exceptions;

namespace Northwind.Api.Exceptions.Handlers
{
    public class EntityNotFoundExceptionHandler : IExceptionHandler
    {
        public async Task<bool> HandleExceptionAsync(Exception exception, HttpContext context)
        {
            if (!(exception is EntityNotFoundException))
            {
                return false;
            }

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(exception.Message);

            return true;
        }
    }
}
