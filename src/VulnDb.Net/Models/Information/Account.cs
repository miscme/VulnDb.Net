using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Information
{
    public class Account : IObjectInformations
    {
        /// <summary>
        /// The current user's organization name
        /// </summary>
        [JsonPropertyName("organization_name")]
        public string OrganizationName { get; set; } 
        
        /// <summary>
        /// The current user's username
        /// </summary>
        [JsonPropertyName("user_name_requesting")]
        public string UserNameRequesting { get; set; } 

        /// <summary>
        /// The current user's email address
        /// </summary>
        [JsonPropertyName("user_email_address_requesting")]
        public string UserEmailAddressRequesting { get; set; } 

        /// <summary>
        /// The end date for the current user's subscription
        /// </summary>
        [JsonPropertyName("subscription_end_date")]
        public string SubscriptionEndDate { get; set; } 

        /// <summary>
        /// The total number of API calls allowed per month
        /// </summary>
        [JsonPropertyName("number_of_api_calls_allowed_per_month")]
        public int NumberOfApiCallsAllowedPerMonth { get; set; } 

        /// <summary>
        /// The number of API calls made this month
        /// </summary>
        [JsonPropertyName("number_of_api_calls_made_this_month")]
        public int NumberOfApiCallsMadeThisMonth { get; set; } 

        /// <summary>
        /// A text string consisting of some general VulnDB statistics
        /// </summary>
        [JsonPropertyName("vulndb_statistics")]
        public string VulndbStatistics { get; set; } 
    }
}