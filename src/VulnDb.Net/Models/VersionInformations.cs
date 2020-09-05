using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    
    public class VersionInformations : IObjectInformations
    {
        /// <summary>
        /// The result set total count
        /// </summary>
        [JsonPropertyName("total_entries")]
        public int TotalEntries { get; set; }

        /// <summary>
        /// The current page, defaults to 1 if no page was set in the request
        /// </summary>
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// The results collection with the following parameters
        /// </summary>
        [JsonPropertyName("results")]
        public VersionInformation[] Results { get; set; }
    }
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