#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VulnDb.Net.Models;
using VulnDb.Net.Models.Information;

namespace VulnDb.Net.Clients
{
    public class AccountClient : BaseClient
    {
        public AccountClient(HttpClient httpClient) : base(httpClient)
        {
        }
        #region General API Information
        /// <summary>
        /// Returns status information on your user account,
        /// including: organization name, username, e-mail address,
        /// subscription end date, maximum number of allowed API calls per month,
        /// API calls made for the current month and general VulnDB statistics. 
        /// </summary>
        /// <returns>Account Model</returns>
        public async Task<VulnDbResponse<Account?>> GetAccountStatusAsync()
        {
            var response = await SendMessageAsync(_httpClient, "account_status", HttpMethod.Get);
            var vulnDbResponse = await GetVulnDbObject<Account>(response);
            return vulnDbResponse;
        }

        /// <summary>
        /// Returns vendors, products and versions that have been merged within a date range.
        /// The start_date(ISO 8601) parameter is required but the end_date is parameter is optional.
        /// If the end_date(ISO 8601) parameter is omitted,
        /// the records between start_date and the current date will be returned. 
        /// </summary>
        /// <param name="startDate">The start date (UTC), defaults to 10 years before today's d</param>
        /// <param name="endDate">The end date (UTC), defaults to today's date</param>
        /// <param name="size">The number of merged records to attempt returning, defaults to 20</param>
        /// <param name="page">The page number</param>
        /// <returns>MergedIds Model</returns>
        public async Task<VulnDbResponse<MergedIds?>> GetMergedIdsAsync(string startDate = "", string endDate = "", int size = 20, int page = 1)
        {
            var urlParams = $@"start_date={startDate}&end_date={endDate}&size={size}&page={page}";
            var response = await SendMessageAsync(_httpClient, "get_merged_ids", HttpMethod.Get, urlParams);
            var vulnDbResponse = await GetVulnDbObject<MergedIds>(response);
            return vulnDbResponse;
        }
        #endregion

    }
}
