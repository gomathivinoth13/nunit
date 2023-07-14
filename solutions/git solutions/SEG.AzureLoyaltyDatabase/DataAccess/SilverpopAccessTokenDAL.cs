////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\SilverpopAccessTokenDAL.cs
//
// summary:	Implements the silverpop access token dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using Newtonsoft.Json;
using SEG.ApiService.Models.Queueing;
using SEG.ApiService.Models.SilverPop;
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
    /// <summary>   A silverpop access token dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class SilverpopAccessTokenDAL : DapperDalBase
    {
        private const string SelectSql = @"SELECT * FROM [dbo].[SilverPopAccessToken] (nolock)";  
        private const string UpsertSQL = @"IF EXISTS (Select Id from SilverPopAccessToken where Id = @ID)  
                                    BEGIN
                                    UPDATE [dbo].[SilverPopAccessToken]
                                       SET [access_token] = @access_token
                                          ,[token_type] = @token_type
                                          ,[refresh_token] = @refresh_token
                                          ,[expires_in] = @expires_in
                                          ,[TransactionId] = @TransactionId
                                          ,[ExpireAtDateTime] = @ExpireAtDateTime
                                          ,[SilverPopClientID] = @SilverPopClientID
                                          ,[SilverPopClientSecret] = @SilverPopClientSecret
                                          ,[SilverPopListId] = @SilverPopListId
                                     WHERE Id = @Id
                                     SELECT @ID
                                    END
                                    ELSE
                                    BEGIN
                                    INSERT INTO [dbo].[SilverPopAccessToken]
                                        ([access_token]
                                        ,[token_type]
                                        ,[refresh_token]
                                        ,[expires_in]
                                        ,[TransactionId]
                                        ,[ExpireAtDateTime]
                                        ,[SilverPopClientID]
                                        ,[SilverPopClientSecret]
                                        ,[SilverPopListId])
                                    VALUES
                                        (@access_token
                                        ,@token_type
                                        ,@refresh_token
                                        ,@expires_in
                                        ,@TransactionId
                                        ,@ExpireAtDateTime
                                        ,@SilverPopClientID
                                        ,@SilverPopClientSecret
                                        ,@SilverPopListId)

                                    Select SCOPE_IDENTITY()
                                    END";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a silver pop access token. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token">    The token. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<long> SaveSilverPopAccessToken(SilverPopAccessToken token)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await SaveSilverPopAccessToken(token, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a silver pop access token. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token">        The token. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<long> SaveSilverPopAccessToken(SilverPopAccessToken token, IDbConnection connection, IDbTransaction transaction = null)
        {
            return await connection.ExecuteScalarAsync<long>(UpsertSQL, token, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets access token. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the access token. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<SilverPopAccessToken> GetAccessToken( IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<SilverPopAccessToken>(SelectSql).ConfigureAwait(false)).SingleOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets access token. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields the access token. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<SilverPopAccessToken> GetAccessToken()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetAccessToken( db).ConfigureAwait(false);
            }
        }
    }
}
