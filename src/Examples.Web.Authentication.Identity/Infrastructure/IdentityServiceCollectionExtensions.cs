using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Examples.Web.Authentication.Identity.Areas.Identity.Data;
using Examples.Web.Infrastructure.Authentication.Identity;
using Examples.Web.Authentication.Identity.Services;
using Microsoft.Net.Http.Headers;

namespace Examples.Web.Infrastructure;

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


    public class ConfigureOption
    {
        public string ConnectionString { get; set; } = default!;
    }
}


