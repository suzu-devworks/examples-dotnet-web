using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Examples.Web.Infrastructure.OpenApi;

/// <summary>
/// Adds a security scheme to the OpenAPI document.
/// The security scheme is configurable via a factory function (default: Bearer).
/// </summary>
/// <example>
/// Program.cs usage:
/// <code><![CDATA[
/// builder.Services.AddOpenApi(options =>
/// {
///     // Default: Bearer scheme.
///     options.AddDocumentTransformer<SecurityRequirementDocumentTransformer>();
/// });
/// ]]></code>
/// <code><![CDATA[
/// builder.Services.AddOpenApi(options =>
/// {
///     // Custom: API key scheme.
///     options.AddDocumentTransformer(new SecurityRequirementDocumentTransformer(() => new OpenApiSecurityScheme
///     {
///         Type = SecuritySchemeType.ApiKey,
///         Name = "X-API-Key",
///         In = ParameterLocation.Header,
///         Scheme = "ApiKey",
///         Description = "Enter API key"
///     }));
/// });
/// ]]></code>
/// </example>
public class SecurityRequirementDocumentTransformer : IOpenApiDocumentTransformer
{
    private readonly Func<OpenApiSecurityScheme> _schemeFactory;

    public SecurityRequirementDocumentTransformer(Func<OpenApiSecurityScheme>? schemeFactory = null)
    {
        _schemeFactory = schemeFactory ?? DefaultBearerScheme;
    }

    private static OpenApiSecurityScheme DefaultBearerScheme()
    {
        return new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Enter JWT token (e.g 'eyJhbG... ')"
        };
    }

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var scheme = _schemeFactory();
        var schemeKey = scheme.Scheme ?? "Bearer";

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add(schemeKey, scheme);

        return Task.CompletedTask;
    }
}
