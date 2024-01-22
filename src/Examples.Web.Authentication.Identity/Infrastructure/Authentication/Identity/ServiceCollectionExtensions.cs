using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Examples.Web.Authentication.Identity.Areas.Identity.Data;
using Examples.Web.Authentication.Identity.Services;

namespace Examples.Web.Infrastructure.Authentication.Identity;

/// <summary>
/// Extension methods for adding authentication services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Identity authentication services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services,
        Action<ConfigureOption> configure)
    {
        var configureOption = new ConfigureOption();
        configure(configureOption);

        services.AddDbContext<IdentityDataContext>(options => options
            .UseSqlite(configureOption.ConnectionString));

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDataContext>()
            .AddErrorDescriber<JapaneseErrorDescriber>()
            .AddApiEndpoints();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.ConfigureApplicationCookie(options =>
        {
            var onRedirectToLogin = options.Events.OnRedirectToLogin;
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Headers.Any(x => x.Key == HeaderNames.Accept && x.Value == "application/json"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                return onRedirectToLogin.Invoke(context);
            };

            var onRedirectToAccessDenied = options.Events.OnRedirectToAccessDenied;
            options.Events.OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Headers.Any(x => x.Key == HeaderNames.Accept && x.Value == "application/json"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }

                return onRedirectToAccessDenied.Invoke(context);
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // // Default Password settings.
            // options.Password.RequireDigit = true;
            // options.Password.RequireLowercase = true;
            // options.Password.RequireNonAlphanumeric = true;
            // options.Password.RequireUppercase = true;
            // options.Password.RequiredLength = 6;
            // options.Password.RequiredUniqueChars = 1;

            // Weak password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 2;

            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // Default User settings.
            options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;

        });

        services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.Cookie.Name = "YourAppCookieName";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/Identity/Account/Login";
            // ReturnUrlParameter requires Microsoft.AspNetCore.Authentication.Cookies;
            //using Microsoft.AspNetCore.Authentication.Cookies;

            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.SlidingExpiration = true;
        });

        services.AddTransient<IEmailSender, FakeEmailSender>();

        services.ConfigureApplicationCookie(o =>
        {
            o.ExpireTimeSpan = TimeSpan.FromDays(5);
            o.SlidingExpiration = true;
        });

        services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));

        return services;
    }

    private sealed class CompositeIdentityHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
           : SignInAuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var bearerResult = await Context.AuthenticateAsync(IdentityConstants.BearerScheme);

            // Only try to authenticate with the application cookie if there is no bearer token.
            if (!bearerResult.None)
            {
                return bearerResult;
            }

            // Cookie auth will return AuthenticateResult.NoResult() like bearer auth just did if there is no cookie.
            return await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }

    public class ConfigureOption
    {
        public string ConnectionString { get; set; } = default!;
    }
}


