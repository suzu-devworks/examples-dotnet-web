using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.WebApi.Infrastructure.Converters.Json
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => TimeSpan.TryParse(reader.GetString() ?? "", out var result) ? result : TimeSpan.Zero;

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());

    }
}
