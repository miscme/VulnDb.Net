#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Authentication;
using VulnDb.Net.Models.Classification;
using VulnDb.Net.Models.Information;
using VulnDb.Net.Models.Product;
using VulnDb.Net.Models.Vendor;
using VulnDb.Net.Models.Version;
using VulnDb.Net.Models.Vulnerability;

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
            var urlParams = $@"start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var response = await SendMessageAsync("get_merged_ids", HttpMethod.Get, urlParams);
            var vulnDbResponse = await GetVulnDbObject<MergedIds>(response);
            return vulnDbResponse;
        }
        #endregion

        #region Pulling Vendor Information
        /// <summary>
        /// Returns max 5 results of vendors search by name.
        /// </summary>
        /// <param name="vendorName"></param>The vendor name to search for. Required
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByNameAsync(string vendorName,
            VendorInformationOptions? options = null)
        {
            var url = $@"vendors/by_name?vendor_name={vendorName}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns vendor by id. Http status code 404 will be returned if no vendor matches the vendor_id.
        /// </summary>
        /// <param name="vendorId"></param>The vendor id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByIdAsync(int vendorId,
            VendorInformationOptions? options = null)
        {
            var url = $@"vendors/{vendorId.ToString()}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the vendors associated with a given product id.
        /// </summary>
        /// <param name="productId"></param>The product id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByProductIdAsync(int productId,
            VendorInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vendors/by_product_id?product_id={productId.ToString()}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns 20 vendors ordered by name.
        /// </summary>
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorsAsync(VendorInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"vendors/?size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the updated vendors within a date range.
        /// Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results.
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 1
        /// <param name="page"></param>The page number
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorModifiedAsync(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"vendors/modified_vendors?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        
        /// <summary>
        /// Returns all the newly created vendors within a date range.
        /// Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results.
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 1
        /// <param name="page"></param>The page number
        /// <returns>VendorInformations Model</returns>
        public async Task<VulnDbResponse<VendorInformations?>> GetVendorNewAsync(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"vendors/new_vendors?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        #endregion

        #region Pulling Product Information
        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="vendorId"></param>The vendor id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductByVendorIdAsync(int vendorId,
            ProductInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vendors/by_vendor_id?vendor_id={vendorId.ToString()}&size={size.ToString()}&page={page.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        /// <summary>
        /// Returns max 5 products ordered by name.
        /// </summary>
        /// <param name="productName"></param>The product name to search for
        /// <param name="vendorId"></param>The vendor id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductByIdAndProductNameAsync(string productName,
            int vendorId, ProductInformationOptions? options = null)
        {
            var url = $@"vendors/by_vendor_id_and_product_name?product_name={productName}&vendor_id={vendorId.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        
        /// <summary>
        /// Returns max 5 products ordered by name.
        /// </summary>
        /// <param name="productName"></param>The product name to search for
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductByIdAndProductNameAsync(string productName,
            ProductInformationOptions? options = null)
        {
            var url = $@"vendors/by_vendor_id_and_product_name?product_name={productName}&vendor_id={""}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="vendorName"></param>The vendor name. Required parameter
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number. Defaults to 1.
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductByVendorNameAsync(string vendorName,
            ProductInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"products/by_vendor_name?vendor_name={vendorName}?size={size.ToString()}&page={page.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        
        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductsAsync(ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/?size={size.ToString()}&page={page.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        
        /// <summary>
        /// Returns all the updated products within a date range. Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductsModifiedAsync(string startDate = "", string endDate = "", ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/modified_products?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        
        /// <summary>
        /// Returns all the newly created products within a date range. Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductsNewAsync(string startDate = "", string endDate = "", ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/new_products?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        
        /// <summary>
        /// Returns product by id.
        /// </summary>
        /// <param name="productId"></param>The product id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        public async Task<VulnDbResponse<ProductInformations?>> GetProductsByIdAsync(int productId, ProductInformationOptions? options = null)
        {
            var url = $@"products/{productId.ToString()}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
        #endregion

        #region Pulling Version Information
        /// <summary>
        /// Returns 20 versions ordered by name.
        /// </summary>
        /// <param name="productId"></param>The product id
        /// <param name="options"></param>If results returned should only be for vulnerable versions - value: true/false - defaults to true
        /// <param name="size"></param>The number of versions to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VersionInformations?>> GetVersionByProductIdAsync(int productId, VersionInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"versions/by_product_id?product_id={productId.ToString()}&size={size.ToString()}&page={page.ToString()}";
            var versionInformation = await GetInformationAsync<VersionInformations, VersionInformationOptions>(url, options);
            return versionInformation;
        }

        /// <summary>
        /// Returns 20 versions ordered by name.
        /// </summary>
        /// <param name="productName"></param>The product name
        /// <param name="options"></param>If results returned should only be for vulnerable versions - value: true/false - defaults to true
        /// <param name="size"></param>The number of versions to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<VersionInformations?>> GetVersionByProductNameAsync(string productName, VersionInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"versions/by_product_name?product_name={productName}&size={size.ToString()}&page={page.ToString()}";
            var versionInformation = await GetInformationAsync<VersionInformations, VersionInformationOptions>(url, options);
            return versionInformation;
        }
        #endregion

        #region Pulling classifications information
        /// <summary>
        /// Returns 20 classifications ordered by name.
        /// </summary>
        /// <param name="options"></param>Optional parameter to indicate that you want to include each classification's type in the returned result
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ClassificationInformations?>> GetClassifications(
            ClassificationInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"classifications/?size={size.ToString()}&page={page.ToString()}";
            var classificationInformation = await GetInformationAsync<ClassificationInformations, ClassificationInformationOptions>(url, options);
            return classificationInformation;
        }
        #endregion

        #region Pulling Vulnerability Information
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesAllMetasploit(VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/find_all_metasploit?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByBugtraqId(int bugtraqId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{bugtraqId.ToString()}/find_by_bugtraq_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByCertvuId(int certvuId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{certvuId.ToString()}/find_by_certvu_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByClassificationId(int classificationId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{classificationId.ToString()}/find_by_classification_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByClassificationIds(IEnumerable<int> classificationIds, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var urlIds = string.Join(",", classificationIds.Select(x => x.ToString()).ToArray());
            var url = $@"vulnerabilities/{urlIds}/find_by_classification_ids?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByCpeId(string cpe, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/find_by_cpe?page={page.ToString()}&size={size.ToString()}&cpe={cpe}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByCveId(int cveId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{cveId.ToString()}/find_by_cve_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByDate(string startDate = "", string endDate = "", VulnerabilityInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"/vulnerabilities/find_by_date?start_date={startDate}&end_date={endDate}&size={size.ToString()}&page={page.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByExploitDbId(int exploitDbId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{exploitDbId.ToString()}/find_by_exploitdb_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByIssId(int issId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{issId.ToString()}/find_by_iss_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        
        public async Task<VulnDbResponse<VulnerabilityInformations?>> GetVulnerabilitiesFindByMilwormId(int milwormId, VulnerabilityInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vulnerabilities/{milwormId.ToString()}/find_by_milworm_id?page={page.ToString()}&size={size.ToString()}";
            var vulnerabilityInformation =
                await GetInformationAsync<VulnerabilityInformations, VulnerabilityInformationOptions>(url, options);
            return vulnerabilityInformation;
        }
        

        #endregion
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
                var serializer = new JsonSerializerOptions {IgnoreNullValues = true};
                var responseModel = await response.Content.ReadFromJsonAsync<T?>(serializer);
                vulnDbResponse = new VulnDbResponse<T?>(responseModel, error: null);
            }
            catch (JsonException e)
            {
                vulnDbResponse = new VulnDbResponse<T?>(null, exception: e);
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
        private async Task<VulnDbResponse<T?>> GetInformationAsync<T, TU>(string url, TU? options = null)
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