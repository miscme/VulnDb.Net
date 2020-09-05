using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class VendorInformations : IObjectInformations
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
        /// 
        [JsonPropertyName("results")]
        public VendorInformation[] Results { get; set; }
        
        // [JsonPropertyName("vendor")]
        // private VendorInformation Vendor
        // {
        //     set
        //     {
        //         if (Vendor != null)
        //         {
        //             Results = new VendorInformation[]
        //             {
        //                 new VendorInformation()
        //                 {
        //                     Id = Vendor.Id,
        //                     Name = Vendor.Name,
        //                     Rating = Vendor.Rating,
        //                     Vtems = Vendor.Vtems,
        //                     ShortName = Vendor.ShortName,
        //                     VendorUrl = Vendor.VendorUrl,
        //                     AverageDisclosureInterval = Vendor.AverageDisclosureInterval,
        //                     CostOfOwnership = Vendor.CostOfOwnership
        //                 }
        //             };
        //         }
        //     }
        //     get => Vendor;
        // }

        [JsonPropertyName("vendor")] // TODO: Add to Result
        public VendorInformation Vendor { get; set; }
    }
    
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