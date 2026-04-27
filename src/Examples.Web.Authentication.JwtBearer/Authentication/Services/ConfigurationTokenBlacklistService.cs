using Microsoft.Extensions.Options;

namespace Examples.Web.Authentication.Services;

public partial class ConfigurationTokenBlacklistService(
    IOptionsMonitor<JwtBlacklistOptions> options,
    ILogger<ConfigurationTokenBlacklistService> logger
) : ITokenBlacklistService
{
    public Task<bool> IsRevokedAsync(string jti)
    {
        var value = options.CurrentValue;
        var isRevoked = value.RevokedJtiList.Contains(jti);
        if (isRevoked)
        {
            LogRevokedJti(jti);
        }
        return Task.FromResult(isRevoked);
    }

    [LoggerMessage(0, LogLevel.Information, "jti '{jti}' is revoked.")]
    private partial void LogRevokedJti(string jti);
}
