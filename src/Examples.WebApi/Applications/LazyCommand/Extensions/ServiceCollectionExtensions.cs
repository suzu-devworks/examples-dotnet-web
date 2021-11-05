using System;
using Microsoft.Extensions.DependencyInjection;
using Examples.WebApi.Applications.LazyCommand.Commands;
using Examples.WebApi.Applications.LazyCommand.Repositories;

namespace Examples.WebApi.Applications.LazyCommand.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLazyCommand(this IServiceCollection services)
        {
            services.AddScoped<IPlanetRepository, PlanetRepository>();
            services.AddScoped<IRocketRepository, RocketRepository>();

            services.AddScoped<IGetRocketCommand, GetRocketCommand>();
            services.AddScoped(x => new Lazy<IGetRocketCommand>(
                () => x.GetRequiredService<IGetRocketCommand>()));

            services.AddScoped<ILaunchRocketCommand, LaunchRocketCommand>();
            services.AddScoped(x => new Lazy<ILaunchRocketCommand>(
                () => x.GetRequiredService<ILaunchRocketCommand>()));

            return services;
        }

    }
}