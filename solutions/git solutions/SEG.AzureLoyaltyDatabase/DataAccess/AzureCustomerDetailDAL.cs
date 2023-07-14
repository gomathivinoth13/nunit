////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\AzureCustomerDetailDAL.cs
//
// summary:	Implements the azure customer detail dal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using SEG.ApiService.Models.Attributes;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An azure customer detail dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureCustomerDetailDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="PhoneNumber">  The phone number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by phone number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByPhoneNumber(string PhoneNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByPhoneNumber(PhoneNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="PhoneNumber">  The phone number. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by phone number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByPhoneNumber(string PhoneNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
            AzureCustomerDetail customerDetail = null;

            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "C.mobilephonenumber=@PhoneNumber", PhoneNumber}

            };

            var customer = await AzureCustomerDAL.GetCustomerByDictionary(request, connection, transaction);

            if (customer != null)
            {

                customerDetail = AzureCustomerDAL.MapCustomerToCustomerDetail(customer);
            }
            return customerDetail;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by alias number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber">  The alias number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by alias number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByAliasNumber(string AliasNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByAliasNumber(AliasNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by alias number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber">  The alias number. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by alias number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByAliasNumber(string AliasNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
            AzureCustomerDetail customerDetail = null;


            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "C.AliasNumber=@AliasNumber", AliasNumber}

            };

            var customer = await AzureCustomerDAL.GetCustomerByDictionary(request, connection, transaction);

            if (customer != null)
            {

                customerDetail = AzureCustomerDAL.MapCustomerToCustomerDetail(customer);
            }
            return customerDetail;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by CRC. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber">  The alias number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByCRC(string AliasNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByCRC(AliasNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by CRC. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="CouponAlias">  The coupon alias. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByCRC(string CouponAlias, IDbConnection connection, IDbTransaction transaction = null)
        {
            AzureCustomerDetail customerDetail = null;


            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "M.CouponAlias=@AliasNumber", CouponAlias}
            };

            var customer = await AzureCustomerDAL.GetCustomerByDictionary(request, connection, transaction);

            if (customer != null)
            {

                customerDetail = AzureCustomerDAL.MapCustomerToCustomerDetail(customer);
            }
            return customerDetail;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer detail. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerDetail">   The customer detail. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveCustomerDetail(AzureCustomerDetail customerDetail)
        {
            if (!string.IsNullOrEmpty(customerDetail.MemberId))
            {
                AzureCustomer dbCustomer = await AzureCustomerDAL.GetCustomerByMemberId(customerDetail.MemberId);
                AzureCustomerBannerMetadata dbCustomerBanner = null;
                if (dbCustomer != null && !String.IsNullOrEmpty(customerDetail.ChainId) && !String.IsNullOrEmpty(customerDetail.CouponAlias))
                {
                    //customer with memberId already exists
                    if (dbCustomer.AzureCustomerBannerMetadata != null)
                    {
                        dbCustomerBanner = dbCustomer.AzureCustomerBannerMetadata.Where(x => x.ChainId.Trim() == customerDetail.ChainId.Trim() && x.MemberId == dbCustomer.MemberId).FirstOrDefault();
                    }


                    if (dbCustomerBanner == null)
                    {
                        //no record for this banner
                        if (dbCustomer.AzureCustomerBannerMetadata == null || dbCustomer.AzureCustomerBannerMetadata.Any())
                        {
                            //new banner, need to be added to existing customer
                            AzureCustomerBannerMetadata banner = AzureCustomerDAL.MapCustomerDetailToBanner(customerDetail);

                            await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(banner).ConfigureAwait(false);
                        }
                    }

                    await AzureCustomerDAL.UpdateCustomer(dbCustomer, customerDetail).ConfigureAwait(false);
                    //update customer banner information 
                    AzureCustomerBannerMetadata updatebanner = AzureCustomerDAL.MapCustomerDetailToBanner(customerDetail);
                    await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(updatebanner).ConfigureAwait(false);
                }
                else
                {
                    //new customer
                    AzureCustomer customer = new AzureCustomer();
                    customer = AzureCustomerDAL.MapCustomerDetailToCustomer(customerDetail);
                    AzureCustomerBannerMetadata newBanner = new AzureCustomerBannerMetadata();
                    newBanner = AzureCustomerDAL.MapCustomerDetailToBanner(customerDetail);
                    customer.LastUpdateDateTime = DateTime.Now;
                    await AzureCustomerDAL.SaveCustomer(customer).ConfigureAwait(false);

                    await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(newBanner).ConfigureAwait(false);


                }


            }
        }


    }
}
