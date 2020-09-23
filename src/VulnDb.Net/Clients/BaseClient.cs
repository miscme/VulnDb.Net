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
    public abstract class BaseClient : IDisposable
    {
        public static string BaseUrl { get; set; } = "https://vulndb.cyberriskanalytics.com/"; // TODO: Code set part to check for trailing / and add if not existent
        public static string ApiVersionUrl { get; set; } = "api/v1/";

        protected readonly HttpClient _httpClient;
        
        public BaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        #region API Request Helper Methods
        /// <summary>
        /// Creates the meta response object,
        /// created to provide a consistent return type and encapsulate possible errors.
        /// </summary>
        /// <param name="response"></param>Response from the request to VulnDb
        /// <typeparam name="T"></typeparam>Type of the response model
        /// <returns>Meta VulnDb Response Object</returns>
        protected async Task<VulnDbResponse<T?>> GetVulnDbObject<T>(HttpResponseMessage response)
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
        protected async Task<VulnDbResponse<T?>> GetInformationAsync<T, TU>(string url, TU? options = null)
            where T : class, IObjectInformations where TU : class, IObjectOptions 
        {
            var optionObject = Activator.CreateInstance<TU>();
            var reqUrl = options == null ? $@"{url}{optionObject}" : $@"{url}{options}";
            var response = await SendMessageAsync(_httpClient, reqUrl,  HttpMethod.Get);
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
        internal static async Task<HttpResponseMessage> SendMessageAsync(HttpClient httpClient, string url, HttpMethod httpMethod, string urlParams = "",
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
            return await httpClient.SendAsync(request);
        }
        #endregion
        
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}