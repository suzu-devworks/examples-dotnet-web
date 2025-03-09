using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.Web.Infrastructure.Serialization;

/// <summary>
///  Converts <see cref="TimeSpan"/> values to and from strings.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/serialization/6.0/timespan-serialization-format"/>
[Obsolete("Include .NET 6.0")]
public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => TimeSpan.TryParse(reader.GetString() ?? "", out var result) ? result : TimeSpan.Zero;

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());

}