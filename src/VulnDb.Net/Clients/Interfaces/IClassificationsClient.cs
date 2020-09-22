using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Classification;

namespace VulnDb.Net.Clients
{
    public interface IClassificationsClient
    {
        Task<VulnDbResponse<ClassificationInformations>> GetClassifications(ClassificationInformationOptions options = null, int size = 20, int page = 1);
    }
}