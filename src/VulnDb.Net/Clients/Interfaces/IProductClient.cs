using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Product;

namespace VulnDb.Net.Clients
{
    public interface IProductClient
    {
        /// <summary>
        /// Returns max 5 products ordered by name.
        /// </summary>
        /// <param name="productName"></param>The product name to search for
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductByIdAndProductNameAsync(string productName, int vendorId, ProductInformationOptions options = null);

        /// <summary>
        /// Returns max 5 products ordered by name.
        /// </summary>
        /// <param name="productName"></param>The product name to search for
        /// <param name="vendorId"></param>The vendor id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductByIdAndProductNameAsync(string productName, ProductInformationOptions options = null);

        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="vendorId"></param>The vendor id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductByVendorIdAsync(int vendorId, ProductInformationOptions options = null, int size = 20, int page = 1);

        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="vendorName"></param>The vendor name. Required parameter
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number. Defaults to 1.
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductByVendorNameAsync(string vendorName, ProductInformationOptions options = null, int size = 20, int page = 1);

        /// <summary>
        /// Returns 20 products ordered by name.
        /// </summary>
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductsAsync(ProductInformationOptions options = null, int size = 20, int page = 1);

        /// <summary>
        /// Returns product by id.
        /// </summary>
        /// <param name="productId"></param>The product id
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductsByIdAsync(int productId, ProductInformationOptions options = null);

        /// <summary>
        /// Returns all the updated products within a date range. Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductsModifiedAsync(string startDate = "", string endDate = "", ProductInformationOptions options = null, int size = 20, int page = 1);

        /// <summary>
        /// Returns all the newly created products within a date range. Set the start_date parameter to be 2015-12-1 or later to receive the most accurate results
        /// </summary>
        /// <param name="startDate"></param>The start date (UTC), defaults to 1 week ago
        /// <param name="endDate"></param>The end date (UTC), defaults to today
        /// <param name="options"></param>These options can be passed to any call to change its behavior
        /// <param name="size"></param>The number of products to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        Task<VulnDbResponse<ProductInformations>> GetProductsNewAsync(string startDate = "", string endDate = "", ProductInformationOptions options = null, int size = 20, int page = 1);
    }
}