using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Classification
{
    public class ClassificationInformations : IObjectInformations
    {
        [JsonPropertyName("total_entries")]
        public int TotalEntries { get; set; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("results")]
        public ClassificationInformation[] Results { get; set; }
    }
}