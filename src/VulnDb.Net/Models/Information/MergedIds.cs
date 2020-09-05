using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Information
{
    public class MergedIds : IObjectInformations
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
        public MergedId[] Results { get; set; }
    }
}