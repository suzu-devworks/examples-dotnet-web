using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.Authentication.Oidc.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = "/Account/SignedOut"
        },
        // Clear auth cookie
        CookieAuthenticationDefaults.AuthenticationScheme,
        // Redirect to OIDC provider signout endpoint
        OpenIdConnectDefaults.AuthenticationScheme);
    }
}
