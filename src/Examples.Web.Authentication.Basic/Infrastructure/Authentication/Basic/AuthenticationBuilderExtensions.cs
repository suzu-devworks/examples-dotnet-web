using AspNetCore.Authentication.Basic;
using Examples.Web.Domain.Identity;
using Examples.Web.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;

namespace Examples.Web.Infrastructure.Authentication.Basic;

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
    public static AuthenticationBuilder AddCustomBasic(this AuthenticationBuilder builder,
        Action<BasicAuthenticationOption>? configure = null)
    {
        var option = new BasicAuthenticationOption();
        configure?.Invoke(option);

        builder.AddBasic<BasicUserValidationService>(BasicDefaults.AuthenticationScheme,
            basic =>
            {
                basic.Realm = option.Realm ?? "Access to sites that require authentication";
                basic.Events = new BasicEvents
                {
                    OnAuthenticationSucceeded = context => Task.CompletedTask
                };
            });

        builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

        return builder;
    }
}
