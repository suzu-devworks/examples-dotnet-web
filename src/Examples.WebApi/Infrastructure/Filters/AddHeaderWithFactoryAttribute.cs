using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.WebApi.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AddHeaderWithFactoryAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new InternalAddHeaderFilter();
        }

        private class InternalAddHeaderFilter : IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                context.HttpContext.Response.Headers.Add(
                    "Internal", new string[] { "My header" });
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
            }
        }

        public bool IsReusable => false;
    }
}