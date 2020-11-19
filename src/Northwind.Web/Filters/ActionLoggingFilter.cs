using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Northwind.Web.Configuration;

namespace Northwind.Web.Filters
{
    public class ActionLoggingFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        private readonly ActionLoggingOptions _options;

        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger, IOptions<ActionLoggingOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var messageEnding = string.Empty;

            if (_options.AddArguments && context.ActionArguments.Any())
            {
                var actionParameters = string.Join(", ", context.ActionArguments.Keys);
                messageEnding = $"with parameters: {actionParameters}";
            }

            _logger.LogDebug($"Action '{context.ActionDescriptor.DisplayName}' invoked {messageEnding}");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogDebug($"Action '{context.ActionDescriptor.DisplayName}' execution finished");
        }
    }
}
