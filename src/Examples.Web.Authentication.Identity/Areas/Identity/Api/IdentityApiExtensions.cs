using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing;

public static class EndpointConventionBuilderExtensions
{
    public static RouteHandlerBuilder MapIdentityLogoutApi(this IEndpointRouteBuilder app)
    {
        return app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
            [FromBody] object empty) =>
        {
            if (empty != null)
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }
            return Results.Unauthorized();
        })
        .WithOpenApi()
        .RequireAuthorization();

    }
}


