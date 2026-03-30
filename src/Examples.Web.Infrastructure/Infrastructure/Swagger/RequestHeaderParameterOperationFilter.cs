using System;
using System.Linq;
using Microsoft.OpenApi;
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
            .Where(x => !operation.Parameters?.Any(y => y.Name == x.Name) ?? false)
            ;

        foreach (var attr in appendices)
        {
            operation.Parameters?.Add(
                new OpenApiParameter()
                {
                    Name = attr.Name,
                    In = ParameterLocation.Header,
                    Description = attr.Description,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String,
                        Default = attr.Default ?? string.Empty
                    }
                });
        }

        return;
    }

}