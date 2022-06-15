using System.Text.Json;
using System.Text.Json.Serialization;
using Examples.WebApi.Infrastructure.Converters.Json;

namespace Examples.WebApi.Infrastructure.Extensions;

/// <summary>
/// Extension methods for settings to a <see cref="JsonSerializerOptions" />.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions UseCustomOptions(this JsonSerializerOptions options)
    {
        //options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;  // (default)

        // Properties with default values are ignored during serialization or deserialization. (default Never)
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        // Read-only properties are ignored during serialization. (default false)
        options.IgnoreReadOnlyProperties = true;

        // Configure a converters enumeration values to and from strings.
        options.Converters.Add(new JsonStringEnumConverter());

        // Add Custom converters.
        options.Converters.Add(new TimeSpanJsonConverter());

        return options;
    }

}
