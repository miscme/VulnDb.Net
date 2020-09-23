using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VulnDb.Net.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var value = reader.GetString();
                if (value.Equals(String.Empty)) return DateTime.MinValue;
                return DateTime.Parse(value);
            }
            catch (InvalidOperationException)
            {
                throw new JsonException($"Could not serialize value as DateTime with custom converter enabled");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-mm-dd", CultureInfo.InvariantCulture));
        }
    }
}