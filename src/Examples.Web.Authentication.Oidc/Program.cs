using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    builder.Configuration.GetSection("OpenIDConnectSettings").Bind(options);

    // Replace the above code with the following
    // var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");
    // options.Authority = oidcConfig["Authority"];
    // options.ClientId = oidcConfig["ClientId"];
    // options.ClientSecret = oidcConfig["ClientSecret"];

    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ResponseType = OpenIdConnectResponseType.Code;

    // It is recommended to add it explicitly.
    options.Scope.Clear();
    options.Scope.Add("openid");           // required
    options.Scope.Add("profile");          // default
    options.Scope.Add("email");            // Email address, email verification flag
    options.Scope.Add("offline_access");   // Obtain a refresh token to prolong your login status.

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.MapInboundClaims = false;
    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
    options.TokenValidationParameters.RoleClaimType = "roles";

    // > To log a user out of Auth0, you need to redirect the user's browser
    // > to the Auth0 logout endpoint (https://{yourDomain}/v2/logout).

    // It seems that this statement existed in the past, and web code
    // and generated AI may attempt to add custom code during logout,
    // but this is no longer mentioned in the current official documentation.
    // https://auth0.com/docs/authenticate/login/logout

    // Auth0's specifications have changed, and it now works correctly with the standard settings.

    // options.Events = new OpenIdConnectEvents
    // {
    //     OnRedirectToIdentityProviderForSignOut = (context) =>
    //     {
    //         var logoutUri = $"{options.Authority}/v2/logout?client_id={options.ClientId}";

    //         var postLogoutUri = context.Properties.RedirectUri;
    //         if (!string.IsNullOrEmpty(postLogoutUri))
    //         {
    //             if (postLogoutUri.StartsWith('/'))
    //             {
    //                 var request = context.Request;
    //                 postLogoutUri = $"{request.Scheme}://{request.Host}{request.PathBase}{postLogoutUri}";
    //             }

    //             logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
    //         }

    //         context.Response.Redirect(logoutUri);
    //         context.HandleResponse();

    //         return Task.CompletedTask;
    //     }
    // };
});

var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(requireAuthPolicy);

// Add services to the container.
builder.Services.AddRazorPages();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets()
    .AllowAnonymous();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
