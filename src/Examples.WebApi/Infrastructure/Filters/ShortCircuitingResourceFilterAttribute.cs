using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.WebApi.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShortCircuitingResourceFilterAttribute : Attribute, IResourceFilter, IOrderedFilter
    {
        public ShortCircuitingResourceFilterAttribute() : this(int.MinValue)
        {
        }

        public ShortCircuitingResourceFilterAttribute(int order)
        {
            this.Order = order;
        }

        public int Order {get;}

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.Result = new ContentResult()
            {
                Content = "Resource unavailable - header not set."
            };
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}