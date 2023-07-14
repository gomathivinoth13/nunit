////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\AzureCustomerExernalLogins.cs
//
// summary:	Implements the azure customer exernal logins class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using Newtonsoft.Json;
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
    /// <summary>   An azure customer exernal logins dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureCustomerExernalLoginsDAL : DapperDalBase
    {
        private static log4net.ILog Logging = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging
        private const string InsertSql = @"IF EXISTS (Select * from [ExternalLogins] (nolock) where [MemberId] = @MemberId and [Provider] =@Provider)   
                                              INSERT INTO [dbo].[ExternalLogins]
                                                   ([Provider]
                                                   ,[UserId]
                                                   ,[MemberId])
                                             VALUES
                                                   (@Provider
                                                   ,@UserId
                                                   ,@MemberId)";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Links an external references asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="userid">   The userid. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task LinkExternalReferencesAsync(string memberId, string provider, string userid)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await LinkExternalReferencesAsync(memberId, provider, userid, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Links an external references asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="provider">     The provider. </param>
        /// <param name="userid">       The userid. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task LinkExternalReferencesAsync(string memberId, string provider, string userid, IDbConnection connection, IDbTransaction transaction = null)
        {

            await connection.ExecuteAsync(InsertSql, new { memberId, provider, userid }, transaction).ConfigureAwait(false);
            
        }


    }
}
