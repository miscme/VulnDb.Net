using System.Text.Json.Serialization;

namespace VulnDb.Net.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("error_id")]
        public int ErrorId { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
    }
}