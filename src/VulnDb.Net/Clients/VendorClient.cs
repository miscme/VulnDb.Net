#nullable enable
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Vendor;

namespace VulnDb.Net.Clients
{
    public sealed class VendorClient : APIClient, IVendorClient
    {
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
            var url = $@"vendors/{vendorId}";
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
            var url = $@"vendors/by_product_id?product_id={productId}&size={size}&page={page}";
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
            var url = $@"vendors/?size={size}&page={page}";
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
                $@"vendors/modified_vendors?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
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
                $@"vendors/new_vendors?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
        #endregion

    }
}