
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.Authentication.Certificate.Pages;

public class PrivacyModel : PageModel
{
    public string ClientCertificateSubject { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public void OnGet()
    {
        // Retrieve the certificate information used for the connection.
        var clientCertificate = Request.HttpContext.Connection.ClientCertificate;

        // Retrieve the user information created as a result of authentication.
        var userName = User.Identity?.Name;

        ClientCertificateSubject = clientCertificate?.Subject ?? "No client certificate";
        UserName = userName ?? "Anonymous";
    }
}

