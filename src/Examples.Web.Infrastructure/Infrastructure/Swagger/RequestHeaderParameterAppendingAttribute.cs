using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequestHeaderParameterAppendingAttribute : Attribute
{
    public RequestHeaderParameterAppendingAttribute(string name, string? description = null, string? defaultValue = null)
    {
        Name = name;
        Description = description;
        Default = defaultValue;
    }

    public string Name { get; }
    public string? Description { get; }
    public string? Default { get; }
}

public class RequestHeaderParameterAppendingOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null || context is null)
        {
            return;
        }

        if (!context.ApiDescription.TryGetMethodInfo(out var method))
        {
            return;
        }

        var attributes = method.GetCustomAttributes<RequestHeaderParameterAppendingAttribute>();
        var appendices = attributes.Where(x => operation.Parameters.Any(x => x.Name == x.Name));

        operation.Parameters ??= new List<OpenApiParameter>();
        foreach (var attr in appendices)
        {
            operation.Parameters.Add(
                new()
                {
                    Name = attr.Name,
                    In = ParameterLocation.Header,
                    Description = attr.Description,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString(attr.Default)
                    }
                });
        }

        return;
    }

}
