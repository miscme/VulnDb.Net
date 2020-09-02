using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulnDb.Net.Models
{
    public class Auth
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; } = "client_credentials";
    }
}
