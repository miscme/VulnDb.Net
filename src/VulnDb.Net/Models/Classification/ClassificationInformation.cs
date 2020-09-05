using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Classification
{
    public class ClassificationInformation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("longname")]
        public string LongName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}