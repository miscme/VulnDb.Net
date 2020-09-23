#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Vendor;

namespace VulnDb.Net.Clients
{
    public sealed class VendorClient : BaseClient, IVendorClient
    {
        public VendorClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByNameAsync(string vendorName,
            VendorInformationOptions? options = null)
        {
            var url = $@"vendors/by_name?vendor_name={vendorName}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByIdAsync(int vendorId,
            VendorInformationOptions? options = null)
        {
            var url = $@"vendors/{vendorId}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorByProductIdAsync(int productId,
            VendorInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vendors/by_product_id?product_id={productId}&size={size}&page={page}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorsAsync(VendorInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"vendors/?size={size}&page={page}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorModifiedAsync(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"vendors/modified_vendors?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }

        public async Task<VulnDbResponse<VendorInformations?>> GetVendorNewAsync(string startDate = "", string endDate = "",
            VendorInformationOptions? options = null, int size = 1, int page = 1)
        {
            var url =
                $@"vendors/new_vendors?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var vendorInformation = await GetInformationAsync<VendorInformations, VendorInformationOptions>(url, options);
            return vendorInformation;
        }
    }
}