#nullable enable
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Classification;

namespace VulnDb.Net.Clients
{
    public class ClassificationsClient : APIClient, IClassificationsClient
    {
        #region Pulling Classifications information
        /// <summary>
        /// Returns 20 classifications ordered by name.
        /// </summary>
        /// <param name="options"></param>Optional parameter to indicate that you want to include each classification's type in the returned result
        /// <param name="size"></param>The number of vendors to attempt returning, defaults to 20
        /// <param name="page"></param>The page number
        /// <returns></returns>
        public async Task<VulnDbResponse<ClassificationInformations?>> GetClassifications(
            ClassificationInformationOptions? options = null, int size = 20, int page = 1)
        {
            var url = $@"classifications/?size={size}&page={page}";
            var classificationInformation = await GetInformationAsync<ClassificationInformations, ClassificationInformationOptions>(url, options);
            return classificationInformation;
        }
        #endregion
    }
}