using Dapper;
using SEG.ApiService.Models.MobileFirst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerPersonalizationDAL : DapperDalBase
    {

        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <returns></returns>
        public static async Task<CustomerPersonalization> GetCustomerPersonalization(string memberId, IDbTransaction transaction = null)
        {
            CustomerPersonalization _customerPersonalization = new CustomerPersonalization();
            _customerPersonalization.MemberId = memberId;
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
              var _promos =  await GetCustomerPersonalizationByMemberId(db,memberId,transaction).ConfigureAwait(false);
             _customerPersonalization.PersonalPromos = _promos;
            }
            
            return _customerPersonalization;

        }


        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="memberId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<Guid>> GetCustomerPersonalizationByMemberId(IDbConnection connection, string memberId, IDbTransaction transaction)
        {            
            string getQuery = @"SELECT PersonalPromo FROM dbo.RewardPersonalization where MemberId = cast(@memberId as varchar(50));";

            return (await connection.QueryAsync<Guid>(getQuery, new {  memberId }, transaction).ConfigureAwait(false)).ToList();
        }


      
    }
}
