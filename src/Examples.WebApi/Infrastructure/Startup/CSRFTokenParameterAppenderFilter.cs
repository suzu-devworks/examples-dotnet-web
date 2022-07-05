using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.WebApi.Infrastructure.Startup;

public class CSRFTokenParameterAppenderFilter : IOperationFilter
{
    private readonly string _name;
    private readonly ParameterLocation _location;

    public CSRFTokenParameterAppenderFilter(string name, ParameterLocation location = ParameterLocation.Header)
    {
        _name = name;
        _location = location;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!IsVerificationRequired(context))
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();
        if (operation.Parameters.Any(x => x.Name == _name))
        {
            return;
        }

        operation.Parameters.Add(
            new()
            {
                Name = _name,
                In = _location,
                Required = false,
                Schema = new OpenApiSchema { Type = "String" },
            });

        return;
    }

    private static bool IsVerificationRequired(OperationFilterContext context)
    {
        var filter = GetEnabledAntiForgeryTokenFilter(context.ApiDescription);

        var required = filter switch
        {
            ValidateAntiForgeryTokenAttribute _ => true,
            AutoValidateAntiforgeryTokenAttribute _ =>
                IsHttpMethodVerificationRequired(context.ApiDescription.HttpMethod ?? ""),
            _ => false,
        };

        return required;
    }

    private static IFilterMetadata? GetEnabledAntiForgeryTokenFilter(ApiDescription api)
    {
        var descriptor = api.ActionDescriptor.FilterDescriptors
            .LastOrDefault(x => (x.Filter is AutoValidateAntiforgeryTokenAttribute
                                    or ValidateAntiForgeryTokenAttribute
                                    or IgnoreAntiforgeryTokenAttribute));

        return descriptor?.Filter;
    }

    private static bool IsHttpMethodVerificationRequired(string httpMethod)
    {
        var method = httpMethod;

        var notRequired = false;
        notRequired |= HttpMethods.IsGet(method);
        notRequired |= HttpMethods.IsHead(method);
        notRequired |= HttpMethods.IsOptions(method);
        notRequired |= HttpMethods.IsTrace(method);

        return !notRequired;
    }

}
