#nullable enable
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Version;

namespace VulnDb.Net.Clients
{
    public class VersionClient : APIClient, IVersionClient
    {
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
            var url = $@"versions/by_product_id?product_id={productId}&size={size}&page={page}";
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
            var url = $@"versions/by_product_name?product_name={productName}&size={size}&page={page}";
            var versionInformation = await GetInformationAsync<VersionInformations, VersionInformationOptions>(url, options);
            return versionInformation;
        }
        #endregion
    }
}