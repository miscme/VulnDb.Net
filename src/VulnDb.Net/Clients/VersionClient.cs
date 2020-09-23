#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Version;

namespace VulnDb.Net.Clients
{
    public class VersionClient : BaseClient, IVersionClient
    {
        public VersionClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<VulnDbResponse<VersionInformations?>> GetVersionByProductIdAsync(int productId, VersionInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"versions/by_product_id?product_id={productId}&size={size}&page={page}";
            var versionInformation = await GetInformationAsync<VersionInformations, VersionInformationOptions>(url, options);
            return versionInformation;
        }

        public async Task<VulnDbResponse<VersionInformations?>> GetVersionByProductNameAsync(string productName, VersionInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"versions/by_product_name?product_name={productName}&size={size}&page={page}";
            var versionInformation = await GetInformationAsync<VersionInformations, VersionInformationOptions>(url, options);
            return versionInformation;
        }
    }
}