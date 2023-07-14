////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\LoggingDAL.cs
//
// summary:	Implements the logging dal class
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
    /// <summary>   A logging dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LoggingDAL : DapperDalBase
    {

        private const int MaxMessageLength = 500;   ///< The maximum message length
        private const int MaxMachineLength = 255;   ///< The maximum machine length
        private const int MaxThreadLength = 255;    ///< The maximum thread length
        private const int MaxLevelLength = 50;  ///< The maximum level length
        private const int MaxLoggerLength = 255;    ///< The maximum logger length
        private const int MaxApiTransactionIdLength = 255;  ///< The maximum API transaction identifier length

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets azure log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="messageId">    Identifier for the message. </param>
        ///
        /// <returns>   An asynchronous result that yields the azure log. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureLog> GetAzureLog(Guid messageId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetAzureLog(messageId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets azure log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="messageId">    Identifier for the message. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the azure log. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureLog> GetAzureLog(Guid messageId, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<AzureLog>("SELECT * FROM dbo.[Log] (nolock) WHERE [MessageId] = @MessageId", new { messageId }, transaction: transaction).ConfigureAwait(false)).SingleOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="log">              The log. </param>
        /// <param name="checkIfExists">    True to check if exists. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveLog(AzureLog log, bool checkIfExists )
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveLog(log, checkIfExists, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="log">              The log. </param>
        /// <param name="checkIfExists">    True to check if exists. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveLog(AzureLog log, bool checkIfExists, IDbConnection connection, IDbTransaction transaction = null)
        {

            AzureLog dbLog = null;

            if (checkIfExists)
            {
                //dbLog = context.Logs.Where(x => x.MessageId == log.MessageId).FirstOrDefault();
                dbLog = await GetAzureLog(log.MessageId, connection, transaction).ConfigureAwait(false);
            }

            if (dbLog == null)
            {
                if (log.Message.Length > MaxMessageLength)
                {
                    log.Message = log.Message.Substring(0, MaxMessageLength - 1);
                }
                if (log.Machine.Length > MaxMachineLength)
                {
                    log.Machine = log.Machine.Substring(0, MaxMachineLength - 1);
                }
                if (log.Thread.Length > MaxThreadLength)
                {
                    log.Thread = log.Thread.Substring(0, MaxThreadLength - 1);
                }
                if (log.Level.Length > MaxLevelLength)
                {
                    log.Level = log.Level.Substring(0, MaxLevelLength - 1);
                }
                if (log.Logger.Length > MaxLoggerLength)
                {
                    log.Logger = log.Logger.Substring(0, MaxLoggerLength - 1);
                }
                if (log.ApiTransactionId != null && log.ApiTransactionId.Length > MaxApiTransactionIdLength)
                {
                    log.ApiTransactionId = log.ApiTransactionId.Substring(0, MaxApiTransactionIdLength - 1);
                }

                string sql = @"INSERT INTO [dbo].[Log]
                                       ([MessageId]
                                       ,[CreateDateTime]
                                       ,[Machine]
                                       ,[Thread]
                                       ,[Level]
                                       ,[Logger]
                                       ,[ApiTransactionId]
                                       ,[Message]
                                       ,[Exception]
                                       ,[Request]
                                       ,[Response])
                                 VALUES
                                       (@MessageId
                                       ,@CreateDateTime
                                       ,@Machine
                                       ,@Thread
                                       ,@Level
                                       ,@Logger
                                       ,@ApiTransactionId
                                       ,@Message
                                       ,@Exception
                                       ,@Request
                                       ,@Response)";

                await connection.ExecuteAsync(sql, log);

            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the external log records by date described by deleteDate. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteExternalLogRecordsByDate(DateTime deleteDate)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await DeleteExternalLogRecordsByDate(deleteDate, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the external log records by date. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteExternalLogRecordsByDate(DateTime deleteDate, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteAsync("DELETE FROM dbo.Log WHERE CreateDateTime < @DeleteDate", new SqlParameter("@DeleteDate", deleteDate));
        }
    }
}
