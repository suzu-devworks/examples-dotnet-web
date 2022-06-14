using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        private readonly PositionOptions settings;

        public MyActionFilterAttribute(IOptions<PositionOptions> options)
        {
            settings = options.Value;
            this.Order = 1;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(settings.Title ?? "", new string[] { settings.Name ?? "" });
            base.OnResultExecuting(context);
        }
    }
}
