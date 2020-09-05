using System;
using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class ClassificationInformations
    {
        [JsonPropertyName("vulnerability")]
        public VulneratbilityInformation Vulnerability { get; set; }
    }

    public class Classifications
    {
        [JsonPropertyName("classification")]
        public Classification[] Classification { get; set; }
    }
}