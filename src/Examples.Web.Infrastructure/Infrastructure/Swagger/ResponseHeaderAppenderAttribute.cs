using System.Net;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ResponseHeaderAppenderAttribute : Attribute
{
    public ResponseHeaderAppenderAttribute(HttpStatusCode statusCodes, string name, string type, string? description)
        : this(statusCodes, name, type, null, description)
    {
    }

    public ResponseHeaderAppenderAttribute(HttpStatusCode statusCode, string name, string type, string? format, string? description)
    {
        StatusCode = statusCode;
        Name = name;
        Type = type;
        Format = format;
        Description = description;
    }

    public HttpStatusCode StatusCode { get; }
    public string Name { get; }
    public string Type { get; }
    public string? Format { get; }
    public string? Description { get; }

}

public class ResponseHeaderAppenderOperationFilter : IOperationFilter
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

        var attributes = method.GetCustomAttributes<ResponseHeaderAppenderAttribute>();

        foreach (var attr in attributes)
        {
            var response = operation.Responses
                .FirstOrDefault(r => r.Key == ((int)attr.StatusCode).ToString())
                .Value;

            if (response is not null)
            {
                response.Headers ??= new Dictionary<string, OpenApiHeader>();
                response.Headers.Add(attr.Name, new()
                {
                    Schema = new() { Type = attr.Type, Format = attr.Format },
                    Description = attr.Description,
                });
            }
        }

        return;
    }

}
