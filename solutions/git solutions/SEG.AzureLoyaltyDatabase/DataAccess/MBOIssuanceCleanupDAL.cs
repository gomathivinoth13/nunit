using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class MBOIssuanceCleanupDAL : DapperDalBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public static async Task<bool> PurgeMBOInssuance()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var storedProcedure = "[dbo].[DELETE_EE_MBO_EV_DATA_SP]";
                    
                    var result = connection.Query(storedProcedure, commandType: CommandType.StoredProcedure);

                    return true;
                    
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }



    
        }   



    }
}
