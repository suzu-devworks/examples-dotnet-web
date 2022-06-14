using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class AddHeaderResultFilterAttribute : ResultFilterAttribute
    {
        private readonly string name;
        private readonly string value;

        public AddHeaderResultFilterAttribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(name, new string[] { value });
            base.OnResultExecuting(context);
        }

    }
}