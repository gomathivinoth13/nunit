using Dapper;
using SEG.ApiService.Models.Offers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEG.ApiService.Models.Database;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerEnrollmentOfferDAL : DapperDalBase
    {

        /// <summary>
        /// getCustomerEnrollmentOffer
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerEnrollmentOffer>> getCustomerEnrollmentOffer(DateTime currentDate, string chainId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await getCustomerEnrollmentOffer(db, currentDate, chainId).ConfigureAwait(false);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentDate"></param>
        /// <param name="chainId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerEnrollmentOffer>> getCustomerEnrollmentOffer(IDbConnection connection, DateTime currentDate, string chainId, IDbTransaction transaction = null)
        {
            List<CustomerEnrollmentOffer> list = new List<CustomerEnrollmentOffer>();
            var result = (await connection.QueryAsync<CustomerEnrollmentOffer>("Select * from CustomerEnrollmentOffer where  @currentDate >= OfferActiveDate and @currentDate<= OfferExpiryDate and Banner = @chainId", new { currentDate, chainId }, transaction).ConfigureAwait(false)).ToList();

            return result;

        }




        /// <summary>
        /// getCustomerEnrollmentOffer
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerEnrollmentOffer>> getCustomerEnrollmentOfferEE(DateTime currentDate, string chainId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await getCustomerEnrollmentOfferEE(db, currentDate, chainId).ConfigureAwait(false);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentDate"></param>
        /// <param name="chainId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerEnrollmentOffer>> getCustomerEnrollmentOfferEE(IDbConnection connection, DateTime currentDate, string chainId, IDbTransaction transaction = null)
        {
            List<CustomerEnrollmentOffer> list = new List<CustomerEnrollmentOffer>();
            var result = (await connection.QueryAsync<CustomerEnrollmentOffer>("Select * from CustomerEnrollmentOffer where LastName = 'EE' and  @currentDate >= OfferActiveDate and @currentDate<= OfferExpiryDate and Banner = @chainId", new { currentDate, chainId }, transaction).ConfigureAwait(false)).ToList();

            return result;

        }
    }
}
