using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.WebApi.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions UseCustomOptions(this JsonSerializerOptions options)
        {
            //options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;  // (default)

            // Properties with default values are ignored during serialization or deserialization. (default Never)
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

            // Read-only properties are ignored during serialization. (default false)
            options.IgnoreReadOnlyProperties = true;

            // Configure a converts enumeration values to and from strings.
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}