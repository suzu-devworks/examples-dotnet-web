using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ShortCircuitingResourceFilterAttribute(
        string[]? handlers = default
        ) : Attribute, IResourceFilter
    {
        public IEnumerable<string>? HandlerMethodNames { get; } = handlers;

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!IsHandled(context.HttpContext))
            {
                return;
            }

            // be short-circuited.
            context.Result = new ContentResult()
            {
                Content = "Resource unavailable - header not set."
            };
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        private bool IsHandled(HttpContext httpContext)
        {
            if (HandlerMethodNames is null)
            {
                return true;
            }

            var handler = httpContext.Request.Query["handler"];
            if (HandlerMethodNames.Any(x => x == handler))
            {
                return true;
            }

            return false;
        }
    }
}