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
    public class MBOCongratsDataDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets MBOCongratsData. </summary>
        ///
        /// <remarks>   Mark Robinson 7/28/2020. </remarks>
        ///
        ///
        /// <returns>
        /// An asynchronous result that yields the MBOCongratsData.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<List<MBOCongratsData>> GetMBOCongratsData(string memberID)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))

            return await GetMBOCongratsData(memberID, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Gets MBOCongratsData. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="memberID">      </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields MBOCongratsData.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<List<MBOCongratsData>> GetMBOCongratsData(string memberID, IDbConnection connection, IDbTransaction transaction = null)
        {
            var results = await connection.QueryAsync<MBOCongratsData>(@"Select * from [dbo].[CustomerMBOCongratsData]
                 WHERE MemberID = cast(@memberId as varchar(36)) AND CreatedDate >= DATEADD(MONTH, -1, GETDATE())", new { memberID }, transaction).ConfigureAwait(false);
            return results.ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets MBOCongratsData. </summary>
        ///
        /// <remarks>   Mark Robinson 7/28/2020. </remarks>
        ///
        ///
        /// <returns>
        /// An asynchronous result that yields the MBOCongratsData.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task InsertMBOCongratsData(MBOCongratsData mboCongratsData)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
              await InsertMBOCongratsData(mboCongratsData, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Gets MBOCongratsData. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="mboCongratsData">      </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields MBOCongratsData.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task InsertMBOCongratsData(MBOCongratsData mboCongratsData, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<bool>(@"INSERT INTO CustomerMBOCongratsData (MemberID, MBOLastUpdatedDate, CampaignID, CreatedDate)
            VALUES (@MemberID, @MBOLastUpdatedDate, @CampaignID, @CreatedDate)", mboCongratsData , transaction).ConfigureAwait(false);
        }
    }
}
