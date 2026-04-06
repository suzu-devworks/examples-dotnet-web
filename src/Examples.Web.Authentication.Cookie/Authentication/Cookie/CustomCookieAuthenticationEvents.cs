
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Examples.Web.Authentication.Cookie;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    private readonly IUserRepository _userRepository;

    public CustomCookieAuthenticationEvents(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;

        // Look for the LastChanged claim.
        var lastChanged = (from c in userPrincipal?.Claims
                           where c.Type == "LastChanged"
                           select c.Value).FirstOrDefault();

        var email = userPrincipal?.Identity?.Name
            ?? throw new InvalidOperationException("Principal does not have a Name claim.");

        if (string.IsNullOrEmpty(lastChanged) ||
            !await _userRepository.ValidateLastChangedAsync(email, lastChanged))
        {
            context.RejectPrincipal();

            await context.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
