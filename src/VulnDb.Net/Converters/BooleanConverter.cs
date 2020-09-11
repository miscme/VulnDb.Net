using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VulnDb.Net.Converters
{
    public class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var value = reader.GetString();
                if (value.ToLower().Equals("true")) return true;
                if (value.ToLower().Equals("false")) return false;
            }
            catch (InvalidOperationException)
            {
                return reader.GetBoolean();
            }
            throw new JsonException($"Could not serialize value as bool with custom converter enabled");

        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}