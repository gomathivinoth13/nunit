using Dapper;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Jwt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    public class JwtTokenDAL : DapperDalBase
    {
        private const string InsertSQL = @"INSERT INTO [dbo].[JwtToken]
                                                       ([TokenId]
                                                       ,[MemberId]
                                                       ,[CreateDateTime])
                                                 VALUES
                                                       (@TokenId
                                                       ,@MemberId
                                                       ,@CreateDateTime)";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a token index. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token">    The token. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveJwtToken(JwtToken token)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveJwtToken(token, db).ConfigureAwait(false);
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

        public static async Task SaveJwtToken(JwtToken token, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<long>(InsertSQL, token, transaction).ConfigureAwait(false);
        }
    }
}
