using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Authentication.JwtBearer;

public class RevocableJwtHandler(
    IOptionsMonitor<RevocableJwtOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration,
    ITokenBlacklistService blacklistService
) : AuthenticationHandler<RevocableJwtOptions>(options, logger, encoder)
{
    private readonly JsonWebTokenHandler _handler = new();

    private readonly ITokenBlacklistService _blacklistService = blacklistService;
    private string _customErrorMessage = "Unauthorized";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string authorization = Request.Headers.Authorization.ToString();

        // If no authorization header found, nothing to process further
        if (string.IsNullOrEmpty(authorization))
        {
            return AuthenticateResult.NoResult();
        }

        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authorization.Substring("Bearer ".Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.NoResult();
        }

        try
        {
            // Get the key from the settings.
            // In a real application, you would likely want to cache the signing key and refresh it periodically.
            var keyString = configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"]
                ?? throw new InvalidOperationException("Signing key not configured.");
            var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(keyString));

            //  Validate the token's signature and expiration.
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };

            var result = await _handler.ValidateTokenAsync(token, validations);
            if (!result.IsValid)
            {
                _customErrorMessage = $"Token validation failed: {result.Exception?.Message}";
                return AuthenticateResult.Fail(_customErrorMessage);
            }

            var jti = result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            // Blacklist check
            if (string.IsNullOrEmpty(jti) || await _blacklistService.IsRevokedAsync(jti))
            {
                _customErrorMessage = "This token has been revoked by the administrator.";
                return AuthenticateResult.Fail(_customErrorMessage);
            }

            var principal = new ClaimsPrincipal(result.ClaimsIdentity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            _customErrorMessage = $"Token invalid: {ex.Message}";
            return AuthenticateResult.Fail(_customErrorMessage);
        }
    }

    // Notify the client if the investigation fails (or is not verified).
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            error = "unauthorized",
            message = _customErrorMessage
        });

        await Response.WriteAsync(result);
    }
}
