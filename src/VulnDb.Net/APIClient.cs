#nullable enable
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VulnDb.Net.Clients;
using VulnDb.Net.Models.Authentication;
using Xunit;

namespace VulnDb.Net
{
    public class APIClient
    {
        private string? ClientId { get; }
        private string? ClientSecret { get; }
        private readonly HttpClient _httpClient;

        public AccountClient Account { get; }
        public ClassificationsClient Classifications { get; }
        public ProductClient Product { get; }
        public VendorClient Vendor { get; }
        public VersionClient Version { get; }
        public VulnerabilityClient Vulnerability { get; }


        public APIClient(string apiToken) : this(null, null, apiToken)
        {
            Assert.NotNull(apiToken);
        }

        public APIClient(string? clientId, string? clientSecret, string? apiToken = null)
        {
            var clientHandler = new HttpClientHandler();
            var cookieContainer = new CookieContainer();
            clientHandler.CookieContainer = cookieContainer;
            _httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(BaseClient.BaseUrl)
            };

            _httpClient.DefaultRequestHeaders.Referrer = new Uri(BaseClient.BaseUrl);

            if (string.IsNullOrEmpty(apiToken))
            {
                ClientId ??= clientId;
                ClientSecret ??= clientSecret;
                apiToken = GetTokenAsync().Result.AccessToken;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    apiToken);

            Account = new AccountClient(_httpClient);
            Classifications = new ClassificationsClient(_httpClient);
            Product = new ProductClient(_httpClient);
            Vendor = new VendorClient(_httpClient);
            Version = new VersionClient(_httpClient);
            Vulnerability = new VulnerabilityClient(_httpClient);
        }

        /// <summary>
        /// POST /oauth/token:
        /// Requests a new token for your user account and adds the received value as an Authentication Header.
        /// </summary>
        private async Task<Token> GetTokenAsync()
        {
            var response = await BaseClient.SendMessageAsync(_httpClient, "oauth/token", HttpMethod.Post, payload: new Credentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            });

            var token = await response.Content.ReadFromJsonAsync<Token>();

            return token;
        }
    }
}
