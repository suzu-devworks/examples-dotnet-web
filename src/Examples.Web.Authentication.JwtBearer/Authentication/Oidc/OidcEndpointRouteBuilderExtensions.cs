namespace Examples.Web.Authentication.JwtBearer.Authentication.Oidc;

public static class OidcEndpointRouteBuilderExtensions
{
    public static void MapWellKnownEndpoints(this IEndpointRouteBuilder app)
    {
        // By default, JwtBearerHandler starts collecting authentication credentials from this endpoint.
        app.MapGet("/.well-known/openid-configuration", async (HttpContext context) =>
        {
            var issuer = $"{context.Request.Scheme}://{context.Request.Host.Value}/";
            var metadata = new
            {
                issuer,
                jwks_uri = $"{issuer}.well-known/jwks.json",
            };

            return Results.Ok(metadata);
        })
        .AllowAnonymous()
        .ExcludeFromDescription();

        // This endpoint serves the JWKS document containing the public keys used to validate JWT tokens.
        app.MapGet("/.well-known/jwks.json", async (IHostEnvironment env) =>
        {
            var jwksPath = Path.Combine(env.ContentRootPath, "jwks.json");
            if (!File.Exists(jwksPath))
            {
                return Results.NotFound();
            }

            return Results.File(jwksPath, "application/json");
        })
        .AllowAnonymous()
        .ExcludeFromDescription();
    }
}
