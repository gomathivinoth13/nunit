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
    public class AuditCustomerServiceTicketDAL : DapperDalBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public static async Task<AuditCustomerServiceTicket> GetAuditCustomerServiceTicket(string TicketNumber)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string sqlStatement = string.Format(@"SELECT TOP 1 * FROM dbo.AuditCustomerServiceTicket (nolock) where TicketNumber ='{0}'", TicketNumber);

                var auditCustomerServiceTicket = connection.Query<AuditCustomerServiceTicket>(sqlStatement);


                if (auditCustomerServiceTicket == null)
                {
                    return new AuditCustomerServiceTicket();
                }
                else
                {
                    return auditCustomerServiceTicket.FirstOrDefault();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditCustomerServiceTicket"></param>
        /// <returns></returns>
        public static async Task<bool> InsertAuditCustomerServiceTicket(AuditCustomerServiceTicket auditCustomerServiceTicket)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string sqlStatement = string.Format(@"INSERT INTO AuditCustomerServiceTicket (TicketNumber,TicketDate,Title,Description,Comments,CustomerServiceRep_UserId) VALUES (@TicketNumber,@TicketDate,@Title,@Description,@Comments,@CustomerServiceRep_UserId)");
                    connection.Execute(sqlStatement, auditCustomerServiceTicket);
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
