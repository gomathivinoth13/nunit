using Dapper;
using SEG.ApiService.Models.MobileFirst;
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
    public class AppReviewDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Mark RObinson 7/28/2020. </remarks>
        ///
        /// <param name="memberid">  </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by application setting
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<AppReviewStatusRequest> SearchAppReviewStatus(string memberId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))

            return await SearchAppReviewStatus(memberId, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the customer review for the customer. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="memberId">     memberid for the customer. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<AppReviewStatusRequest> SearchAppReviewStatus(string memberId, IDbConnection connection, IDbTransaction transaction = null)
        {
            var results = (await connection.QueryAsync<AppReviewStatusRequest>("Select [MemberID],[LastPromptDate],[LastUpdatedDateTime],[CreatedDatetime],[LastUpdatedSource] from [dbo].[CustomerReview] (nolock) where MemberID = cast(@memberId as varchar(36))", new { memberId }, transaction).ConfigureAwait(false));
            return results.FirstOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Mark RObinson 7/28/2020. </remarks>
        ///
        /// <param name="appReviewStatusRequest">  Template key. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by application setting
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task UpdateAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
                await UpdateAppReviewStatus(appReviewStatusRequest, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets byapplication setting. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="appReviewStatusRequest">     Identifier for the template. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task UpdateAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest, IDbConnection connection, IDbTransaction transaction = null)
        {
            string updateQuery = @"UPDATE[dbo].[CustomerReview] SET[LastPromptDate] = @LastPromptDate, LastUpdatedDateTime = @LastUpdatedDateTime, LastUpdatedSource = @LastUpdatedSource WHERE MemberID = cast(@memberId as varchar(36))";

            await connection.ExecuteAsync(updateQuery, appReviewStatusRequest, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Harishma P 6/28/2023. </remarks>
        ///
        /// <param name="appReviewStatusRequest">  Template key. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by application setting
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task SaveAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
                await SaveAppReviewStatus(appReviewStatusRequest, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets byapplication setting. </summary>
        ///
        /// <remarks>   Harishma P 6/28/2023. </remarks>
        ///
        /// <param name="appReviewStatusRequest">     Identifier for the template. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task SaveAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest, IDbConnection connection, IDbTransaction transaction = null)
        {
            string insertQuery = @"INSERT INTO [dbo].[CustomerReview](MemberID,LastPromptDate,LastUpdatedDateTime,CreatedDatetime,LastUpdatedSource)
            VALUES(@MemberID,@LastPromptDate,@LastUpdatedDateTime,@CreatedDatetime,@LastUpdatedSource)";

            await connection.ExecuteAsync(insertQuery, appReviewStatusRequest, transaction).ConfigureAwait(false);
        }

    }
}
