#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private async Task<VulnDbResponse<T?>> GetVulnDbObject<T>(HttpResponseMessage response) where T:class
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
            var responseModel = await response.Content.ReadFromJsonAsync<T?>();
            vulnDbResponse = new VulnDbResponse<T?>(responseModel, null);
            return vulnDbResponse;
        }
        
        #region General API Information
        /// <summary>
        /// GET /api/v1/account_status:
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
        /// GET /api/v1/get_merged_ids?start_date=:start_date&end_date=:end_date:
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
        public async Task<VulnDbResponse<MergedIds?>> GetMergedIds(string startDate = "", string endDate = "", int size = 20, int page = 1)
        {
            var urlParams = $@"start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var response = await SendMessageAsync("get_merged_ids", HttpMethod.Get, urlParams);
            var vulnDbResponse = await GetVulnDbObject<MergedIds>(response);
            return vulnDbResponse;
        }
        #endregion

        #region Pulling Vendor Information
        private async Task<VulnDbResponse<VendorInformations?>> GetVendorInformation(string url, VendorInformationOptions? options = null)
        {
            var reqUrl = $@"vendors/{url}";
            if (options == null)
            {
                var vendorInformationOptions = new VendorInformationOptions();
                reqUrl = $@"{reqUrl}{vendorInformationOptions}";
            }
            var response = await SendMessageAsync(reqUrl,  HttpMethod.Get);
            var vulnDbResponse = await GetVulnDbObject<VendorInformations>(response);
            return vulnDbResponse;
        }
        
        /// <summary>
        /// Returns max 5 results of vendors search by name.
        /// </summary>
        /// <param name="vendorName"></param>The vendor name to search for. Required
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorInformationByName(string vendorName,
            VendorInformationOptions? options = null)
        {
            var url = $@"by_name?vendor_name={vendorName}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the vendors associated with a given product id.
        /// </summary>
        /// <param name="vendorId"></param>The product id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The page number
        /// <param name="page"></param>The number of vendors to attempt returning, defaults to 20
        /// <returns></returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorInformationById(int vendorId,
            VendorInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"{vendorId.ToString()}&{size.ToString()}&{page.ToString()}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the vendors associated with a given product id.
        /// </summary>
        /// <param name="productId"></param>The product id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorInformationByProductId(int productId,
            VendorInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"by_product_id?product_id={productId.ToString()}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns 20 vendors ordered by name.
        /// </summary>
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorInformationAll(VendorInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"?size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the updated vendors within a date range.
        /// Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results.
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 1
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorModified(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"modified_vendors?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the newly created vendors within a date range.
        /// Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results.
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 1
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorNew(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"new_vendors?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetVendorInformation(url, options);
            return vendorInformation;
        }
        #endregion
        #endregion
        
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
        
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}