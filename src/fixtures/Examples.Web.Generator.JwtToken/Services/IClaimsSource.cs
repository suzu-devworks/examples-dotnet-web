using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Examples.Web.Generator.JwtToken.Services;

/// <summary>
/// An interface that represents the source of the claims to be included in the JWT token.
/// </summary>
public interface IClaimsSource
{
    /// <summary>
    /// The subject of the token, typically representing the user or entity the token is issued for.
    /// </summary>
    string Subject { get; }

    /// <summary>
    /// The audience of the token, representing the intended recipients of the token.
    /// </summary>
    string Audience { get; }

    /// <summary>
    /// The issuer of the token, representing the entity that issued the token.
    /// </summary>
    string Issuer { get; }

    /// <summary>
    /// The expiration time (in minutes) for the token, indicating how long the token is valid before it expires.
    /// </summary>
    int ExpirationMinutes { get; }

    /// <summary>
    /// The roles associated with the token, which can be used for authorization purposes. Each role will be included as a claim in the token with the claim type "role".
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Gets the claims to be included in the JWT token based on the properties of the implementing class.
    /// </summary>
    /// <returns> A collection of claims to be included in the JWT token. </returns>
    IEnumerable<Claim> GetClaims()
    {
        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Sub, Subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        foreach (var role in Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims.AsEnumerable();
    }
}
