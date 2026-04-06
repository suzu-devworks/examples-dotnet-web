using Examples.Web.Authentication.Cookie;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//# Add support for controllers (for AccountController).
builder.Services.AddControllersWithViews();

//# Add Cookie Authentication.
builder.Services.AddAuthentication(CookieDefaults.AuthenticationScheme)
    .AddCustomCookie(options => builder.Configuration.GetSection("Authentication").Bind(options));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

//# Add Cookie Policy Middleware with Strict SameSite policy.
app.UseCookiePolicy(new CookiePolicyOptions
{
    //# OAuth2 and other cross-origin authentication methods will no longer work.
    MinimumSameSitePolicy = SameSiteMode.Strict,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

//# Add default controller route for AccountController.
app.MapDefaultControllerRoute();

app.Run();
