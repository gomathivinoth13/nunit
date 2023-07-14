using Dapper;
using SEG.ApiService.Models.AuditCustomerServicePoints;
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
    public class AuditCustomerServiceRepDAL : DapperDalBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<AuditCustomerServiceRep> GetAuditCustomerServiceRep(string userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string sqlStatement = string.Format(@"SELECT TOP 1 * FROM dbo.AuditCustomerServiceRep (nolock) where UserId ='{0}'", userId);

                var auditCustomerServiceRep = connection.Query<AuditCustomerServiceRep>(sqlStatement);


                if (auditCustomerServiceRep == null)
                {
                    return new AuditCustomerServiceRep();
                }
                else
                {
                    return auditCustomerServiceRep.FirstOrDefault();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditCustomerServiceRep"></param>
        /// <returns></returns>

        public static async Task<bool> InsertAuditCustomerServiceRep(AuditCustomerServiceRep auditCustomerServiceRep)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string sqlStatement = string.Format(@"INSERT INTO AuditCustomerServiceRep (UserId,FirstName,LastName) VALUES (@UserId,@FirstName,@LastName)");
                    connection.Execute(sqlStatement, auditCustomerServiceRep);
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}


