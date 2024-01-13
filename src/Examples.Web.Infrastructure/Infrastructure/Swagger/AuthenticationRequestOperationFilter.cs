using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

public class AuthenticationRequestOperationFilter : IOperationFilter
{
    private readonly string _name;
    private readonly string[] _scopes;

    public AuthenticationRequestOperationFilter(string name, IEnumerable<string>? scopes = null)
    {
        _name = name;
        _scopes = scopes?.ToArray() ?? Array.Empty<string>();
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.ApiDescription.CustomAttributes();
        var anonymous = attributes.Any(attr => attr is AllowAnonymousAttribute);
        if (anonymous) { return; }

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = _name
                    }
                },
                _scopes
            }
        });
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });

        return;
    }
}
