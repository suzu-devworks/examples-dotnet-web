using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters;

/// <summary>
/// An <see cref="IResourceFilter"/> that demonstrates short-circuiting behavior by setting
/// a result without calling the next delegate in the execution pipeline, effectively preventing the resource from being executed.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ShortCircuitingResourceFilterAttribute(
    string[]? handlers = default
    ) : Attribute, IResourceFilter
{
    public IEnumerable<string>? HandlerMethodNames { get; } = handlers;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingResourceFilterAttribute), nameof(OnResourceExecuting));
        if (!IsHandled(context.HttpContext))
        {
            FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingResourceFilterAttribute), "NotHandled");
            return;
        }

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingResourceFilterAttribute), "ShortCircuited");
        // be short-circuited.
        context.Result = new ContentResult()
        {
            Content = "Resource unavailable - header not set."
        };
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingResourceFilterAttribute), nameof(OnResourceExecuted));
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
