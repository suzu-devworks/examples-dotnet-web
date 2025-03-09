using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.Web.Infrastructure.Serialization;

public class RangeJsonConverter : JsonConverter<Range>
{
    public override Range Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => StringConverter.ToRange(reader.GetString() ?? "");

    public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());

}