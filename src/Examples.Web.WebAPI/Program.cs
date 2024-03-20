using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Examples.Web.Infrastructure;
using Examples.Web.Infrastructure.Routing;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //# Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //# Kestrel: Security settings.
    builder.WebHost.ConfigureKestrel(options =>
        builder.Configuration.GetSection("Kestrel").Bind(options));

    builder.Services.AddControllers(options =>
        {
            //# Set kebab-case URLs.
            options.Conventions.Add(new RouteTokenTransformerConvention(
                new SlugifyParameterTransformer()));
        })
        //# Set JSON custom serializer options.
        .AddJsonOptions(options => options.JsonSerializerOptions.UseCustomOptions());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    //# Setting order is important!
    app.UsePathBase(app.Configuration.GetValue("PathBase", ""));

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection(app.Configuration.GetValue("UseHttpsRedirection", true));

    app.UseRouting();
    app.UseAuthorization();

    //# Response Header: Security settings.
    app.UseSecurityHttpResponseHeader();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
