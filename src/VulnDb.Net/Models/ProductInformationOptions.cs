namespace VulnDb.Net.Models
{
    public class ProductInformationOptions : IObjectOptions
    {
        /// <summary>
        /// When set to true, the Average Disclosure Interval for each product is returned, defaults to false.
        /// </summary>
        public bool AverageDisclosureInterval { get; set; }
        
        /// <summary>
        /// When set to true, the Code Maturity Score for each product is returned, defaults to false.
        /// </summary>
        public bool CodeMaturityScore { get; set; }
        
        /// <summary>
        /// When set to true, the Cost of Ownership for each product is returned, defaults to false
        /// </summary>
        public bool CostOfOwnership { get; set; }
        
        /// <summary>
        /// When set to true, the rating for each product is returned, defaults to false.
        /// </summary>
        public bool Rating { get; set; }
        
        /// <summary>
        /// When set to true, VTEM information for each product is returned, defaults to false.
        /// </summary>
        public bool Vtems { get; set; }

        public ProductInformationOptions()
        {
            AverageDisclosureInterval = false;
            CodeMaturityScore = false;
            CostOfOwnership = false;
            Rating = false;
            Vtems = false;
        }
        
        public override string ToString()
        {
            return
                string.Format(@"&average_disclosure_interval={0}&code_maturity_score={1}&cost_of_ownership={2}&rating={3}&vtems={4}",
                    AverageDisclosureInterval.ToString().ToLower(), CostOfOwnership.ToString().ToLower(),
                    CodeMaturityScore.ToString().ToLower(),
                    Rating.ToString().ToLower(), Vtems.ToString().ToLower());
        }
    }
}