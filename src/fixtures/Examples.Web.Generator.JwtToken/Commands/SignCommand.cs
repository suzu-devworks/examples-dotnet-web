using System.CommandLine;
using Examples.Web.Generator.JwtToken.Services;
using Examples.Web.Generator.JwtToken.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Web.Generator.JwtToken.Commands;

public static class SignCommand
{
    public static Command Create(IServiceProvider serviceProvider,
        string name = "sign", string description = "Generates a JWT token based on the provided parameters.")
    {
        Command command = new(name, description);
        command.ConfigureSignCommand(serviceProvider);
        return command;
    }

    public static void ConfigureSignCommand(this Command command, IServiceProvider serviceProvider)
    {
        Option<FileInfo> pfxOption = new("--pfx", "-f")
        {
            Description = "Path to the PFX/P12 certificate file used for signing the JWT token.",
            Required = true
        };
        command.Options.Add(pfxOption);

        Option<string?> passwordOption = new("--password", "-p")
        {
            Description = "Password for the PFX/P12 certificate file.",
        };
        command.Options.Add(passwordOption);

        Option<string> subOption = new("--sub")
        {
            Description = "Subject (sub) claim.",
            DefaultValueFactory = _ => "user"
        };
        command.Options.Add(subOption);

        Option<string> audOption = new("--aud")
        {
            Description = "Audience (aud) claim.",
            DefaultValueFactory = _ => "my-api"
        };
        command.Options.Add(audOption);

        Option<string> issOption = new("--iss")
        {
            Description = "Issuer (iss) claim.",
            DefaultValueFactory = _ => "my-local-issuer"
        };
        command.Options.Add(issOption);

        Option<int> expOption = new("--exp")
        {
            Description = "Expiration time (minutes) for the token.",
            DefaultValueFactory = _ => 60
        };
        command.Options.Add(expOption);

        Option<string[]> roleOption = new("--role")
        {
            Description = "Roles (role claim) for the token. Multiple roles can be specified.",
        };
        command.Options.Add(roleOption);

        command.SetAction(async parseResult =>
        {
            var console = serviceProvider.GetRequiredService<IConsoleService>();
            var jwt = serviceProvider.GetRequiredService<JwtService>();

            Parameters parameters = new(
                parseResult.GetValue(pfxOption)!,
                parseResult.GetValue(passwordOption),
                parseResult.GetValue(subOption)!,
                parseResult.GetValue(audOption)!,
                parseResult.GetValue(issOption)!,
                parseResult.GetValue(expOption),
                parseResult.GetValue(roleOption) ?? []
            );

            try
            {
                await new Handler(console, jwt).Handle(parameters);
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
        FileInfo Pfx,
        string? Password,
        string Subject,
        string Audience,
        string Issuer,
        int ExpirationMinutes,
        IEnumerable<string> Roles) : IClaimsSource;

    private class Handler(IConsoleService console, JwtService jwt)
    {
        public async Task Handle(Parameters parameters)
        {
            // Load the signing credentials from the certificate
            var credentials = await CertificateLoader.LoadSigningCredentialsAsync(parameters.Pfx.FullName,
                () => parameters.Password
                    ?? console.PromptPassword("Enter Password for PFX file (invisible): ", showAsterisk: false));

            // Creating a JWT token
            var token = await jwt.CreateTokenAsync(
                credentials,
                parameters
            );

            // Displaying the result
            console.WriteSuccess("Token generated successfully.");
            console.WriteMessage($"Algorithm: {credentials.Algorithm}", ConsoleColor.Yellow);
            console.WriteToken("Generated JWT", token);
        }
    }
}
