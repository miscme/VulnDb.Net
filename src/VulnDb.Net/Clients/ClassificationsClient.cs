#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Classification;

namespace VulnDb.Net.Clients
{
    public class ClassificationsClient : BaseClient, IClassificationsClient
    {
        public ClassificationsClient(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<VulnDbResponse<ClassificationInformations?>> GetClassifications(
            ClassificationInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"classifications/?size={size}&page={page}";
            var classificationInformation = await GetInformationAsync<ClassificationInformations, ClassificationInformationOptions>(url, options);
            return classificationInformation;
        }
    }
}