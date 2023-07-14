using Dapper;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationSFMCFunctionApp.Implementation
{
    public class EagleEyeMBOIssuanceEventDataDAL : ConfigurationDAL, IEagleEyeMBOIssuanceEventData
    {

        public async Task<EagleEyeMBOIssuanceEventData> GetEagleEyeMBOIssuanceEventData(string accountID)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var storedProcedure = "[dbo].[usp_GetEagleEyeMBOIssuanceEventData]";
                    var parameters = new
                    {
                        AccountID = accountID,
                        EventName = EventNameType.CREATE.ToString()
                    };

                    var eventData = connection.Query<EagleEyeMBOIssuanceEventData>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                    if (eventData == null)
                    {
                        return new EagleEyeMBOIssuanceEventData();
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


        public async Task<bool> SetEagleEyeMBOIssuanceEventData(EagleEyeMBOIssuanceEventData data)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var storedProcedure = "[dbo].[usp_InsertEagleEyeMBOIssuanceEventData]";

                    var eventData = connection.ExecuteScalar<int>(storedProcedure, data, commandType: CommandType.StoredProcedure);

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
