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

        var withX5cOption = new Option<bool>("--with-x5c")
        {
            Description = "Include X.509 certificate chain (x5c) and thumbprint (x5t) in the JWK output.",
        };
        command.Options.Add(withX5cOption);

        command.SetAction(async parseResult =>
        {
            var console = serviceProvider.GetRequiredService<IConsoleService>();

            Parameters parameters = new(
                parseResult.GetValue(pubOption)!,
                parseResult.GetValue(passwordOption),
                parseResult.GetValue(kidOption),
                parseResult.GetValue(withX5cOption)
            );

            try
            {
                await new Handler(console).Handle(parameters);
            }
            catch (Exception ex)
            {
                console.WriteException(ex);
                return 1;
            }

            return 0;
        });
    }

    private record Parameters(
      FileInfo Pub,
      string? Password,
      string? KeyId,
      bool WithX5c);

    private static readonly JsonSerializerOptions DefaultJsonOptions
        = new(JsonSerializerDefaults.Web);
    private static readonly JsonSerializerOptions IndentedJsonOptions
        = new(DefaultJsonOptions) { WriteIndented = true };

    private class Handler(IConsoleService console)
    {
        public async Task Handle(Parameters parameters)
        {
            // Load the certificate once for both public key extraction and optional x5c generation
            using var certificate = await CertificateLoader.LoadCertificateFromFileAsync(parameters.Pub.FullName,
                () => parameters.Password
                    ?? console.PromptPassword("Enter Password for PFX file (invisible): ", showAsterisk: false));

            var credentials = CertificateLoader.GetPublicSigningCredentials(certificate);

            // Convert the SecurityKey to a JsonWebKey (includes public key parameters)
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(credentials.Key);

            if (parameters.WithX5c)
            {
                var x509Key = new X509SecurityKey(certificate) { KeyId = certificate.Thumbprint };

                // ConvertFromX509SecurityKey includes x5c/x5t but lacks public key parameters;
                // copy only the certificate-related fields into the main JWK.
                var x509Jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(x509Key);
                foreach (var chain in x509Jwk.X5c)
                {
                    jwk.X5c.Add(chain);
                }
                jwk.X5t = x509Jwk.X5t;
                jwk.X5tS256 = x509Jwk.X5tS256;
            }

            // Set the Key ID (kid) in the JWK. If not provided, use the certificate thumbprint.
            jwk.Kid = parameters.KeyId ?? jwk.Kid;
            jwk.Alg = credentials.Algorithm;
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
    }
}
