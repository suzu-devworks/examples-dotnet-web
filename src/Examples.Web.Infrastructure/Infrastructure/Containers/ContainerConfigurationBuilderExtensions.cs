
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Examples.Web.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;

namespace Examples.Web.Infrastructure.Containers;

/// <summary>
/// Extension methods for container configuration to <see cref="IConfigurationBuilder"/>.
/// </summary>
public static class ContainerConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a configuration provider to read secrets from a specified directory (defaulting to /run/secrets).
    /// The provider is appended to the builder, so its values can override existing configuration values for the same keys.
    /// </summary>
    /// <param name="configuration">The configuration builder to add the provider to.</param>
    /// <param name="directoryPath">The directory path to read secrets from.</param>
    /// <returns>The updated configuration builder.</returns>
    public static IConfigurationBuilder AddContainerSecrets(this IConfigurationBuilder configuration,
        string directoryPath = "/run/secrets")
    {
        return configuration.AddKeyPerFile(directoryPath: directoryPath, optional: true);
    }

    /// <summary>
    /// Adds configuration provider to read Kestrel certificate password from a file specified in configuration.
    /// </summary>
    /// <param name="configuration">The configuration builder to add the provider to.</param>
    /// <returns>The updated configuration builder.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configuration is not of type <see cref="IConfigurationManager"/>.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified certificate password file does not exist.</exception>
    public static IConfigurationBuilder AddKestrelCertPasswordFile(this IConfigurationBuilder configuration)
    {
        if (configuration is not IConfigurationManager configurationManager)
        {
            throw new InvalidOperationException("Configuration must be of type IConfigurationManager to add Kestrel certificate password file provider.");
        }

        return configurationManager.AddKestrelCertPasswordFile();
    }

    /// <summary>
    /// Adds configuration provider to read Kestrel certificate password from a file specified in configuration.
    /// </summary>
    /// <param name="configuration">The configuration manager to add the provider to.</param>
    /// <returns>The updated configuration manager.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the specified certificate password file does not exist.</exception>
    public static IConfigurationManager AddKestrelCertPasswordFile(this IConfigurationManager configuration)
    {
        var value = configuration.GetSection("Kestrel:Certificates:Default:Password:FILE").Value;
        if (string.IsNullOrEmpty(value))
        {
            return configuration;
        }

        var passwordFilePath = value.ToSafePath();
        if (!File.Exists(passwordFilePath))
        {
            throw new FileNotFoundException($"Kestrel certificate password file not found at path: {passwordFilePath}");
        }

        var password = File.ReadLines(passwordFilePath).FirstOrDefault();
        configuration["Kestrel:Certificates:Default:Password"] = password;
        Debug.WriteLine("[ContainerConfigurationBuilderExtensions] Set: Kestrel:Certificates:Default:Password from file.");
        return configuration;
    }
}
