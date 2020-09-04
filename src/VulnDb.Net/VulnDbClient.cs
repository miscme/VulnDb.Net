#nullable enable
using System;
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
        private string BaseUrl { get; set; } = "https://vulndb.cyberriskanalytics.com/"; // TODO: Code set part to check for trailing / and add if not existent
        private string ApiVersionUrl { get; set; } = "api/v1/";
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

        #region API Requests
        /// <summary>
        /// POST /oauth/token:
        /// Requests a new token for your user account and adds the received value as an Authentication Header.
        /// </summary>
        public async Task GetTokenAsync()
        {
            var response = await SendMessageAsync("oauth/token", HttpMethod.Post, payload: new Credentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            });

            var token = await response.Content.ReadFromJsonAsync<Token>(); // TODO: try catch?
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        #region General API Information

        /// <summary>
        /// GET /api/v1/get_merged_ids?start_date=:start_date&end_date=:end_date:
        /// Returns vendors, products and versions that have been merged within a date range.
        /// The start_date parameter is required but the end_date is parameter is optional.
        /// If the end_date parameter is omitted,
        /// the records between start_date and the current date will be returned. 
        /// </summary>
        /// <param name="startDate">The start date (UTC), defaults to 10 years before today's d</param>
        /// <param name="endDate">The end date (UTC), defaults to today's date</param>
        /// <param name="size">The number of merged records to attempt returning, defaults to 20</param>
        /// <param name="page">The page number</param>
        /// <returns>MergedIds Model</returns>
        public async Task<MergedIds> GetMergedIds(string startDate, string endDate = "", int size = 20, int page = 1)
        {
            return null;
        }
        
        /// <summary>
        /// GET /api/v1/account_status:
        /// Returns status information on your user account,
        /// including: organization name, username, e-mail address,
        /// subscription end date, maximum number of allowed API calls per month,
        /// API calls made for the current month and general VulnDB statistics. 
        /// </summary>
        /// <returns>Account Model</returns>
        public async Task<Account> GetAccountStatusAsync()
        {
            var response = await SendMessageAsync("account_status", HttpMethod.Get);
            var account = await response.Content.ReadFromJsonAsync<Account>();
            return account; // TODO: api return null or error???
        }
        #endregion
        #endregion
        
        private async Task<HttpResponseMessage> SendMessageAsync(string url, HttpMethod httpMethod, string urlParams = "",
            Object? payload = null)
        {
            var reqUrl = $"{BaseUrl}{ApiVersionUrl}{url}{urlParams}";
            if (url.Contains("token"))
            {
                reqUrl =  $"{BaseUrl}{url}";
            }
            using var request = new HttpRequestMessage(httpMethod, reqUrl);
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