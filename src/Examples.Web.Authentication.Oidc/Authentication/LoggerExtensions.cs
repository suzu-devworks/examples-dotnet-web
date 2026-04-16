namespace Examples.Web.Authentication.Oidc.Authentication;

public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "ID Token: {IdToken}")]
    public static partial void LogDebugIdToken(this ILogger logger, string? idToken);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Access Token: {AccessToken}")]
    public static partial void LogDebugAccessToken(this ILogger logger, string? accessToken);

}
