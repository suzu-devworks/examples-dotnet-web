using Microsoft.OpenApi.Models;
using Examples.Web.Infrastructure;
using NLog;
using NLog.Web;
using Examples.Web.Infrastructure.Security;
using Examples.Web.WebAPI.Applications;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //# NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //# Kestrel: Security settings.
    builder.WebHost.ConfigureKestrel(serverOptions =>
        builder.Configuration.GetSection("Kestrel").Bind(serverOptions));

    //# Set lower case URLs.
    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    //# CORS.
    builder.Services.AddCors(options =>
        options.AddDefaultSpaPolicyFrom(builder.Configuration.GetSection("CorsPolicyOptions")));

    // Add services to the container.
    builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.UseCustomOptions());

    builder.Services.AddEndpointsApiExplorer();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //# Configure Custom Swagger options.
    //builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGen(options =>
    {
        options.UseAnnotationFilters();
        options.UseXmlComments();
        options.SwaggerDoc("v1", new()
        {
            Version = "v1",
            Title = "Examples.Web.WebAPI",
            Description = "&#127861; ASP.NET Core Web API examples.",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://github.com/suzu-devworks/examples-dotnet-web/blob/main/LICENSE")
            }
        });
        options.MapType<TimeSpan>(() => new() { Type = "string" });
    });

    //# Configure Custom Options.
    builder.Services.Configure<RequestLocalizationOptions>(options =>
        options.AddCustomCultures("ja", "ja-JP", "fr", "fr-CA", "en", "en-US"));

    //# Configure Custom Services.
    builder.Services.AddApplicationServices();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        //# Add Swagger UI options.
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Examples.Web.WebAPI v1");
            options.SwaggerEndpoint("v1/swagger.yaml", "Examples.Web.WebAPI v1(yaml)");
            //# Swagger UI at the app's root.
            //options.RoutePrefix = string.Empty;
            //# Schemas shrink all.
            options.DefaultModelsExpandDepth(0);
        });
    }

    app.UseHttpsRedirection();

    //# Setting order is important!
    //# PathBase: Configuration from appsettings.
    app.UsePathBase(app.Configuration.GetValue<string>("PathBase"));
    app.UseRouting();
    app.UseCors();

    //# Use Middleware.
    app.UseRequestLocalization();
    //# Response Header: Security settings.
    app.UseSecurityHttpResponseHeader();

    app.MapControllers();

    //# Use Home controller with Minimal API. 
    //# app.MapGet("/", () => Results.Redirect("~/swagger"));

    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

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


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
