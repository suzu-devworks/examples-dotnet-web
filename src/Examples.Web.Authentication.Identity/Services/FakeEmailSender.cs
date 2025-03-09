using Microsoft.AspNetCore.Identity.UI.Services;

namespace Examples.Web.Authentication.Identity.Services;

public class FakeEmailSender(
    ILogger<FakeEmailSender> logger
    ) : IEmailSender
{
    private readonly ILogger _logger = logger;

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var dir = Path.Combine(Environment.CurrentDirectory, "temp");
        Directory.CreateDirectory(dir);

        File.WriteAllText(Path.Combine(dir!, "confirm.html"), ToHtml(email, subject, htmlMessage));
        _logger.LogDebug("Generate e-mail: {path}", Path.Combine(dir!, "confirm.html"));
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