using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class Credentials
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; } = "client_credentials";
    }
}
