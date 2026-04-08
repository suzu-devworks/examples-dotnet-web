using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.WebUI.Pages
{
    public class EnvironmentModel(ILogger<EnvironmentModel> logger, IConfiguration config) : PageModel
    {
        private readonly ILogger<EnvironmentModel> _logger = logger;

        public IEnumerable<KeyValuePair<string, string?>> Environments { get; } = config
            .AsEnumerable()
            .Where(x => x.Key.StartsWith("Demo:", StringComparison.Ordinal) && x.Value != null)
            .OrderBy(x => x.Key, StringComparer.Ordinal);

        public string? ServiceKeyDevelopmentLibrary { get; } = config["DevAccount_FromLibrary"];
        public string? ServiceKeyProductionLibrary { get; } = config["ProdAccount_FromLibrary"];

        public void OnGet()
        {
        }
    }
}
