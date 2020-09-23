#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Product;

namespace VulnDb.Net.Clients
{
    public class ProductClient : BaseClient, IProductClient
    {
        public ProductClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductByVendorIdAsync(int vendorId,
            ProductInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"vendors/by_vendor_id?vendor_id={vendorId}&size={size}&page={page}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductByIdAndProductNameAsync(string productName,
            int vendorId, ProductInformationOptions? options = null)
        {
            var url = $@"vendors/by_vendor_id_and_product_name?product_name={productName}&vendor_id={vendorId}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductByIdAndProductNameAsync(string productName,
            ProductInformationOptions? options = null)
        {
            var url = $@"vendors/by_vendor_id_and_product_name?product_name={productName}&vendor_id={""}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductByVendorNameAsync(string vendorName,
            ProductInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"products/by_vendor_name?vendor_name={vendorName}?size={size}&page={page}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductsAsync(ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/?size={size}&page={page}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductsModifiedAsync(string startDate = "", string endDate = "", ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/modified_products?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductsNewAsync(string startDate = "", string endDate = "", ProductInformationOptions? options = null,
            int size = 20, int page = 1)
        {
            var url = $@"products/new_products?start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }

        public async Task<VulnDbResponse<ProductInformations?>> GetProductsByIdAsync(int productId, ProductInformationOptions? options = null)
        {
            var url = $@"products/{productId}";
            var productInformation = await GetInformationAsync<ProductInformations, ProductInformationOptions>(url, options);
            return productInformation;
        }
    }
}