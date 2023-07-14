using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;
using Dapper;
using System.Data;
using WalletAccountDataProcessorFunctionApp.Interface;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{
    public class WalletAccountIDEventDataDAL : ConfigurationDAL,IWalletAccountIDEventDataDAL
    {
        public async Task<bool> SetWalletAccountIdEventData(WalletAccountIDEventData data)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"INSERT INTO [dbo].[WalletAccountDataProcessorEventData](
	                             EventID
	                            ,EventName
	                            ,AccountID
	                            ,WalletID
	                            ,CampaignID
	                            ,State
                                ,Status
	                            ,Type
                                ,ClientType
	                            ,Created_DT
                                ,Created_Source
                                ,Updqated_DT
	                            )
                            VALUES (
	                             @EventID
	                            ,@EventName
	                            ,@AccountID
	                            ,@WalletID
	                            ,@CampaignID
	                            ,@State
                                ,@Status
	                            ,@Type
                                ,@ClientType
	                            ,@Created_DT
	                            ,@Created_Source
	                            ,@Updated_DT
	                            )";

                    var eventData = connection.ExecuteScalar<int>(sql,data, commandType: CommandType.Text);
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<bool> UpdateWalletAccountIdEventData(WalletAccountIDEventData data)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"Update 
                                  [dbo].[WalletAccountDataProcessorEventData] 
                                SET 
                                  [Created_Source] = @Created_Source, 
                                  [Updqated_DT] = @Updated_DT
                                WHERE 
                                  [AccountID] = @AccountID";

                    var eventData = connection.ExecuteScalar<int>(sql,data, commandType: CommandType.Text);
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
