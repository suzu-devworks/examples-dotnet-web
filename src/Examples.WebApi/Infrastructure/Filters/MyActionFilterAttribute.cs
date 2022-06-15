using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        private readonly PositionOptions _settings;

        public MyActionFilterAttribute(IOptions<PositionOptions> options)
        {
            _settings = options.Value;
            this.Order = 1;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_settings.Title ?? "", new string[] { _settings.Name ?? "" });
            base.OnResultExecuting(context);
        }
    }
}
