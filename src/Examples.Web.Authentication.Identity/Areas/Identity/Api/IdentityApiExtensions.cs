using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing;

public static class EndpointConventionBuilderExtensions
{
    public static RouteHandlerBuilder MapIdentityLogoutApi(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/logout", async (
            SignInManager<IdentityUser> signInManager,
            [FromBody] object empty) =>
        {
            // The request checks for an empty body to prevent CSRF attacks. By requiring something
            // in the body, the request must be made from JavaScript, which is the only way to
            // access the cookie. It can't be accessed by a form-based post.
            if (empty == null)
            {
                // 'User-controlled bypass of sensitive method.'
                // return Results.Unauthorized();
                return Results.BadRequest();
            }

            await signInManager.SignOutAsync();
            return Results.Ok();
        })
        .WithOpenApi()
        .RequireAuthorization();

    }
}