using Examples.Web.Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Examples.Web.Authentication.JwtBearer;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddRevocableJwtBearer(this AuthenticationBuilder builder,
        string authenticationScheme, Action<RevocableJwtOptions>? configureOptions = null)
    {
        builder.Services.TryAddSingleton<ITokenBlacklistService, ConfigurationTokenBlacklistService>();
        return builder.AddScheme<RevocableJwtOptions, RevocableJwtHandler>(authenticationScheme, options =>
        {
            configureOptions?.Invoke(options);
        });
    }
}
