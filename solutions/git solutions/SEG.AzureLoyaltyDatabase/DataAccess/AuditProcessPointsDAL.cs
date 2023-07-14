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
    public class AuditProcessPointsDAL : DapperDalBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditProcessPoints"></param>
        /// <returns></returns>
        public static async Task<bool> InsertAuditProcessPoints(AuditProcessPoints auditProcessPoints)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string sqlStatement = string.Format(@"INSERT INTO AuditProcessPoints (AuditId,MemberID,WalletID,StoreNumber,ReceiptNumber,TotalPoints,CreateDateTime,CreateUser,LastUpdateDateTime,LastUpdateUser,ServicePointsResponse,CustomerServiceTicket_TicketNumber) VALUES(@AuditId,@MemberID,@WalletID,@StoreNumber,@ReceiptNumber,@TotalPoints,@CreateDateTime,@CreateUser,@LastUpdateDateTime,@LastUpdateUser,@ServicePointsResponse,@CustomerServiceTicket_TicketNumber)");
                    connection.Execute(sqlStatement, auditProcessPoints);
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
