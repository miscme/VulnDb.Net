using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Version;

namespace VulnDb.Net.Clients
{
    public interface IVersionClient
    {
        Task<VulnDbResponse<VersionInformations>> GetVersionByProductIdAsync(int productId, VersionInformationOptions options = null, int size = 20, int page = 1);
        Task<VulnDbResponse<VersionInformations>> GetVersionByProductNameAsync(string productName, VersionInformationOptions options = null, int size = 20, int page = 1);
    }
}