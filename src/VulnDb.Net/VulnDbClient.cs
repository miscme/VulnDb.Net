using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VulnDb.Net.Models;

namespace VulnDb.Net
{
    public class VulnDbClient : IDisposable
    {
        private string BaseUrl { get; set; } = "https://vulndb.cyberriskanalytics.com/";
        private string ClientId { get; }
        private string ClientSecret { get; }
        private readonly HttpClient _httpClient;

        
        public VulnDbClient(string apiToken) : this(null, null, apiToken)
        {
        }

        public VulnDbClient(string clientId, string clientSecret, string apiToken = null)
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
            var credentials = new Auth
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            };
            var payload = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await SendMessageAsync("/oauth/token", HttpMethod.Post, content);
            if (response == null || !response.IsSuccessStatusCode) return new Token();
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var sr = new StreamReader(responseStream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(sr) {CloseInput = false};
            var responseBody = await jsonReader.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(responseBody);
            return token;
        }
        
        private async Task<HttpResponseMessage> SendMessageAsync(string url, HttpMethod httpMethod, HttpContent httpContent = null)
        {
            var request = new HttpRequestMessage(httpMethod, $"{BaseUrl}{url}");
            request.Content ??= httpContent;
            return await _httpClient.SendAsync(request);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}