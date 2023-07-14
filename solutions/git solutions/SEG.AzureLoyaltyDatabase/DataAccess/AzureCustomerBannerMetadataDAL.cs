////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\AzureCustomerBannerMetadataDAL.cs
//
// summary:	Implements the azure customer banner metadata dal class
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
    /// <summary>   An azure customer banner metadata dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureCustomerBannerMetadataDAL : DapperDalBase
    {
        private static log4net.ILog Logging = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging
        private const string UpsertSQL = @"IF EXISTS (Select [MemberId] from [CustomerBannerMetadata] (nolock) where [MemberId] = @MemberId and [ChainId] =@ChainID) 
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
        /// <summary>   Saves a customer banner metadata. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="banner">   The banner. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveCustomerBannerMetadata(AzureCustomerBannerMetadata banner)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveCustomerBannerMetadata(banner, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer banner metadata. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="banner">       The banner. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveCustomerBannerMetadata(AzureCustomerBannerMetadata banner, IDbConnection connection, IDbTransaction transaction = null)
        {
            try
            {
                //new banner, need to be added to existing customer
                if (!string.IsNullOrEmpty(banner.MemberId))
                {

                    if (!(await AzureCustomerDAL.CustomerExists(banner.MemberId).ConfigureAwait(false)))
                    {
                        var cust = new AzureCustomer()
                        {
                            MemberId = banner.MemberId,
                            CreateDateTime = DateTime.Now,
                            LastUpdateDateTime = DateTime.Now
                        };

                        await AzureCustomerDAL.SaveCustomer(cust);

                    }

                    await connection.ExecuteAsync(UpsertSQL, banner);

                }
            }
            catch( Exception e )
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets by member identifier and chain identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<AzureCustomerBannerMetadata> GetByMemberIdAndChainId(string memberId, string chainId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))

                return await GetByMemberIdAndChainId(memberId, chainId, db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets by member identifier and chain identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="chainId">      Identifier for the chain. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the by member identifier and chain identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<AzureCustomerBannerMetadata> GetByMemberIdAndChainId(string memberId, string chainId, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<AzureCustomerBannerMetadata>("Select * from [dbo].[CustomerBannerMetadata] (nolock) where ChainId = @ChainId and MemberID = @MemberID", new { memberId, chainId }, transaction).ConfigureAwait(false)).SingleOrDefault();
        }
    }
}
