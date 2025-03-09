using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequestHeaderParameterAttribute : Attribute
{
    public RequestHeaderParameterAttribute(string name, string? description = null, string? defaultValue = null)
    {
        Name = name;
        Description = description;
        Default = defaultValue;
    }

    public string Name { get; }
    public string? Description { get; }
    public string? Default { get; }
}

public class RequestHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var appendices = context.MethodInfo
            .GetCustomAttributes(inherit: true)
            .OfType<RequestHeaderParameterAttribute>()
            .Where(x => !operation.Parameters.Any(y => y.Name == x.Name))
            ;

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