using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class VendorInformations
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
        public VendorInformation[] Results { get; set; }
    }
    
    public class VendorInformation
    {
        /// <summary>
        /// The vendor id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>
        /// The vendor name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// The vendor short name
        /// </summary>
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }
        
        /// <summary>
        /// The vendor url
        /// </summary>
        [JsonPropertyName("vendor_url")]
        public string VendorUrl { get; set; }
    }
}