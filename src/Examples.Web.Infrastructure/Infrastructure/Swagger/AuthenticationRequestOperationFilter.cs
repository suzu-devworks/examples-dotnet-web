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
        if (!RequireAuthorized(context))
        {
            return;
        }

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
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

        return;
    }

    private static bool RequireAuthorized(OperationFilterContext context)
    {
        var authorizes = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>();
        var anonymousAttrs = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>();

        if (anonymousAttrs.Any())
        {
            return false;
        }

        if (!authorizes.Any())
        {
            return false;
        }

        // var attributes = anonymousAttrs.OfType<AuthorizeAttribute>();

        return true;
    }
}
