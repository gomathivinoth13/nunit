////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	dataaccess\devicedal.cs
//
// summary:	Implements the devicedal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using SEG.ApiService.Models.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A device dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DeviceDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Check device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> CheckDeviceId(string deviceId, IDbConnection connection, IDbTransaction transaction = null)
        {

            var result = await connection.ExecuteScalarAsync<int>("Select count(*) from dbo.Device (nolock) where DeviceID =@DeviceID", new { deviceId }, transaction).ConfigureAwait(false);
            return (result > 0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Check device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> Identifier for the device. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> CheckDeviceId(string deviceId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await CheckDeviceId(deviceId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a device. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="device">   The device. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveDevice(Device device)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveDevice(device, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a device. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="device">       The device. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveDevice(Device device, IDbConnection connection, IDbTransaction transaction = null)
        {
            try
            {

                /*
              @MemberId nvarchar(36) 
                  ,@DeviceId nvarchar(32)
                  ,@LastLoginDateTime datetime
                  ,@FirebaseRegistrationId varchar(255)
                  ,@chain varchar(2)
                      */  
                var dev = new { 
                    device.MemberId,
                    device.DeviceId,
                    device.LastLoginDateTime,
                    device.FirebaseRegistrationId,
                    chain = device.Chain
                }; 
                
                

                await connection.ExecuteAsync("dbo.Device_Upsert", dev, transaction, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                var b = ex;
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets devices by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/28/2018. </remarks>
        ///
        /// <param name="memberID"> Identifier for the member. </param>
        ///
        /// <returns>   An asynchronous result that yields the devices by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<Device>> GetDevicesByMemberID(string memberID)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetDevicesByMemberID(memberID, db);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets devices by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/28/2018. </remarks>
        ///
        /// <param name="memberID">     Identifier for the member. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the devices by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<Device>> GetDevicesByMemberID(string memberID, IDbConnection connection, IDbTransaction transaction = null)
        {
            // await connection.ExecuteAsync("dbo.Device_Upsert", device, transaction, commandType: CommandType.StoredProcedure);
            var results = await connection.QueryAsync<Device>("Select * from dbo.Device (nolock) where MemberId = @MemberID", new { MemberId = memberID });
            return results.AsList();
        }
    }
}
