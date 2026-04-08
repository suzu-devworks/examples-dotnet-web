namespace Examples.Web.WebApp.Applications.Environments;

/// <summary>
/// Extension methods for adding application configuration to a <see cref="ConfigurationManager"/>.
/// </summary>
public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddApplicationConfiguration(this ConfigurationManager configuration)
    {
        configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["Demo:Source:ConfigurationManager"] = "Defined in Program ConfigurationManager",
                ["Demo:OrderProbe:All"] = "Program ConfigurationManager",
                ["Demo:Compare:ConfigureVsProgramConfigManager"] = "From Program ConfigurationManager",
                ["Demo:Compare:UseVsProgramConfigManager"] = "From Program ConfigurationManager",
            });

        return configuration;
    }
}
