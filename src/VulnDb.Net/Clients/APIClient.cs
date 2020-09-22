#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VulnDb.Net.Converters;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Authentication;
using VulnDb.Net.Models.Classification;
using VulnDb.Net.Models.Information;
using VulnDb.Net.Models.Product;
using VulnDb.Net.Models.Version;
using VulnDb.Net.Models.Vulnerability;
using Xunit;

namespace VulnDb.Net.Clients
{
    public class APIClient : IDisposable
    {
        private string BaseUrl { get; set; } = "https://vulndb.cyberriskanalytics.com/"; // TODO: Code set part to check for trailing / and add if not existent
        private string ApiVersionUrl { get; set; } = "api/v1/";
        private string? ClientId { get; }
        private string? ClientSecret { get; }
        private readonly HttpClient _httpClient;
        
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

            var token = await response.Content.ReadFromJsonAsync<Token>();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }
        
        #region General API Information
        /// <summary>
        /// Returns status information on your user account,
        /// including: organization name, username, e-mail address,
        /// subscription end date, maximum number of allowed API calls per month,
        /// API calls made for the current month and general VulnDB statistics. 
        /// </summary>
        /// <returns>Account Model</returns>
        public async Task<VulnDbResponse<Account?>> GetAccountStatusAsync()
        {
            var response = await SendMessageAsync("account_status", HttpMethod.Get);
            var vulnDbResponse = await GetVulnDbObject<Account>(response);
            return vulnDbResponse;
        }
        
        /// <summary>
        /// Returns vendors, products and versions that have been merged within a date range.
        /// The start_date(ISO 8601) parameter is required but the end_date is parameter is optional.
        /// If the end_date(ISO 8601) parameter is omitted,
        /// the records between start_date and the current date will be returned. 
        /// </summary>
        /// <param name="startDate">The start date (UTC), defaults to 10 years before today's d</param>
        /// <param name="endDate">The end date (UTC), defaults to today's date</param>
        /// <param name="size">The number of merged records to attempt returning, defaults to 20</param>
        /// <param name="page">The page number</param>
        /// <returns>MergedIds Model</returns>
        public async Task<VulnDbResponse<MergedIds?>> GetMergedIdsAsync(string startDate = "", string endDate = "", int size = 20, int page = 1)
        {
            var urlParams = $@"start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var response = await SendMessageAsync("get_merged_ids", HttpMethod.Get, urlParams);
            var vulnDbResponse = await GetVulnDbObject<MergedIds>(response);
            return vulnDbResponse;
        }
        #endregion
        
        #region API Request Helper Methods
        /// <summary>
        /// Creates the meta response object,
        /// created to provide a consistent return type and encapsulate possible errors.
        /// </summary>
        /// <param name="response"></param>Response from the request to VulnDb
        /// <typeparam name="T"></typeparam>Type of the response model
        /// <returns>Meta VulnDb Response Object</returns>
        private async Task<VulnDbResponse<T?>> GetVulnDbObject<T>(HttpResponseMessage response)
            where T : class, IObjectInformations
        {
            VulnDbResponse<T?> vulnDbResponse;
            if (!response.IsSuccessStatusCode)
            {
                var errorString = await response.Content.ReadAsStringAsync();
                ErrorResponse? error = null;
                if (errorString != null)
                {
                    error = JsonSerializer.Deserialize<ErrorResponse>(errorString);
                }
                vulnDbResponse = new VulnDbResponse<T?>(null, error);
                return vulnDbResponse;
            }

            try
            {
                var serializer = new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        Converters =
                        {
                            new BooleanConverter(),
                            new DateTimeConverter()
                        }
                    };
                var responseModel = await response.Content.ReadFromJsonAsync<T?>(serializer);
                vulnDbResponse = new VulnDbResponse<T?>(responseModel, error: null);
            }
            catch (JsonException e)
            {
                vulnDbResponse = new VulnDbResponse<T?>(null, exception: e);

                #region Debugging code
                // DEBUGGING ONLY: FIXING SERIALIZATION ERROR
                // TODO: Remove debugging code
                if (Debugger.IsAttached)
                {
                    if (e.BytePositionInLine != null)
                    {
                        var responseContent = response.Content.ReadAsByteArrayAsync().Result;
                        var problemBytes = Encoding.ASCII.GetString(responseContent.Skip((int)(e.BytePositionInLine) - 10).Take(30).ToArray());
                    }
                }
                // END DEBUGGING SECTION
                #endregion
            }
            return vulnDbResponse;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>The call identifier url
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <typeparam name="T"></typeparam>The model type
        /// <typeparam name="TU"></typeparam>The options model type
        /// <returns></returns>
        sealed protected async Task<VulnDbResponse<T?>> GetInformationAsync<T, TU>(string url, TU? options = null)
            where T : class, IObjectInformations where TU : class, IObjectOptions 
        {
            var optionObject = Activator.CreateInstance<TU>();
            var reqUrl = options == null ? $@"{url}{optionObject}" : $@"{url}{options}";
            var response = await SendMessageAsync(reqUrl,  HttpMethod.Get);
            var vulnDbResponse = await GetVulnDbObject<T>(response);
            return vulnDbResponse;
        }
        
        /// <summary>
        /// Sends request and constructs url
        /// </summary>
        /// <param name="url"></param>The request url
        /// <param name="httpMethod"></param>The http method to use
        /// <param name="urlParams"></param>The optional url parameters to use
        /// <param name="payload"></param>The optional payload to attach
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendMessageAsync(string url, HttpMethod httpMethod, string urlParams = "",
            object? payload = null)
        {
            var reqUrl = $"{BaseUrl}{ApiVersionUrl}{url}";
            if (urlParams != "")
            {
                reqUrl = $"{BaseUrl}{ApiVersionUrl}{url}?{urlParams}";
            }
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
        #endregion
        
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}