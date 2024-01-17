using Microsoft.EntityFrameworkCore;
using Examples.Web.Infrastructure.Authentication.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentityAuthentication(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("IdentityDataContextConnection")
            ?? throw new InvalidOperationException("Connection string 'IdentityDataContextConnection' not found.");
    });

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
    })
    .AddGitHub(githubOptions =>
    {
        githubOptions.ClientId = builder.Configuration["Authentication:Github:ClientId"]!;
        githubOptions.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"]!;
        // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
        githubOptions.Scope.Add("user:email");
    });


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//# Don't you use it? app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
