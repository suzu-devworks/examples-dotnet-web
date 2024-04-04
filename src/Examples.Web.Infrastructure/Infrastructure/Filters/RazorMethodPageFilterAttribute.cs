using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Examples.Web.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RazorPageMethodFilterAttribute : Attribute, IAsyncPageFilter
    {
        public RazorPageMethodFilterAttribute(
            string[]? handlers = default
        )
        {
            HandlerMethodNames = handlers;
        }

        public IEnumerable<string>? HandlerMethodNames { get; }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (!IsHandled(context.HandlerMethod))
            {
                await next();
            }

            await OnPageHandlerExecutionAsyncCore(context, next);
            return;
        }

        protected virtual async Task OnPageHandlerExecutionAsyncCore(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await next();
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            if (!IsHandled(context.HandlerMethod))
            {
                return;
            }

            await OnPageHandlerSelectionAsyncCore(context);
            return;
        }

        protected virtual Task OnPageHandlerSelectionAsyncCore(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        private bool IsHandled(HandlerMethodDescriptor? handlerMethod)
        {
            if (HandlerMethodNames is null)
            {
                return true;
            }

            if (HandlerMethodNames.Any(x => x == handlerMethod?.Name))
            {
                return true;
            }

            return false;
        }

    }

}