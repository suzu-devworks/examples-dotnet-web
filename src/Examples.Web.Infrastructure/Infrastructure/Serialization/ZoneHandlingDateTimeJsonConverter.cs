using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.Web.Infrastructure.Serialization
{
    public class ZoneHandlingDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public ZoneHandlingDateTimeJsonConverter(DateTimeKind kind)
            => Kind = kind;

        public DateTimeKind Kind { get; }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(DateTime.SpecifyKind(value, Kind));
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ZoneHandlingDateTimeJsonConverterAttribute : JsonConverterAttribute
    {
        public ZoneHandlingDateTimeJsonConverterAttribute(DateTimeKind kind)
            => Kind = kind;

        public DateTimeKind Kind { get; }

        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            {
                return new ZoneHandlingDateTimeJsonConverter(Kind);
            }

            return base.CreateConverter(typeToConvert);
        }

    }
}