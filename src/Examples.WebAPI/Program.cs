using Examples.Web.Infrastructure;
using Examples.WebAPI.Infrastructure;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Trace("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //# NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //# Configure Custom Kestrel options.
    builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false);

    // Add services to the container.

    //# Set URLs Lower Case.
    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    //# Set JsonSerializerOptions.
    builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.AddCustomOptions());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    //# Configure Custom Swagger options.
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddDefaultOptions();
        options.AddXmlComments();
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Examples.WebAPI",
            Version = "v1",
            Description = "&#127861; ASP.NET Core Web API examples.",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://github.com/suzu-devworks/examples-dotnet-web/blob/main/LICENSE")
            }
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        //# Add Swagger UI options.
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Examples.WebAPI v1");
            options.SwaggerEndpoint("v1/swagger.yaml", "Examples.WebAPI v1(yaml)");
            // shrink all.
            options.DefaultModelsExpandDepth(0);
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    //# Use Middleware.
    app.UseSecurityHttpResponseHeader();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    //# NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    //# Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
