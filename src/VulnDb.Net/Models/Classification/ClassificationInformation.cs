using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Classification
{
    public class ClassificationInformation
    {
        /// <summary>
        /// The classification id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The classification name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("longname")]
        public string LongName { get; set; }

        /// <summary>
        /// The classification description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}