using Examples.Web.Infrastructure.Authentication.Basic;
using Examples.Web.Infrastructure.Containers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//# Add Basic Authentication.
builder.Services.AddAuthentication(defaultScheme: BasicAuthentication.DefaultScheme)
     .AddCustomBasic(option => builder.Configuration.GetSection("Authentication").Bind(option));

builder.Services.AddControllers();

//# Add Forwarded Headers options.
builder.Services.AddContainerForwardedHeaders();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
