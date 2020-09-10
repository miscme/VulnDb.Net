using System.Text.Json.Serialization;
using VulnDb.Net.Models.Version;
using VulnDb.Net.Models.Vulnerability;

namespace VulnDb.Net.Models.Product
{
    public class ProductInformation
    {
        /// <summary>
        /// The product id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("versions")]
        public VersionInformation[] Versions { get; set; }

        /// <summary>
        /// (optional) The rating for the product
        /// </summary>
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        
        /// <summary>
        /// (optional) The Code Maturity Score for the product
        /// </summary>
        [JsonPropertyName("code_maturity_score")]
        public string CodeMaturityScore { get; set; }
        
        /// <summary>
        /// (optional) The Average Disclosure Interval for the product
        /// </summary>
        [JsonPropertyName("average_disclosure_interval")]
        public int AverageDisclosureInterval { get; set; }

        /// <summary>
        /// (optional) The Cost of Ownership for the product
        /// </summary>
        [JsonPropertyName("cost_of_ownership")]
        public string CostOfOwnership { get; set; }
        
        /// <summary>
        /// (optional) The VTEM information for the product
        /// </summary>
        [JsonPropertyName("vtems")]
        public Vtem[] Vtems { get; set; }
    }
}