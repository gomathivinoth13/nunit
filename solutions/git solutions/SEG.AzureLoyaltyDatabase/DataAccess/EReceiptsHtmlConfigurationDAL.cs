using Dapper;
using SEG.LoyaltyDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class EReceiptsHtmlConfigurationDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Mark RObinson 7/28/2020. </remarks>
        ///
        /// <param name="key">  Template key. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by application setting
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<EReceiptsHtmlConfiguration> GetEReceiptsHtmlConfiguration(string key)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))

                return await GetEReceiptsHtmlConfiguration(key, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets byapplication setting. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="key">     Identifier for the template. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<EReceiptsHtmlConfiguration> GetEReceiptsHtmlConfiguration(string key, IDbConnection connection, IDbTransaction transaction = null)
        {
            var results = (await connection.QueryAsync<EReceiptsHtmlConfiguration>("Select * from [dbo].[EReceiptsHtmlConfiguration] (nolock)  where ConfigurationKey =  @key", new { key }, transaction).ConfigureAwait(false));
            return results.FirstOrDefault();
        }
    }
}
