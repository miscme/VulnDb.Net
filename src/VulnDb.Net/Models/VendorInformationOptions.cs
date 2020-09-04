using System;

namespace VulnDb.Net.Models
{
    public class VendorInformationOptions
    {
        /// <summary>
        /// When set to true, the Average Disclosure Interval for each vendor is returned, defaults to false.
        /// </summary>
        private bool AverageDisclosureInterval { get; set; }
        /// <summary>
        /// When set to true, the Cost of Ownership for each vendor is returned, defaults to false.
        /// </summary>
        private bool CostOfOwnership { get; set; }
        /// <summary>
        /// When set to true, the rating for each vendor is returned, defaults to false.
        /// </summary>
        private bool Rating { get; set; }
        /// <summary>
        /// When set to true, VTEM information for each vendor is returned, defaults to false.
        /// </summary>
        private bool Vtems { get; set; }
        
        /// <summary>
        /// These options can be passed to any call to change its behavior
        /// </summary>
        public VendorInformationOptions()
        {
            AverageDisclosureInterval = false;
            CostOfOwnership = false;
            Rating = false;
            Vtems = false;
        }

        public override string ToString()
        {
            return
                string.Format(@"&average_disclosure_interval={0}&cost_of_ownership={1}&rating={2}&vtems={3}",
                    AverageDisclosureInterval.ToString().ToLower(), CostOfOwnership.ToString().ToLower(),
                    Rating.ToString().ToLower(), Vtems.ToString().ToLower());
        }
    }
}