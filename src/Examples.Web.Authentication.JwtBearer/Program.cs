using Examples.Web.Authentication;
using Examples.Web.Authentication.JwtBearer;
using Examples.Web.Authentication.Oidc;
using Examples.Web.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Add Bearer security scheme to the document (enables the "Authorize" button in SwaggerUI).
    options.AddDocumentTransformer<SecurityRequirementDocumentTransformer>();
    // Add a security requirement for the Bearer scheme to all operations.
    options.AddOperationTransformer<SecurityRequirementOperationTransformer>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.Authority = "https://{--your-authority--}";
        jwtOptions.Audience = "https://{--your-audience--}";
    })
    .AddJwtBearer("some-scheme", jwtOptions =>
    {
        jwtOptions.MetadataAddress = builder.Configuration["Api:MetadataAddress"]!;
        // Optional if the MetadataAddress is specified
        jwtOptions.Authority = builder.Configuration["Api:Authority"];
        jwtOptions.Audience = builder.Configuration["Api:Audience"];
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidAudiences = builder.Configuration.GetSection("Api:ValidAudiences").Get<string[]>(),
            ValidIssuers = builder.Configuration.GetSection("Api:ValidIssuers").Get<string[]>()
        };

        jwtOptions.MapInboundClaims = false;
    })
    .AddJwtBearer("Auth0", jwtOptions =>
    {
        jwtOptions.Authority = builder.Configuration["Authentication:Schemes:Auth0:Authority"];
        jwtOptions.Audience = builder.Configuration["Authentication:Schemes:Auth0:Audience"];
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
            RoleClaimType = "https://my-app.example.com/roles",
        };
    })
    .AddJwtBearer("FakeOidc")
    .AddRevocableJwtBearer("RevocableJwt")
    .AddPolicyScheme("Aggregate", "Aggregate Policy", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            // Logic to select the appropriate scheme based on the request
            if (context.Request.Path.StartsWithSegments("/hello-fake-oidc"))
            {
                return "FakeOidc";
            }

            if (context.Request.Path.StartsWithSegments("/hello-revocable-jwt"))
            {
                return "RevocableJwt";
            }

            return JwtBearerDefaults.AuthenticationScheme; // Default scheme
        };
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<JwtBearerOptions>("FakeOidc", options =>
    {
        // This is important: Pass SSL verification in internal communications.
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    });

    builder.Services.ConfigureAll<JwtBearerOptions>(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // You can add custom claims transformation or additional validation here if needed
                context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearerEvents")
                    .LogInformation("Token validated successfully for {User}", context.HttpContext.User.Identity?.Name);
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Log the exception or handle it as needed
                context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearerEvents")
                    .LogError(context.Exception, "Authentication failed");
                return Task.CompletedTask;
            }
        };
    });
}

var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(requireAuthPolicy);

builder.Services.Configure<JwtBlacklistOptions>(
    builder.Configuration.GetSection("Authentication:JwtBlacklistOptions"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();

    app.MapScalarApiReference().AllowAnonymous();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.DefaultModelsExpandDepth(0);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

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
.WithName("GetWeatherForecast");

app.MapGet("/hello-auth0", [Authorize(AuthenticationSchemes = "Auth0")] (HttpContext context) =>
    Results.Ok(new
    {
        Message = $"Hello from Auth0 protected endpoint!",
        context.User.Identity?.IsAuthenticated
    }))
    .WithName("Auth0 Protected Endpoint");

app.MapGet("/hello-fake-oidc", [Authorize(AuthenticationSchemes = "Aggregate")] (HttpContext context) =>
    Results.Ok(new
    {
        Message = $"Hello from FakeOidc protected endpoint!",
        context.User.Identity?.IsAuthenticated
    }))
    .WithName("FakeOidc Protected Endpoint");

app.MapGet("/hello-revocable-jwt", [Authorize(AuthenticationSchemes = "Aggregate")] (HttpContext context) =>
    Results.Ok(new
    {
        Message = $"Hello from RevocableJwt protected endpoint!",
        context.User.Identity?.IsAuthenticated
    }))
    .WithName("RevocableJwt Protected Endpoint");

app.MapWellKnownEndpoints();

app.Run();

file record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
