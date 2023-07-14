////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\DeviceMetaDataDAL.cs
//
// summary:	Implements the device meta data dal class
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
    /// <summary>   A device meta data dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DeviceMetaDataDAL : DapperDalBase
    {
        const string InsertSql = @"INSERT INTO [dbo].[DeviceMetadata]    
                                       ([DeviceId]
                                       ,[Category]
                                       ,[Subcategory]
                                       ,[Key]
                                       ,[Value])
                                 VALUES
                                       (@DeviceId 
                                       ,@Category 
                                       ,@Subcategory
                                       ,@Key
                                       ,@Value);
                                  Select SCOPE_IDENTITY()";

        const string UpdateSql = @"UPDATE [dbo].[DeviceMetadata]     
                                   SET [DeviceId] = @DeviceId
                                      ,[Category] = @Category
                                      ,[Subcategory] = @Subcategory
                                      ,[Key] = @Key
                                      ,[Value] = @Value
                                 WHERE [DeviceMetadataId]  =@DeviceMetadataId;
                                 SELECT @DeviceMetadataId";

        public static async Task<long> SaveDeviceMetaData(DeviceMetadata deviceMetaData)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await SaveDeviceMetaData(deviceMetaData, db);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a device meta data. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="deviceMetaData">   Information describing the device meta. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<long> SaveDeviceMetaData(DeviceMetadata deviceMetaData, IDbConnection connection, IDbTransaction transaction = null)
        {
            var deviceExists = await DeviceDAL.CheckDeviceId(deviceMetaData.DeviceId, connection, transaction).ConfigureAwait(false);

            if (deviceExists)
            {
                bool exists = (await connection.QueryAsync<DeviceMetadata>("Select * from dbo.DeviceMetaData (nolock) where [DeviceId] = @DeviceID AND [Category]=@Category AND [Key]=@Key", new { deviceMetaData.DeviceId, deviceMetaData.Category, deviceMetaData.Subcategory, deviceMetaData.Key })).Any();
                if (!exists)
                    return await connection.ExecuteScalarAsync<long>(InsertSql, deviceMetaData, transaction).ConfigureAwait(false);
                else
                    return await connection.ExecuteScalarAsync<long>(UpdateSql, deviceMetaData, transaction).ConfigureAwait(false);
            }
            else
            {
                string error = $"Error Saving DeviceMetaData.  Unable to find record for DeviceId: {deviceMetaData.DeviceId}";
                throw new ArgumentException(error);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets meta data. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="param">        The parameter. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the meta data. </returns>         
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async static Task<DeviceMetadata> GetMetaData(object param, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<DeviceMetadata>("SELECT * from DeviceMetaData (nolock) where DeviceMetaDataId = @DeviceMetaDataId", param, transaction).ConfigureAwait(false)).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async static Task<DeviceMetadata> GetDeviceMetaData(DeviceMetadata param, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<DeviceMetadata>("Select * from dbo.DeviceMetaData (nolock) where [DeviceId] = @DeviceID AND[Category] = @Category AND[Key] = @Key", new { param.DeviceId, param.Category, param.Key }, transaction).ConfigureAwait(false)).SingleOrDefault();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets meta data. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceMetaDataId"> Identifier for the device meta data. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the meta data. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<DeviceMetadata> GetMetaData(string deviceMetaDataId, IDbConnection connection, IDbTransaction transaction = null)
        {
            return await GetMetaData(new { deviceMetaDataId }, connection, transaction).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets meta data. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceMetaDataId"> Identifier for the device meta data. </param>
        ///
        /// <returns>   An asynchronous result that yields the meta data. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<DeviceMetadata> GetMetaData(string deviceMetaDataId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetMetaData(deviceMetaDataId, db).ConfigureAwait(false);
            }
        }
    }
}
