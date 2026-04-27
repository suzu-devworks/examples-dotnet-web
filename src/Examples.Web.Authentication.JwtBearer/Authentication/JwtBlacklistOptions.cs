namespace Examples.Web.Authentication;

public class JwtBlacklistOptions
{
    public IEnumerable<string> RevokedJtiList { get; init; } = [];
}
