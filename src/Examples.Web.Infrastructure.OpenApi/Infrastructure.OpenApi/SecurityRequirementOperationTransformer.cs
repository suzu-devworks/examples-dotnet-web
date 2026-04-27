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
        var hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any();
        if (hasAllowAnonymous)
        {
            return Task.CompletedTask;
        }

        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(referenceId, context.Document)] = []
        });

        operation.Responses ??= [];
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

        return Task.CompletedTask;
    }
}
