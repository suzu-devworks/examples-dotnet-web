namespace Examples.Web.WebApp.Applications.Environments;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddApplicationConfiguration(this ConfigurationManager configuration)
    {
        configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConfigurationKey1"] = "From ConfigurationManager: overwrite.",
                // ["ConfigurationKey2"] = "From ConfigurationManager: overwrite."
            });

        return configuration;
    }
}
