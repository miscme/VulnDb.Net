namespace VulnDb.Net.Models
{
    public class ProductInformationOptions
    {
        private bool AverageDisclosureInterval { get; set; }
        private bool CodeMaturityScore { get; set; }
        private bool CostOfOwnership { get; set; }
        private bool Rating { get; set; }
        private bool Vtems { get; set; }

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