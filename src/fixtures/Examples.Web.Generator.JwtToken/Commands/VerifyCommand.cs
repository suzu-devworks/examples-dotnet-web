using System.CommandLine;
using Examples.Web.Generator.JwtToken.Services;
using Examples.Web.Generator.JwtToken.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Generator.JwtToken.Commands;

public static class VerifyCommand
{
    public static Command Create(IServiceProvider serviceProvider,
        string name = "verify", string description = "Verifies a JWT token based on the provided parameters.")
    {
        Command command = new(name, description);
        command.ConfigureVerifyCommand(serviceProvider);
        return command;
    }

    public static void ConfigureVerifyCommand(this Command command, IServiceProvider serviceProvider)
    {
        Option<string> tokenOption = new("--token", "-t")
        {
            Description = "The JWT token to verify.",
            Required = true
        };
        command.Options.Add(tokenOption);

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

        Option<string> issOption = new("--iss")
        {
            Description = "Expected issuer (iss) claim.",
            DefaultValueFactory = _ => "my-local-issuer"
        };
        command.Options.Add(issOption);

        Option<string> audOption = new("--aud")
        {
            Description = "Expected audience (aud) claim.",
            DefaultValueFactory = _ => "my-api"
        };
        command.Options.Add(audOption);

        command.SetAction(async parseResult =>
        {
            var console = serviceProvider.GetRequiredService<IConsoleService>();
            var jwt = serviceProvider.GetRequiredService<JwtService>();

            Parameters parameters = new(
                 parseResult.GetValue(tokenOption)!,
                 parseResult.GetValue(pubOption)!,
                 parseResult.GetValue(passwordOption),
                 parseResult.GetValue(audOption)!,
                 parseResult.GetValue(issOption)!
            );

            await new Handler(console, jwt).Handle(parameters);
            return 0;
        });
    }

    private record Parameters(
        string Token,
        FileInfo Pub,
        string? Password,
        string Audience,
        string Issuer);

    private class Handler(IConsoleService console, JwtService jwt)
    {
        public async Task Handle(Parameters parameters)
        {
            try
            {
                // Load the verification public key from the certificate
                var credentials = await CertificateLoader.LoadPublicSigningCredentialsAsync(parameters.Pub.FullName,
                    () => parameters.Password
                        ?? console.PromptPassword("Enter Password for PFX file (invisible): ", showAsterisk: false));

                // Verify the token
                TokenValidationResult result = await jwt.VerifyTokenAsync(parameters.Token, credentials.Key, parameters.Issuer, parameters.Audience);

                if (result.IsValid)
                {
                    console.WriteMessage("✓ Token is valid!", ConsoleColor.Green);
                    console.WriteLine();
                    console.WriteClaims("Token Claims", result);
                }
                else
                {
                    console.WriteMessage($"✗ Token is invalid: {result.Exception.Message}", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                console.WriteException(ex);
            }
        }
    }
}
