using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VulnDb.Net
{
    public class VulnDbClient : IDisposable
    {
        private const string BaseUrl = "";
        private string ClientId { get; }
        private string ClientSecret { get; }

        public VulnDbClient(string clientId, string clientSecret)
            : this(url: BaseUrl, clientId, clientSecret)
        {
        }

        public VulnDbClient(string url, string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            var clientHandler = new HttpClientHandler();
            var cookieContainer = new CookieContainer();
            clientHandler.CookieContainer = cookieContainer;
            var client = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            client.DefaultRequestHeaders.Referrer = new Uri(url);
        }

        public async Task GetTokenAsync(string clientId, string clientSecret)
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}