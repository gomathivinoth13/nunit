////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\ErrorQueueDAL.cs
//
// summary:	Implements the error queue dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error queue dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class ErrorQueueDAL : DapperDalBase
    {
        #region Public Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an error queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueueLog">    The error queue log. </param>
        ///
        /// <returns>   An asynchronous result that yields an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<int> SaveErrorQueue(ErrorQueueLog errorQueueLog)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await SaveErrorQueue(errorQueueLog, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an error queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueueLog">    The error queue log. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<int> SaveErrorQueue(ErrorQueueLog errorQueueLog, IDbConnection connection, IDbTransaction transaction = null)
        {

            string sqlQuery = "";
            if (errorQueueLog.ErrorQueueLogId > 0)
            {
                ErrorQueueLog dbErrorQueue = await GetErrorQueueLog(errorQueueLog.ErrorQueueLogId).ConfigureAwait(false);
                if (dbErrorQueue != null)
                {
                    dbErrorQueue.CompleteDateTime = errorQueueLog.CompleteDateTime;
                    dbErrorQueue.MessageReprocessCount = errorQueueLog.MessageReprocessCount;
                    dbErrorQueue.MessageMaxErrorLimitCount = errorQueueLog.MessageMaxErrorLimitCount;
                }
                sqlQuery = @"UPDATE [dbo].[ErrorQueueLog]
                                   SET  [CompleteDateTime] = @CompleteDateTime
                                       ,[MessageReprocessCount] = @MessageReprocessCount
                                       ,[MessageMaxErrorLimitCount] = @MessageMaxErrorLimitCount
                                 WHERE ErrorQueueLogId = @ErrorQueueLogId;
                            SELECT @ErroRQueueLogId
";

            }
            else
            {
                sqlQuery = @"INSERT INTO [dbo].[ErrorQueueLog]
                                       ([StartDateTime]
                                       ,[CompleteDateTime]
                                       ,[MessageReprocessCount]
                                       ,[MessageMaxErrorLimitCount])
                                 VALUES
                                       (@StartDateTime
                                       ,@CompleteDateTime
                                       ,@MessageReprocessCount
                                       ,@MessageMaxErrorLimitCount);
                            SELECT SCOPE_IDENTITY()";

            }

            return await connection.ExecuteScalarAsync<int>(sqlQuery, errorQueueLog, transaction: transaction).ConfigureAwait(false);

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets error queue log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="ErrorQueueLogId">  Identifier for the error queue log. </param>
        ///
        /// <returns>   An asynchronous result that yields the error queue log. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<ErrorQueueLog> GetErrorQueueLog(int ErrorQueueLogId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetErrorQueueLog(ErrorQueueLogId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets error queue log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="ErrorQueueLogId">  Identifier for the error queue log. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the error queue log. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<ErrorQueueLog> GetErrorQueueLog(int ErrorQueueLogId, IDbConnection connection, IDbTransaction transaction = null)
        {

            return (await connection.QueryAsync<ErrorQueueLog>("SELECT * FROM dbo.[ErrorQueueLog] (nolock) WHERE [ErrorQueueLogId] = @ErrorQueueLogId", new { ErrorQueueLogId }, transaction: transaction).ConfigureAwait(false)).SingleOrDefault();

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets error code. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorCode">    The error code. </param>
        ///
        /// <returns>   An asynchronous result that yields the error code. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<ErrorCode> GetErrorCode(string errorCode)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetErrorCode(errorCode, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets error code. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorCode">    The error code. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the error code. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<ErrorCode> GetErrorCode(string errorCode, IDbConnection connection, IDbTransaction transaction = null)
        {

            return (await connection.QueryAsync<ErrorCode>("SELECT * FROM dbo.ErrorCode (nolock) WHERE CODE = @errorCode", new { errorCode }, transaction: transaction).ConfigureAwait(false)).SingleOrDefault();
        }

        #endregion Public Methods
    }
}