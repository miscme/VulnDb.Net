using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Information
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
}