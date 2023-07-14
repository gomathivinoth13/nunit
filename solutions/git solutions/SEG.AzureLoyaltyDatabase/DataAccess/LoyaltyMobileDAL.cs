using Dapper;
using SEG.ApiService.Models.MobileFirst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    public class LoyaltyMobileDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets app version by . </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="chainId"> Identifier for the device. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<int> GetAppVersion(string chainId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetAppVersion(chainId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets app version by . </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="chainId"> Identifier for the device. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<int> GetAppVersion(string chainId, IDbConnection connection, IDbTransaction transaction = null)
        {
            return await connection.ExecuteScalarAsync<int>("Select Version from AppVersion where ChainID = @chainId", new { chainId }, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets app version by . </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="chainId"> Identifier for the device. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AppVersionV2Response>> GetAppVersionV2()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetAppVersionV2(db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets app version by . </summary>amc
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="chainId"> Identifier for the device. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AppVersionV2Response>> GetAppVersionV2(IDbConnection connection, IDbTransaction transaction = null)
        {
            var result = await connection.QueryAsync<AppVersionV2Response>(@"Select ChainId, AndroidMinimumVersion, AndroidStoreUrl,
                iOSMinimumVersion, iOSStoreUrl, AndroidAppName, iOSAppName, iOSLatestVersion, AndroidLatestVersion from AppVersion", transaction).ConfigureAwait(false);
            return result.ToList();
        }
    }
}
