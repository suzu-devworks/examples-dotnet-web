using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Examples.Web.Infrastructure.Serialization;

namespace Examples.Web.Infrastructure;

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
        //options.IgnoreReadOnlyProperties = true;

        // Configure a converters enumeration values to and from strings.
        options.Converters.Add(new JsonStringEnumConverter());

        // Add Custom converters.
        options.Converters.Add(new LongAsStringConverter());
        options.Converters.Add(new RangeJsonConverter());
        options.Converters.Add(new IndexJsonConverter());

        return options;
    }

}
