using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

public class AntiforgeryTokenParameterOperationFilter : IOperationFilter
{
    private readonly string _name;
    private readonly ParameterLocation _location;

    public AntiforgeryTokenParameterOperationFilter(string name, ParameterLocation location = ParameterLocation.Header)
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
        var method = context.ApiDescription.HttpMethod ?? "";
        var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors
            .Where(x => x.Filter is AutoValidateAntiforgeryTokenAttribute
                                or ValidateAntiForgeryTokenAttribute
                                or IgnoreAntiforgeryTokenAttribute)
            .Select(x => x.Filter);

        if (!filters.Any())
        {
            return false;
        }

        if (filters.Any(filter => filter is IgnoreAntiforgeryTokenAttribute))
        {
            return false;
        }

        if (filters.Any(filter => filter is AutoValidateAntiforgeryTokenAttribute)
            && IsHttpMethodVerificationRequired(method))
        {
            return true;
        }

        if (filters.Any(filter => filter is ValidateAntiForgeryTokenAttribute))
        {
            return true;
        }

        return false;
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