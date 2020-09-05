using System.Text.Json.Serialization;

namespace VulnDb.Net.Models.Authentication
{
    public class Credentials
    {
        /// <summary>
        /// Your client id
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Your client secret
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; } = "client_credentials";
    }
}
