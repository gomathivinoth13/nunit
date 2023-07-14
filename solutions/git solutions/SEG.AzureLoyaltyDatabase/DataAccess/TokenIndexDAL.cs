////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\TokenIndexDAL.cs
//
// summary:	Implements the token index dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A token index dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class TokenIndexDal : DapperDalBase
    {
        private const string UpsertSQL = @"IF EXISTS (Select * from [Storage].[TokenIndex] (nolock) where RowKey = @RowKey)  ///< The row key]
                                            UPDATE [Storage].[TokenIndex]
                                               SET [TokenExpire] = @TokenExpire
                                                  ,[EmailAddress] = @EmailAddress
                                                  ,[MemberID] = @MemberID
                                                  ,[SSO] = @SSO
                                             WHERE [RowKey] = @RowKey
                                            ELSE
                                            INSERT INTO [Storage].[TokenIndex]
                                                       ([RowKey]
                                                       ,[TokenExpire]
                                                       ,[EmailAddress]
                                                       ,[MemberID]
                                                       ,[SSO])
                                                 VALUES
                                                       (@RowKey
                                                       ,@TokenExpire
                                                       ,@EmailAddress
                                                       ,@MemberID
                                                       ,@SSO)";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a token index. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token">    The token. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveTokenIndex(TokenIndex token)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveTokenIndex(token, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a token index. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token">        The token. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveTokenIndex(TokenIndex token, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<long>(UpsertSQL, token, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the token index described by accessToken. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="accessToken">  The access token. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteTokenIndex(string accessToken)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await DeleteTokenIndex(accessToken, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the token index. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="accessToken">  The access token. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteTokenIndex(string accessToken, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<long>("DELETE FROM [Storage].[TokenIndex] WHERE RowKey = @RowKey", new { RowKey = accessToken }, transaction).ConfigureAwait(false);
        }

    }
}
