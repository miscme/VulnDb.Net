using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class Account
    {
        [JsonPropertyName("organization_name")]
        public string OrganizationName { get; set; } 

        [JsonPropertyName("user_name_requesting")]
        public string UserNameRequesting { get; set; } 

        [JsonPropertyName("user_email_address_requesting")]
        public string UserEmailAddressRequesting { get; set; } 

        [JsonPropertyName("subscription_end_date")]
        public string SubscriptionEndDate { get; set; } 

        [JsonPropertyName("number_of_api_calls_allowed_per_month")]
        public int NumberOfApiCallsAllowedPerMonth { get; set; } 

        [JsonPropertyName("number_of_api_calls_made_this_month")]
        public int NumberOfApiCallsMadeThisMonth { get; set; } 

        [JsonPropertyName("vulndb_statistics")]
        public string VulndbStatistics { get; set; } 
    }
}