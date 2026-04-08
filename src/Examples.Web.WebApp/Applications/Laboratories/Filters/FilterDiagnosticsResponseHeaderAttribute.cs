using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters;

/// <summary>
/// An <see cref="ResultFilterAttribute"/> that adds a response header
/// with filter execution timeline information for diagnostics purposes.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class FilterDiagnosticsResponseHeaderAttribute : ResultFilterAttribute
{
    public const string TimelineHeaderName = "X-Filter-Timeline";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var httpContext = context.HttpContext;
        FilterDiagnosticsTracker.Record(httpContext, nameof(FilterDiagnosticsResponseHeaderAttribute), nameof(OnResultExecuting));

        var timeline = FilterDiagnosticsTracker.GetTimelineHeaderValue(httpContext);
        if (!string.IsNullOrEmpty(timeline))
        {
            httpContext.Response.Headers[TimelineHeaderName] = timeline;
        }

        base.OnResultExecuting(context);
    }
}
