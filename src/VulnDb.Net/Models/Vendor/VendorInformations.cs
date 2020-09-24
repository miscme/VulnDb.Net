using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Vendor
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
        private VendorInformation Vendor { get; set; }
    }
}