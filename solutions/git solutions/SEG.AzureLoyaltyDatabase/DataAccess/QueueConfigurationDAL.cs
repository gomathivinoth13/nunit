////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\QueueConfigurationDAL.cs
//
// summary:	Implements the queue configuration dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using SEG.ApiService.Models.Database;
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
    /// <summary>   A queue configuration dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class QueueConfigurationDAL : DapperDalBase
    {
        private const string UpsertSQL = @"IF EXISTS (Select Id from [CustomerBannerMetadata] (nolock) where [MemberId] = @MemberId and [ChainId] =@ChainID)  
                                           UPDATE [dbo].[CustomerBannerMetadata]
                                                SET [CouponAlias] = @CouponAlias
                                                    ,[CouponId] = @CouponId
                                                    ,[ShoppingListId] = @ShoppingListId
                                                    ,[StoreNumber] = @StoreNumber
                                                    ,[CreateDateTime] = @CreateDateTime
                                                    ,[LastUpdateDateTime] = @LastUpdateDateTime
                                                WHERE [MemberId] = @MemberId and [ChainId] =@ChainID
                                            
                                            ELSE 
                                            INSERT INTO [dbo].[CustomerBannerMetadata]
                                                        ([MemberId]
                                                        ,[ChainId]
                                                        ,[CouponAlias]
                                                        ,[CouponId]
                                                        ,[ShoppingListId]
                                                        ,[StoreNumber]
                                                        ,[CreateDateTime]
                                                        ,[LastUpdateDateTime])
                                                    VALUES
                                                        (@MemberId
                                                        ,@ChainId
                                                        ,@CouponAlias
                                                        ,@CouponId
                                                        ,@ShoppingListId
                                                        ,@StoreNumber
                                                        ,@CreateDateTime
                                                        ,@LastUpdateDateTime)";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        ///
        /// <returns>   An asynchronous result that yields the configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<QueueConfiguration> GetConfigurationByQueueName(string queueName)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetConfigurationByQueueName(queueName, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<QueueConfiguration> GetConfigurationByQueueName(string queueName, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<QueueConfiguration>("SELECT *  FROM [dbo].[QueueConfiguration] (nolock) Where QUeueName = @QueueName", new { queueName }, transaction).ConfigureAwait(false)).SingleOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a queue configuration. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="configuratino">    The configuratino. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task SaveQueueConfiguration(QueueConfiguration configuratino)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
                await SaveQueueConfiguration(configuratino, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a queue configuration. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="configuratino">    The configuratino. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task SaveQueueConfiguration(QueueConfiguration configuratino, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteAsync(UpsertSQL, configuratino, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields the active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<string>> GetActiveQueueNames()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetActiveQueueNames(db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<string>> GetActiveQueueNames(IDbConnection connection, IDbTransaction transaction = null)
        {
            return await connection.QueryAsync<string>("SELECT QueueName FROM [dbo].[QueueConfiguration] (nolock) Where IsActive = 1", transaction: transaction).ConfigureAwait(false);

        }
    }
}
