using Dapper;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationSFMCFunctionApp.Implementation
{
    public class CampaignIssuanceCountDAL : ConfigurationDAL, ICampaignIssuanceCount
    {
        //private string _connectionString { get; set; }

        //public CampaignIssuanceCountDAL(string connectionString)
        //{
        //    _connectionString = connectionString;

        //}

        public async Task<CampaignIssuanceCount> GetCampaignIssuanceCountData(string campaignID)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var sqlStatement = string.Format("SELECT * FROM [CampaignIssuanceCount] with (nolock) where [CampaignID] = '{0}' ", campaignID);

                    var eventData = connection.Query<CampaignIssuanceCount>(sqlStatement);

                    if (eventData == null)
                    {
                        return new CampaignIssuanceCount();
                    }
                    else
                    {
                        return eventData.FirstOrDefault();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<bool> InsertCampaignIssuanceCountData(CampaignIssuanceCount data)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var sqlStatement = string.Format(@"INSERT INTO [dbo].[CampaignIssuanceCount]
           ([CampaignID]
      ,[IssuanceCount]
      ,[Status]
      ,[CampaignEndDate]
      ,[Created_DT]
      ,[Created_Source]
      ,[Updated_DT]
      ,[Updated_Source])
     VALUES
           (@CampaignID
      ,@IssuanceCount
      ,@Status
      ,@CampaignEndDate
      ,@Created_DT
      ,@Created_Source
      ,@Updated_DT
      ,@Updated_Source) SELECT CAST(SCOPE_IDENTITY() as int)");

                    var eventData = connection.ExecuteScalar<int>(sqlStatement, data);

                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public async Task<bool> UpdateCampaignIssuanceCountData(CampaignIssuanceCount data)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var sqlStatement = string.Format(@"UPDATE [dbo].[CampaignIssuanceCount] SET 
      [IssuanceCount] = @IssuanceCount
      ,[Updated_DT] = @Updated_DT
      ,[Updated_Source] = @Updated_Source
 WHERE campaignID = @CampaignID
 SELECT CAST(SCOPE_IDENTITY() as int)");

                    var eventData = connection.ExecuteScalar<int>(sqlStatement, data);

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
