using Examples.Web.Authentication.Oidc.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.Authentication.Oidc.Pages;

public class PrivacyModel(ILogger<PrivacyModel> logger) : PageModel
{
    public async Task OnGet()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");
        logger.LogDebugIdToken(idToken);

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        logger.LogDebugAccessToken(accessToken);
    }
}
