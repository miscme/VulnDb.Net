using System.Text.Json.Serialization;
using VulnDb.Net.Models.Product;
using VulnDb.Net.Models.Vulnerability;

namespace VulnDb.Net.Models.Vendor
{
    public class VendorInformation : IObjectInformations
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
        
        [JsonPropertyName("products")]
        public ProductInformation Products { get; set; }
        
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
        
        /// <summary>
        /// (optional) The rating for the vendor
        /// </summary>
        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        /// <summary>
        /// (optional) The Average Disclosure Interval for the vendor
        /// </summary>
        [JsonPropertyName("average_disclosure_interval")]
        public int AverageDisclosureInterval { get; set; }

        /// <summary>
        /// (optional) The Cost of Ownership for the vendor
        /// </summary>
        [JsonPropertyName("cost_of_ownership")]
        public string CostOfOwnership { get; set; }
        
        /// <summary>
        /// (optional) VTEM information for the vendor
        /// </summary>
        [JsonPropertyName("vtems")]
        public Vtem[] Vtems { get; set; }
    }
}