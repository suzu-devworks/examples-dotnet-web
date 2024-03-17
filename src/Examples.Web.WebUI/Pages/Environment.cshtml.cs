using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.WebUI.Pages
{
    public class Environment(ILogger<Environment> logger, IConfiguration config) : PageModel
    {
        private readonly ILogger<Environment> _logger = logger;

        public IEnumerable<KeyValuePair<string, string?>> Environments = [
            KeyValuePair.Create("ConfigurationKey1", config["ConfigurationKey1"]),
            KeyValuePair.Create("ConfigurationKey2", config["ConfigurationKey2"]),
        ];

        public string? ServiceKeyDevelopmentLibrary { get; } = config["DevAccount_FromLibrary"];
        public string? ServiceKeyProductionLibrary { get; } = config["ProdAccount_FromLibrary"];
        public string? ServiceKeyDevelopment { get; } = config["DevAccount"];
        public string? ServiceKeyProduction { get; } = config["ProdAccount"];

        public void OnGet()
        {
        }
    }
}