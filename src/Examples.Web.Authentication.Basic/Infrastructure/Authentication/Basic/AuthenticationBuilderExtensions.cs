using AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;

namespace Examples.Web.Authentication.Basic;

/// <summary>
/// Extension methods for <see cref="AuthenticationBuilder" />.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds Basic authentication using <see cref="Microsoft.AspNetCore.Authentication" /> to the <see cref="AuthenticationBuilder" />.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddBasicWithAspNetCore(this AuthenticationBuilder builder,
        Action<BasicAuthenticationOption>? configure = null)
    {
        var option = new BasicAuthenticationOption();
        configure?.Invoke(option);

        var services = builder.Services;
        // use fake for _LoginPartial.cshtml.
        // services.AddSingleton<SignInManager<IdentityUser>, FakeSignInManager<IdentityUser>>();
        // services.AddSingleton<UserManager<IdentityUser>, FakeUserManager<IdentityUser>>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

        builder.AddBasic<BasicUserValidationService>(basic =>
           {
               basic.Realm = option.Realm ?? "Access to sites that require authentication";
               basic.Events = new BasicEvents
               {
                   OnAuthenticationSucceeded = context => Task.CompletedTask
               };
           });

        return builder;
    }

}