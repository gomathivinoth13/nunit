using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;
using Dapper;
using WalletAccountDataProcessorFunctionApp.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{
    public class WalletAccountDataProcessEventDataRepo : IWalletAccountDataProcessEventDataRepo
    {
        private readonly ILogger<WalletAccountDataProcessEventDataRepo> _log;
        private readonly string _connectionString;

        public WalletAccountDataProcessEventDataRepo(ILogger<WalletAccountDataProcessEventDataRepo> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _connectionString = Environment.GetEnvironmentVariable("DataBaseConnectionString") ?? throw new ArgumentNullException(nameof(_connectionString));

        }
        public async Task<bool> SetWalletAccountIdEventData(List<WalletAccountIDEventData> data)
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

                    int rowsAffected = await connection.ExecuteAsync(sql, data);
                    if (rowsAffected == 0) return false;
                }

                return true;
            }
            catch (SqlException ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return false;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return false;

            }

        }

        public async Task<bool> UpdateWalletAccountIdEventData(List<WalletAccountIDEventData> data)
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

                    int rowsAffected = await connection.ExecuteAsync(sql, data);
                    if (rowsAffected == 0) return false;
                }

                return true;
            }
            catch (SqlException ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return false;

            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return false;

            }
        }

    }
}
