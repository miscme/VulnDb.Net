using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Classification;

namespace VulnDb.Net.Clients
{
    /// <summary>
    /// Returns 20 classifications ordered by name.
    /// </summary>
    /// <param name="options"></param>Optional parameter to indicate that you want to include each classification's type in the returned result
    /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
    /// <param name="page"></param>The page number
    /// <returns></returns>
    public interface IClassificationsClient
    {
        Task<VulnDbResponse<ClassificationInformations>> GetClassifications(ClassificationInformationOptions options = null, int size = 20, int page = 1);
    }
}