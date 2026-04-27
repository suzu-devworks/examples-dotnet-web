using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Examples.Web.Infrastructure.OpenApi;

/// <summary>
/// Adds a security requirement to operations that are not marked as AllowAnonymous.
/// The security scheme is configurable via the referenceId parameter (default: Bearer).
/// </summary>
public sealed class SecurityRequirementOperationTransformer(string referenceId = "Bearer") : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Access ApiDescription here via context.Description.
        var endpointMetadata = context.Description.ActionDescriptor.EndpointMetadata;
        var hasAllowAnonymous = endpointMetadata.OfType<IAllowAnonymous>().Any();
        if (hasAllowAnonymous)
        {
            return Task.CompletedTask;
        }

        var schemeNames = endpointMetadata
            .OfType<IAuthorizeData>()
            .SelectMany(static authorizeData =>
                (authorizeData.AuthenticationSchemes ?? string.Empty)
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (schemeNames.Length == 0)
        {
            schemeNames = [referenceId];
        }

        operation.Security ??= [];
        foreach (var schemeName in schemeNames)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(schemeName, context.Document)] = []
            });
        }

        operation.Responses ??= [];
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

        return Task.CompletedTask;
    }
}
