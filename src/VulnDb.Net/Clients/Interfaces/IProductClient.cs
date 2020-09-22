using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Product;

namespace VulnDb.Net.Clients
{
    public interface IProductClient
    {
        Task<VulnDbResponse<ProductInformations>> GetProductByIdAndProductNameAsync(string productName, int vendorId, ProductInformationOptions options = null);
        Task<VulnDbResponse<ProductInformations>> GetProductByIdAndProductNameAsync(string productName, ProductInformationOptions options = null);
        Task<VulnDbResponse<ProductInformations>> GetProductByVendorIdAsync(int vendorId, ProductInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<ProductInformations>> GetProductByVendorNameAsync(string vendorName, ProductInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<ProductInformations>> GetProductsAsync(ProductInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<ProductInformations>> GetProductsByIdAsync(int productId, ProductInformationOptions options = null);
        Task<VulnDbResponse<ProductInformations>> GetProductsModifiedAsync(string startDate = "", string endDate = "", ProductInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<ProductInformations>> GetProductsNewAsync(string startDate = "", string endDate = "", ProductInformationOptions options = null, int size = 20, int page = 1);
    }
}