using Examples.Web.Infrastructure;
using Examples.Web.Infrastructure.Containers;
using Examples.Web.WebApp.Applications.Environments;
using Examples.Web.WebApp.Applications.Laboratories.Filters;

var builder = WebApplication.CreateBuilder(args);

//# Add a configuration provider to read secrets from /run/secrets.
builder.Configuration.AddContainerSecrets();

//# Add application configuration.
builder.Configuration.AddApplicationConfiguration();

// Add services to the container.
builder.Services.AddRazorPages();

//# configure Filters for processing order demonstration.
builder.Services.AddProcessingOrderFilters();

//# Add Forwarded Headers options.
builder.Services.AddProxyForwardedHeaders();

var app = builder.Build();

//# Enable Forwarded Headers Middleware.
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
