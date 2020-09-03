#nullable enable
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VulnDb.Net.Models;

namespace VulnDb.Net
{
    public class VulnDbClient : IDisposable
    {
        private string BaseUrl { get; set; } = "https://vulndb.cyberriskanalytics.com";
        private string? ClientId { get; }
        private string? ClientSecret { get; }
        private readonly HttpClient _httpClient;

        
        public VulnDbClient(string apiToken) : this(null, null, apiToken)
        {
        }

        public VulnDbClient(string? clientId, string? clientSecret, string? apiToken = null)
        {
            var clientHandler = new HttpClientHandler();
            var cookieContainer = new CookieContainer();
            clientHandler.CookieContainer = cookieContainer;
            _httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Referrer = new Uri(BaseUrl);
            if (apiToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    apiToken);
            }
            else
            {
                ClientId ??= clientId;
                ClientSecret ??= clientSecret;
            }
        }

        public async Task<Token> GetTokenAsync()
        {
            var response = await SendMessageAsync("/oauth/token", HttpMethod.Post, new Credentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            });
            var token = await GetResponseObjectAsync(response) as Token;
            return token;
        }

        private async Task<Object> GetResponseObjectAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) // TODO: Introduce error handling system
            {
                try
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<Token>();
                    return responseContent;
                }
                catch (NotSupportedException e)
                {
                    return null;
                }
                catch (JsonException)
                {
                    return null;
                }
            }

            return null;
        }
        
        private async Task<HttpResponseMessage> SendMessageAsync(string url, HttpMethod httpMethod,
            Object? payload = null)
        {
            using var request = new HttpRequestMessage(httpMethod, $"{BaseUrl}{url}");
            if (payload != null)
            {
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                request.Content = content;
            }
            return await _httpClient.SendAsync(request);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}