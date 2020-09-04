using System.Text.Json.Serialization;
using VulnDb.Net.Models;

namespace VulnDb.Net.Models
{
    public class ProductInformations : IObjectInformations
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
        public ProductInformation Results { get; set; }
        
        /// <summary>
        /// The product information
        /// </summary>
        [JsonPropertyName("product")] // TODO: get into results
        public ProductInformation Product { get; set; }
    }
    
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