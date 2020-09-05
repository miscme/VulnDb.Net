using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Version
{
    public class VersionInformation
    {
        /// <summary>
        /// The version id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The version name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("affected")]
        public string Affected { get; set; }

        [JsonPropertyName("cpe")]
        public VersionInformation[] Cpe { get; set; }
    }
}