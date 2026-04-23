using System.CommandLine;
using System.Text.Json;
using Examples.Web.Generator.JwtToken.Services;
using Examples.Web.Generator.JwtToken.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Generator.JwtToken.Commands;

public static class JwksCommand
{
    public static Command Create(IServiceProvider serviceProvider,
        string name = "jwks", string description = "Generate public key information (JWKS) from the certificate.")
    {
        Command command = new(name, description);
        command.ConfigureJwksCommand(serviceProvider);
        return command;
    }

    public static void ConfigureJwksCommand(this Command command, IServiceProvider serviceProvider)
    {
        Option<FileInfo> pubOption = new("--pub", "-f")
        {
            Description = "Path to the public key certificate file(.cer/.crt/.pem/.pfx/.p12) used for verifying the JWT token.",
            Required = true
        };
        command.Options.Add(pubOption);

        Option<string?> passwordOption = new("--password", "-p")
        {
            Description = "Password for the PFX/P12 certificate file.",
        };
        command.Options.Add(passwordOption);

        var kidOption = new Option<string?>("--kid")
        {
            Description = "Key ID (If omitted, the thumbprint on the certificate will be used).",
        };
        command.Options.Add(kidOption);

        command.SetAction(async parseResult =>
        {
            var console = serviceProvider.GetRequiredService<IConsoleService>();

            Parameters parameters = new(
                parseResult.GetValue(pubOption)!,
                parseResult.GetValue(passwordOption),
                parseResult.GetValue(kidOption)
            );

            await new Handler(console).Handle(parameters);
            return 0;
        });
    }
    private record Parameters(
      FileInfo Pub,
      string? Password,
      string? KeyId);

    private static readonly JsonSerializerOptions DefaultJsonOptions
        = new(JsonSerializerDefaults.Web);
    private static readonly JsonSerializerOptions IndentedJsonOptions
        = new(DefaultJsonOptions) { WriteIndented = true };

    private class Handler(IConsoleService console)
    {
        public async Task Handle(Parameters parameters)
        {
            try
            {
                // Load the verification public key from the certificate
                var certificate = await CertificateLoader.LoadX509Certificate2Async(parameters.Pub.FullName,
                    () => parameters.Password
                        ?? console.PromptPassword("Enter Password for PFX file (invisible): ", showAsterisk: false));

                // Convert the SecurityKey to a JsonWebKey
                var key = new X509SecurityKey(certificate, parameters.KeyId ?? certificate.Thumbprint);
                var jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(key);

                // Set the Key ID (kid) in the JWK. If not provided, use the certificate thumbprint.
                jwk.Kid = parameters.KeyId ?? certificate.Thumbprint;
                jwk.Use = "sig";

                // Create a JWKS object containing the JWK
                var jwks = new { keys = new[] { jwk } };

                // Serialize the JWKS to a formatted JSON string
                var options = Console.IsOutputRedirected ? DefaultJsonOptions : IndentedJsonOptions;
                string jsonString = JsonSerializer.Serialize(jwks, options);

                // Displaying the result
                console.WriteSuccess("JWKS generated successfully.");
                console.WriteJson("Generated JWKS", jsonString);

                console.WriteLine();
                console.WriteMessage("If you publish this content to /.well-known/jwks.json on the server,", ConsoleColor.Yellow);
                console.WriteMessage("authentication will succeed with only the Authority settings on the API side.", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                console.WriteException(ex);
            }
        }
    }
}
