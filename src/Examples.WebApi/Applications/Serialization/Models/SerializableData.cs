using System;
using System.Text.Json.Serialization;
using Examples.WebApi.Infrastructure.Converters.Json;

namespace Examples.WebApi.Applications.Serialization.Models
{
    public class SerializableData
    {
        public Guid Id { get; init; }

        public int SearchId { get; init; }

        public DateTimeOffset Date { get; set; }

        public TimeSpan Elaps { get; set; }

        public GrobalStatus Grobal { get; set; }

        public LocalStatus Local { get; set; } = LocalStatus.Unknown;

        [JsonConverter(typeof(RangeJsonConverter))]
        public Range OffsetRange { get; set; }

    }
}
