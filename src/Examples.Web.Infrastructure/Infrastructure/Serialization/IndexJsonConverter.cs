using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.Web.Infrastructure.Serialization;

public class IndexJsonConverter : JsonConverter<Index>
{
    public override Index Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => StringConverter.ToIndex(reader.GetString() ?? "");

    public override void Write(Utf8JsonWriter writer, Index value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());

}