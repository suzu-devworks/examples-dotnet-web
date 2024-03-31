using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebUI.Infrastructure.Filters;

public class ResponseHeaderAttribute(string name, string value) : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append(name, value);

        base.OnResultExecuting(context);
    }
}
