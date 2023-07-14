using Dapper;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Excentus;
using SEG.ApiService.Models.MobileFirst;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   SMSCode DAL. </summary>
    ///
    /// <remarks>   Mark, 5/6/2020. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class EncryptSMSCodeDAL : DapperDalBase
    {
        private const string UpsertSQL = @"IF EXISTS (Select * from [dbo].[SMSCode] (nolock) where PhoneNumber = @PhoneNumber) 
                                            UPDATE [dbo].[SMSCode]
                                               SET [Code] = @Code
                                                  ,[CreationTime] = @CreationTime
                                                  ,[ExpirationTime] = @ExpirationTime
                                             WHERE [PhoneNumber] = @PhoneNumber
                                            ELSE
                                            INSERT INTO [dbo].[SMSCode]
                                                       ([PhoneNumber]
                                                       ,[Code]
                                                       ,[CreationTime]
                                                       ,[ExpirationTime])
                                                 VALUES
                                                       (@PhoneNumber
                                                       ,@Code
                                                       ,@CreationTime
                                                       ,@ExpirationTime)";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a token index. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="code">    The token. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveSMSCode(EncryptSMSCode code)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveSMSCode(code, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a token index. </summary>
        ///
        /// <remarks>   Mark, 5/6/2020. </remarks>
        ///
        /// <param name="code">        The code. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveSMSCode(EncryptSMSCode code, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<long>(UpsertSQL, code, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the codex described by phoneNumber. </summary>
        ///
        /// <remarks>   Mark, 5/6/2020. </remarks>
        ///
        /// <param name="phoneNumber">  The phoneNumber. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteSMSCode(string phoneNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await DeleteSMSCode(phoneNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the token index. </summary>
        ///
        /// <remarks>   Mark, 5/6/2020. </remarks>
        ///
        /// <param name="phoneNumber">  The code. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteSMSCode(string phoneNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
            await connection.ExecuteScalarAsync<long>("DELETE FROM [dbo].[SMSCode] WHERE PhoneNumber = @PhoneNumber", new { PhoneNumber = phoneNumber }, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get the code described by phoneNumber. </summary>
        ///
        /// <remarks>   Mark, 5/6/2020. </remarks>
        ///
        /// <param name="phoneNumber">  The phoneNumber. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<EncryptSMSCode> GetSMSCode(string phoneNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetSMSCode(phoneNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the token index. </summary>
        ///
        /// <remarks>   Mark, 5/6/2020. </remarks>
        ///
        /// <param name="phoneNumber">  The code. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<EncryptSMSCode> GetSMSCode(string phoneNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
           return await connection.ExecuteScalarAsync<EncryptSMSCode>("SELECT PhoneNumber, Code, CreationTime, ExpirationTime FROM [dbo].[SMSCode] WHERE PhoneNumber = @PhoneNumber", new { PhoneNumber = phoneNumber }, transaction).ConfigureAwait(false);
        }

    }
}
