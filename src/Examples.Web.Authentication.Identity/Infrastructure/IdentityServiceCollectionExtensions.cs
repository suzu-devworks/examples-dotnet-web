using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Examples.Web.Authentication.Identity.Areas.Identity.Data;

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
            .AddEntityFrameworkStores<IdentityDataContext>();

        return services;
    }


    public class ConfigureOption
    {
        public string ConnectionString { get; set; } = default!;
    }
}


