using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Examples.Web.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var certCollection = CertificateLoader.LoadCertificate(
    builder.Configuration["Authentication:Certificate:CustomTrustStore"]);

builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        // This checks whether the CA is "trusted" during connection and is necessary to complete the TLS handshake.

        httpsOptions.ClientCertificateMode =
            Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;

        httpsOptions.OnAuthenticate = (context, sslOptions) =>
        {
            sslOptions.CertificateChainPolicy = new X509ChainPolicy
            {
                TrustMode = X509ChainTrustMode.CustomRootTrust,
                RevocationMode = X509RevocationMode.NoCheck
            };

            sslOptions.CertificateChainPolicy.CustomTrustStore.AddRange(certCollection);
        };
    });
});

builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        // The certificate is retrieved from the established connection and
        // recognized as an ASP.NET Core user (ClaimsPrincipal).

        options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
        options.CustomTrustStore.AddRange(certCollection);
        options.RevocationMode = X509RevocationMode.NoCheck;

        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                var validationService = context.HttpContext.RequestServices
                    .GetRequiredService<ICertificateValidationService>();

                if (!validationService.ValidateCertificate(context.ClientCertificate))
                {
                    // If it fails, a 403 Forbidden error will be returned
                    // when accessing a page with the Authorize attribute.
                    context.Fail("Invalid certificate");
                    return Task.CompletedTask;
                }

                // At this point, context.Principal is already set by default, but you can customize it as needed.
                // For example, you can add claims based on the certificate information.
                var claims = new[]
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.GetNameInfo(X509NameType.SimpleName, false),
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };
                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build());

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<ICertificateValidationService,
    Examples.Web.Authentication.Services.CertificateValidationService>();

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

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
