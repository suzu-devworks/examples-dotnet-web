using Examples.Web.Blazor.WebApp.Components;
using Examples.Web.Infrastructure.Containers;

var builder = WebApplication.CreateBuilder(args);

//# Add a configuration provider to read secrets from /run/secrets.
builder.Configuration.AddContainerSecrets();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//# Add Forwarded Headers options.
builder.Services.AddProxyForwardedHeaders();

var app = builder.Build();

//# Enable Forwarded Headers Middleware.
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
