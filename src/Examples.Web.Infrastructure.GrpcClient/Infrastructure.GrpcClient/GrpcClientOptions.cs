using System.ComponentModel.DataAnnotations;

namespace Examples.Web.Infrastructure.GrpcClient;

/// <summary>
/// Options for configuring gRPC clients, including base addresses, timeouts, and certificate validation settings.
/// </summary>
public class GrpcClientOptions
{
    /// <summary>
    /// The name of the configuration section in the appsettings.json file.
    /// </summary>
    public static string ConfigurationSectionName => "GrpcClient";

    /// <summary>
    /// A dictionary of gRPC client configurations.
    /// </summary>
    public IReadOnlyDictionary<string, GrpcClientOptionsSettings> Clients { get; init; } = new Dictionary<string, GrpcClientOptionsSettings>();

    /// <summary>
    /// Gets the configuration settings for the Greeter gRPC client.
    /// </summary>
    public GrpcClientOptionsSettings Greeter => Clients.GetValueOrDefault("Greeter")
        ?? throw new InvalidOperationException("Greeter client configuration is missing.");

    /// <summary>
    /// Configuration settings for a specific gRPC client.
    /// </summary>
    public class GrpcClientOptionsSettings
    {
        /// <summary>
        /// The base address of the gRPC service.
        /// </summary>
        [Required]
        [Url]
        public string BaseAddress { get; init; } = string.Empty;

        /// <summary>
        /// Gets the base address as a Uri object for use in gRPC client configuration.
        /// </summary>
        public Uri BaseAddressUri => new Uri(BaseAddress);

        /// <summary>
        /// The timeout for gRPC calls in seconds.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "TimeoutSeconds must be greater than 0.")]
        public int? TimeoutSeconds { get; init; }

        /// <summary>
        /// Gets the timeout as a TimeSpan object for use in gRPC client configuration.
        /// </summary>
        public TimeSpan? Timeout => TimeoutSeconds.HasValue ? TimeSpan.FromSeconds(TimeoutSeconds.Value) : null;

        /// <summary>
        /// Indicates whether to allow untrusted SSL/TLS certificates when connecting to the gRPC service.
        /// </summary>
        public bool AllowUntrustedCertificate { get; init; } = false;
    }
}
