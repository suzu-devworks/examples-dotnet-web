namespace Examples.Web.Authentication;

public interface ITokenBlacklistService
{
    Task<bool> IsRevokedAsync(string jti);
}
