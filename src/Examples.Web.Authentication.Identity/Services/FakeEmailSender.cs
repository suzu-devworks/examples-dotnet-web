using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Examples.Web.Authentication.Identity.Services;

public class FakeEmailSender(
    ILogger<FakeEmailSender> logger
    ) : IEmailSender
{
    private readonly ILogger _logger = logger;

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        File.WriteAllText(Path.Combine(dir!, "confirm.html"), ToHtml(email, subject, htmlMessage));
        _logger.LogWarning("Generate test email to {email}: {path}", email, Path.Combine(dir!, "confirm.html"));

        return Task.Delay(500);
    }

    private static string ToHtml(string email, string subject, string htmlMessage) => $"""
        <html>
        <head>
        <title>{subject}</title>
        </head>
        <body>
            <p>
            Dear {email}. 
            </p>
            <div>
            {htmlMessage}
            </div>
        </body>
        </html>
        """;

}
