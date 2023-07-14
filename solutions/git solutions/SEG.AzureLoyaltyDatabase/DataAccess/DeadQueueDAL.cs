////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\DeadQueueDAL.cs
//
// summary:	Implements the dead queue dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using Newtonsoft.Json;
using SEG.ApiService.Models.Queueing;
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
    /// <summary>   A dead queue dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DeadQueueDAL : DapperDalBase
    {
        #region Constants

        public const int QueueNameMaxSize = 255;    ///< Size of the queue name maximum
        public const int ErrorMaxSize = 500;    ///< Size of the error maximum

        #endregion Constants

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an error task to dead queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueueTask">   The error queue task. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task AddErrorTaskToDeadQueue(ErrorQueueTask errorQueueTask)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await AddErrorTaskToDeadQueue(errorQueueTask, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an error task to dead queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueueTask">   The error queue task. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task AddErrorTaskToDeadQueue(ErrorQueueTask errorQueueTask, IDbConnection connection, IDbTransaction transaction = null)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii };


            DeadTaskQueue dtQueue = new DeadTaskQueue
            {
                QueueName = errorQueueTask.QueueTask.QueueName.Length > QueueNameMaxSize ? errorQueueTask.QueueTask.QueueName.Substring(0, QueueNameMaxSize) : errorQueueTask.QueueTask.QueueName,
                QueueTask = JsonConvert.SerializeObject(errorQueueTask.QueueTask),
                Error = errorQueueTask.Error.Length > ErrorMaxSize ? errorQueueTask.Error.Substring(0, ErrorMaxSize) : errorQueueTask.Error,
                Exception = errorQueueTask.Exception
            };

            var sql = @"INSERT INTO [dbo].[DeadTaskQueue]
                               ([CreateDateTime]
                               ,[QueueName]
                               ,[Error]
                               ,[QueueTask]
                               ,[Exception])
                         VALUES
                               (getdate()
                               ,@QueueName
                               ,@Error
                               ,@QueueTask
                               ,@Exception)";

            await connection.ExecuteAsync(sql, dtQueue).ConfigureAwait(false);

        }


    }
}