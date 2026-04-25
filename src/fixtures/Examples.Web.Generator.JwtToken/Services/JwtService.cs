using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Generator.JwtToken.Services;

/// <summary>
/// A service responsible for creating and validating JWT tokens based on provided claims and signing credentials.
/// </summary>
public class JwtService
{
    private readonly JsonWebTokenHandler _handler = new();
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Creates a JWT token asynchronously based on the provided signing credentials and claims source.
    /// </summary>
    /// <param name="credentials">The signing credentials to use for the token.</param>
    /// <param name="source">The source of claims for the token.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token.</returns>
    public Task<string> CreateTokenAsync(SigningCredentials credentials, IClaimsSource source,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        SigningCredentials credentials2 = new(credentials.Key,
            credentials.Key is ECDsaSecurityKey ? SecurityAlgorithms.EcdsaSha256 : SecurityAlgorithms.RsaSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = source.Issuer,
            Audience = source.Audience,
            Subject = new ClaimsIdentity(source.GetClaims()),
            Expires = _timeProvider.GetUtcNow().AddMinutes(source.ExpirationMinutes).DateTime,
            SigningCredentials = credentials2
        };

        var token = _handler.CreateToken(descriptor);

        return Task.FromResult(token);
    }

    /// <summary>
    /// Verifies a JWT token asynchronously using the provided public key, issuer, and audience.
    /// </summary>
    /// <param name="token">The JWT token to verify.</param>
    /// <param name="publicKey">The public key to use for verification.</param>
    /// <param name="issuer">The expected issuer of the token.</param>
    /// <param name="audience">The expected audience of the token.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the token validation result.</returns>
    public async Task<TokenValidationResult> VerifyTokenAsync(string token, SecurityKey publicKey, string issuer, string audience,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            RoleClaimType = ClaimTypes.Role
        };

        return await _handler.ValidateTokenAsync(token, parameters);
    }
}
