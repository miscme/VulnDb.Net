using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Vendor;

namespace VulnDb.Net.Clients
{
    public interface IVendorClient
    {
        Task<VulnDbResponse<VendorInformations>> GetVendorByIdAsync(int vendorId, VendorInformationOptions options = null);
        Task<VulnDbResponse<VendorInformations>> GetVendorByNameAsync(string vendorName, VendorInformationOptions options = null);
        Task<VulnDbResponse<VendorInformations>> GetVendorByProductIdAsync(int productId, VendorInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<VendorInformations>> GetVendorModifiedAsync(string startDate = "", string endDate = "", VendorInformationOptions options = null, int size = 1, int page = 1);
        Task<VulnDbResponse<VendorInformations>> GetVendorNewAsync(string startDate = "", string endDate = "", VendorInformationOptions options = null, int size = 1, int page = 1);
        Task<VulnDbResponse<VendorInformations>> GetVendorsAsync(VendorInformationOptions options = null, int size = 20, int page = 1);
    }
}