using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Examples.WebApi.Applications.Serialization.Models
{
    [JsonConverter(typeof(LocalStatusJsonConverter))]
    public sealed class LocalStatus
    {
        public static readonly LocalStatus Unknown = new(0, null, null);
        public static readonly LocalStatus Wait = new(200, nameof(Wait), @"^[Ww]ait$");
        public static readonly LocalStatus Activate = new(300, nameof(Activate), @"^[Aa]ctivate$");
        public static readonly LocalStatus Completed = new(100, nameof(Completed), @"^[Cc]ompleted$");

        private LocalStatus(int value, string? display, string? pattern)
        {
            Value = value;
            Display = display;
            _matchExpression = (pattern is null) ? null : new(pattern, RegexOptions.Compiled);
        }

        public int Value { get; }
        public string? Display { get; }

        private readonly Regex? _matchExpression;

        public override string? ToString() => this.Display;

        public static IEnumerable<LocalStatus> All => Array.AsReadOnly(new[]{
            LocalStatus.Wait,
            LocalStatus.Activate,
            LocalStatus.Completed,
        });

        public static LocalStatus Get(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var entry = All.FirstOrDefault(x => x._matchExpression?.IsMatch(value) ?? false);
            return entry ?? Unknown;
        }

        public class LocalStatusJsonConverter : JsonConverter<LocalStatus>
        {
            public override LocalStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => LocalStatus.Get(reader.GetString() ?? "");

            public override void Write(Utf8JsonWriter writer, LocalStatus value, JsonSerializerOptions options)
                => writer.WriteStringValue(value?.ToString());
        }
    }
}
