using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class MergedId
    {
        /// <summary>
        /// The database id for the merged record
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The type of data that was merged (either vendor, product or version)
        /// </summary>
        [JsonPropertyName("item_type")]
        public string ItemType { get; set; }

        /// <summary>
        /// The id of the vendor, product or version that was removed
        /// </summary>
        [JsonPropertyName("source_id")]
        public int SourceId { get; set; }

        /// <summary>
        /// The id of the vendor, product or version that was merged into
        /// </summary>
        [JsonPropertyName("destination_id")]
        public int DestinationId { get; set; }
    }

    public class MergedIds
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