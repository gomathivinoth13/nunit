using Dapper;
using SEG.ApiService.Models.Offers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class CampaignIssuanceCountDAL : DapperDalBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="campaignID"></param>
        /// <returns></returns>
        public static async Task<Int64> getCampaignIssuanceCount(string campaignID)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await getCampaignIssuanceCount(db, campaignID).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="campaignID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<Int64> getCampaignIssuanceCount(IDbConnection connection, string campaignID, IDbTransaction transaction = null)
        {
            var result = (await connection.QueryAsync<Int64>("Select top 1 IssuanceCount from CampaignIssuanceCount where cast(@CampaignID as varchar(100)) = campaignID ", new { campaignID }, transaction).ConfigureAwait(false)).ToList();

            return result.FirstOrDefault();

        }


    }
}
