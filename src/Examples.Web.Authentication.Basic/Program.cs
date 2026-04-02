using System.Net;
using Examples.Web.Authentication.Basic;
using Microsoft.AspNetCore.HttpOverrides;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//# Add Basic Authentication.
builder.Services.AddAuthentication(defaultScheme: BasicDefaults.AuthenticationScheme)
     .AddBasicWithAspNetCore(option => builder.Configuration.GetSection("Authentication").Bind(option));

//# Add Forwarded Headers Middleware to process the X-Forwarded-For, X-Forwarded-Proto, and X-Forwarded-Prefix headers.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    //# Clear the default known networks and proxies to allow processing of forwarded headers from any source.
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
    //# Allows the all range of docker bridge network (172.16.0.0/12) to be trusted for forwarded headers.
    options.KnownIPNetworks.Add(new(IPAddress.Parse("172.16.0.0"), 12));

    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedPrefix;
});

var app = builder.Build();

//# Enable Forwarded Headers Middleware to process the X-Forwarded-For, X-Forwarded-Proto, and X-Forwarded-Prefix headers.
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
