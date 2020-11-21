using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Northwind.Api.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        Task<bool> HandleExceptionAsync(Exception exception, HttpContext context);
    }
}
