////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	dataaccess\mobiledevicedal.cs
//
// summary:	Implements the mobiledevicedal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Dapper;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Mobile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A mobile device dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class MobileDeviceDAL : DapperDalBase
    {

        #region Constants

        private const int DeviceIdMaxLenth = 32;    ///< The device identifier maximum lenth
        private const int MemberIdMaxLenth = 36;    ///< The member identifier maximum lenth
        private const int DeviceTokenMaxLenth = 500;    ///< The device token maximum lenth
        private const int OperatingSystemMaxLenth = 500;    ///< The operating system maximum lenth
        private const int DeviceNameMaxLenth = 500; ///< The device name maximum lenth
        private const int FeedCompletedVersionMaxLenth = 100;   ///< The feed completed version maximum lenth
        private const int OnboardingVersionMaxLenth = 100;  ///< The onboarding version maximum lenth
        private const int CategoryMaxLenth = 100;   ///< The category maximum lenth
        private const int SubcategoryMaxLenth = 100;    ///< The subcategory maximum lenth
        private const int KeyMaxLenth = 100;    ///< The key maximum lenth
        private const int ValueMaxLenth = 100;  ///< The value maximum lenth

        private const string PushSubscriptionCategory = "PushSubscription"; ///< Category the push subscription belongs to
        private const string ChannelSubcategory = "Channel";    ///< The channel subcategory
        private const string OperatingSystemKey = "OperatingSystem";    ///< The operating system key
        private const string DeviceTokenKey = "DeviceToken";    ///< The device token key

        private const string BadParameterLengthErrorMessage = "{0} has exceeded the max length allowed.Max Length: {1}, Current Lenght: {2}, Current Value: {3}";   ///< Message describing the bad parameter length error
        #endregion Constants

        #region Read

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="chainId">      Identifier for the chain. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<UserDetail> GetUserDetailByDeviceId(string deviceId, string chainId, IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetUserDetailByDeviceId(deviceId, chainId, db, transaction).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="chainId">      Identifier for the chain. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<UserDetail> GetUserDetailByDeviceId(string deviceId, string chainId, IDbConnection connection, IDbTransaction transaction = null)
        {
            UserDetail userDetail = null;
            var sql = @"
                        SELECT AzureCustomer.*,
	                           Device.*,
                               DeviceMetaData.*,
                               AzureCustomerBanners.*
                          FROM Device Device with (nolock) inner join
                               Customer AzureCustomer with (nolock) on Device.MemberID = AzureCustomer.MemberID left join
                               DeviceMetaData DeviceMetaData with (nolock) on Device.DeviceID = DeviceMetaData.DeviceId left join
	                           CustomerBannerMetaData AzureCustomerBanners  with (nolock) on AzureCustomer.MemberID = AzureCustomerBanners.MemberID
                         WHERE Device.DeviceId = @DeviceId
                        ";
            Dictionary<string, AzureCustomer> customerDictionary = new Dictionary<string, AzureCustomer>();

            var customers = (await connection.QueryAsync<AzureCustomer, Device, DeviceMetadata, AzureCustomerBannerMetadata, AzureCustomer>(sql, (azurecustomer, device, dm, metadata) =>
             {
                 //Adding azurecustomer null check,  found null check -- Sam 2/14/2018
                 if (azurecustomer != null)
                 {
                     if (!customerDictionary.TryGetValue(azurecustomer.MemberId, out AzureCustomer found))
                     {
                         found = azurecustomer;

                         customerDictionary.Add(azurecustomer.MemberId, found);
                     }

                     if (found != null)
                     {
                         if (metadata != null)
                         {
                             if (found.AzureCustomerBannerMetadata == null) found.AzureCustomerBannerMetadata = new List<AzureCustomerBannerMetadata>();
                             if (!found.AzureCustomerBannerMetadata.Any(a => a.ChainId == metadata.ChainId))
                                 found.AzureCustomerBannerMetadata.Add(metadata);
                         }
                         if (device != null)
                         {
                             if (found.Devices == null) found.Devices = new List<Device>();
                             if (!found.Devices.Any(a => a.DeviceId == device.DeviceId))
                                 found.Devices.Add(device);

                         }
                         if (dm != null)
                         {
                             var dev = found.Devices.FirstOrDefault(a => a.DeviceId == dm.DeviceId);
                             if (dev != null)
                             {
                                 if (dev.DeviceMetadata == null) dev.DeviceMetadata = new List<DeviceMetadata>();
                                 if (!dev.DeviceMetadata.Any(a => a.DeviceMetadataId == dm.DeviceMetadataId))
                                     dev.DeviceMetadata.Add(dm);
                             }
                         }
                         customerDictionary[found.MemberId] = found;
                         return found;
                     }
                     else
                     {
                         return null;
                     }
                 }
                 else
                 {
                     return null;
                 }

             }, new { DeviceId = deviceId }, transaction: transaction, splitOn: "MemberId,DeviceId,DeviceId,MemberId").ConfigureAwait(false)).ToList();
            Device dbDevice = null;

            //Added customers !=null -- Sam 02/14/2018

            if (customers != null && customers.Any())
            {
                var cust = customers.FirstOrDefault(a => a.Devices.Any(b => b.DeviceId == deviceId));
                if (cust != null)
                {
                    dbDevice = cust.Devices.First(f => f.DeviceId == deviceId);
                    userDetail = new UserDetail
                    {
                        DeviceId = deviceId
                    };
                    //Added dbDevice null check -- Sam 02/14/2018
                    if (dbDevice != null)
                    {
                        if (dbDevice.LastLoginDateTime.HasValue)
                        {
                            userDetail.LastLoginDateTime = dbDevice.LastLoginDateTime.Value;
                        }

                        if (!String.IsNullOrEmpty(dbDevice.MemberId))
                        {
                            userDetail.MemberId = dbDevice.MemberId;

                            if (cust != null)
                            {
                                if (!String.IsNullOrEmpty(cust.AliasNumber))
                                {
                                    userDetail.AliasNumberDB = cust.AliasNumber;
                                }
                                if (!String.IsNullOrEmpty(cust.FirstName))
                                {
                                    userDetail.FirstNameDB = cust.FirstName;
                                }

                                if (cust.AzureCustomerBannerMetadata != null && cust.AzureCustomerBannerMetadata.Any())
                                {
                                    AzureCustomerBannerMetadata azureCustomerBannerMetadata = cust.AzureCustomerBannerMetadata.FirstOrDefault(x => x.ChainId.Trim() == chainId);
                                    if (azureCustomerBannerMetadata != null)
                                    {
                                        if (azureCustomerBannerMetadata.StoreNumber.HasValue)
                                        {
                                            userDetail.StoreIdDB = azureCustomerBannerMetadata.StoreNumber.Value.ToString();
                                        }
                                        if (!String.IsNullOrEmpty(azureCustomerBannerMetadata.CouponAlias))
                                        {
                                            userDetail.CouponAlias = azureCustomerBannerMetadata.CouponAlias;
                                        }
                                        if (!String.IsNullOrEmpty(azureCustomerBannerMetadata.ShoppingListId))
                                        {
                                            userDetail.ShoppingListIdDB = azureCustomerBannerMetadata.ShoppingListId;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Added dbDevice null check

                if (dbDevice != null && dbDevice.DeviceMetadata != null)
                {
                    List<DeviceMetadata> dbDeviceMetadata = dbDevice.DeviceMetadata.Where(x => x.DeviceId == deviceId && x.Category == PushSubscriptionCategory).ToList();
                    if (dbDeviceMetadata != null && dbDeviceMetadata.Any())
                    {
                        string deviceToken = null;
                        string operatingSystem = null;

                        List<string> channels = dbDeviceMetadata.Where(x => x.Subcategory == ChannelSubcategory).Select(x => x.Key).ToList();

                        DeviceMetadata deviceTokenDeviceMetadata = dbDeviceMetadata.FirstOrDefault(x => x.Key == DeviceTokenKey);
                        if (deviceTokenDeviceMetadata != null)
                        {
                            deviceToken = deviceTokenDeviceMetadata.Value;
                        }
                        DeviceMetadata operatingSystemDeviceMetadata = dbDeviceMetadata.FirstOrDefault(x => x.Key == OperatingSystemKey);
                        if (operatingSystemDeviceMetadata != null)
                        {
                            operatingSystem = operatingSystemDeviceMetadata.Value;
                        }

                        if (channels != null)
                        {
                            userDetail.PushSubscription = new PushSubscription
                            {
                                Channels = String.Join(",", channels)
                            };
                            if (!String.IsNullOrEmpty(deviceToken))
                            {
                                userDetail.PushSubscription.DeviceToken = deviceToken;
                            }
                            if (!String.IsNullOrEmpty(operatingSystem))
                            {
                                userDetail.PushSubscription.OperatingSystem = operatingSystem;
                            }
                        }
                    }
                }
            }


            return userDetail;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets device tokens by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        ///
        /// <returns>   An asynchronous result that yields the device tokens by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<string> GetDeviceTokensByMemberId(string memberId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetDeviceTokensByMemberId(memberId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets device tokens by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the device tokens by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<string> GetDeviceTokensByMemberId(string memberId, IDbConnection connection, IDbTransaction transaction = null)
        {
            string deviceToken = null;
            string sql = @" SELECT DeviceMetaData.Value 
                              FROM Device Device with (nolock) INNER JOIN
                                   DeviceMetaData DeviceMetaData with (nolock) on Device.DeviceID = DeviceMetaData.DeviceId 
                             WHERE [Key] = 'DeviceToken' and MEMBERID = @MemberID";

            IEnumerable<string> dbDeviceToken = await connection.QueryAsync<string>(sql, new { memberId }, transaction).ConfigureAwait(false);

            if (dbDeviceToken != null && dbDeviceToken.Any())
            {
                deviceToken = string.Join(",", dbDeviceToken);
            }


            return deviceToken;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets member identifier by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> Identifier for the device. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<string> GetMemberIdByDeviceId(string deviceId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetMemberIdByDeviceId(deviceId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets member identifier by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields the member identifier by device identifier.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<string> GetMemberIdByDeviceId(string deviceId, IDbConnection connection, IDbTransaction transaction = null)
        {
            return await connection.ExecuteScalarAsync<string>("Select top 1 MemberID from Device where DeviceID = @deviceId", new { deviceId }, transaction).ConfigureAwait(false);
        }

        #endregion Read

        #region Save

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a mobile device. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="mobileDevice"> The mobile device. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveMobileDevice(MobileDevice mobileDevice)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveMobileDevice(mobileDevice, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a mobile device. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="mobileDevice"> The mobile device. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveMobileDevice(MobileDevice mobileDevice, IDbConnection connection, IDbTransaction transaction = null)
        {
            if (!VerifyMobileDevice(mobileDevice, out List<string> errorMessages))
            {
                string error = String.Format("SaveMobileDevice failed with: Invalid MobileDevice Record. Errors: {0}", String.Join(", ", errorMessages));
                throw new Exception(error);
            }

            await DeviceDAL.SaveDevice(new Device() { 
                DeviceId= mobileDevice.DeviceId, 
                FirebaseRegistrationId= mobileDevice.AccessToken,
                MemberId= mobileDevice.MemberId
            });

            if (!string.IsNullOrWhiteSpace(mobileDevice.AccessToken))
            {
                await DeviceMetaDataDAL.SaveDeviceMetaData(new DeviceMetadata() { DeviceId = mobileDevice.DeviceId, Category = "FireBase", Key = "DeviceToken", Value = mobileDevice.AccessToken });
            }
        }

        public static async Task<string[]> GetPushIdentifiersByMemberID(string memberID)
        {
            return (await DeviceDAL.GetDevicesByMemberID(memberID))
                    .SelectMany(s => s.DeviceMetadata.Where(b => b.Key == "DeviceToken" &&
                                                                 b.Category == "FireBase")
                    .Select(a => a.Value)).Distinct().ToArray();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a push subscription. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="deviceToken">  The device token. </param>
        /// <param name="type">         The type. </param>
        /// <param name="channel">      The channel. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SavePushSubscription(string deviceId, string deviceToken, string type, string channel)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SavePushSubscription(deviceId, deviceToken, type, channel, db).ConfigureAwait(false);
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a push subscription. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="deviceToken">  The device token. </param>
        /// <param name="type">         The type. </param>
        /// <param name="channel">      The channel. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SavePushSubscription(string deviceId, string deviceToken, string type, string channel, IDbConnection connection, IDbTransaction transaction = null)
        {
            if (String.IsNullOrEmpty(deviceToken))
            {
                throw new Exception("Missing required field Device_Token");
            }


            //DeviceMetadata channelDeviceMetadata = await context.DeviceMetadata.FirstOrDefaultAsync(x => x.Category == PushSubscriptionCategory && x.Subcategory == ChannelSubcategory && x.Key == channel).ConfigureAwait(false);

            DeviceMetadata channelDeviceMetadata = await DeviceMetaDataDAL.GetDeviceMetaData(new DeviceMetadata { Category = PushSubscriptionCategory, Key = channel, DeviceId = deviceId }, connection, transaction).ConfigureAwait(false);


            if (channelDeviceMetadata != null)
            {
                //check if the key is concatinated with multiple channels values .
                List<string> channelList = channelDeviceMetadata.Key.Split(',').ToList();
                //check if the key is concatinated with multiple channels values .then split them up to invidual records .
                if (channelList.Count > 1)
                {
                    //insert individual key values 
                    foreach (string channelValue in channelList)
                    {
                        channelDeviceMetadata = new DeviceMetadata
                        {
                            DeviceId = deviceId,
                            Category = PushSubscriptionCategory,
                            Subcategory = ChannelSubcategory,
                            Key = channelValue
                        };
                        await DeviceMetaDataDAL.SaveDeviceMetaData(channelDeviceMetadata, connection, transaction).ConfigureAwait(false);
                    }

                    //delete the concatinated key values from database 
                    await DeletePushSubscriptionChannel(deviceId, channel).ConfigureAwait(false);
                }
            }
            else
            {
                List<string> channelList = channel.Split(',').ToList();
                foreach (string channelValue in channelList)
                {
                    channelDeviceMetadata = new DeviceMetadata
                    {
                        DeviceId = deviceId,
                        Category = PushSubscriptionCategory,
                        Subcategory = ChannelSubcategory,
                        Key = channelValue
                    };
                    await DeviceMetaDataDAL.SaveDeviceMetaData(channelDeviceMetadata, connection, transaction).ConfigureAwait(false);
                }

            }

            if (!String.IsNullOrEmpty(type))
            {
                DeviceMetadata operatingSystemDeviceMetadata = await DeviceMetaDataDAL.GetDeviceMetaData(new DeviceMetadata { Category = PushSubscriptionCategory, Key = OperatingSystemKey, DeviceId = deviceId }, connection, transaction).ConfigureAwait(false);

                if (operatingSystemDeviceMetadata != null)
                {
                    operatingSystemDeviceMetadata.Value = type;

                }
                else
                {
                    operatingSystemDeviceMetadata = new DeviceMetadata
                    {
                        DeviceId = deviceId,
                        Category = PushSubscriptionCategory,
                        Key = OperatingSystemKey,
                        Value = type
                    };
                }
                await DeviceMetaDataDAL.SaveDeviceMetaData(operatingSystemDeviceMetadata, connection, transaction).ConfigureAwait(false);

            }

            if (!String.IsNullOrEmpty(deviceToken))
            {
                DeviceMetadata deviceTokenDeviceMetadata = await DeviceMetaDataDAL.GetDeviceMetaData(new DeviceMetadata { Category = PushSubscriptionCategory, Key = DeviceTokenKey, DeviceId = deviceId }, connection, transaction).ConfigureAwait(false);

                if (deviceTokenDeviceMetadata != null)
                {
                    deviceTokenDeviceMetadata.Value = deviceToken;

                }
                else
                {
                    deviceTokenDeviceMetadata = new DeviceMetadata
                    {
                        DeviceId = deviceId,
                        Category = PushSubscriptionCategory,
                        Key = DeviceTokenKey,
                        Value = deviceToken
                    };

                }
                await DeviceMetaDataDAL.SaveDeviceMetaData(deviceTokenDeviceMetadata, connection, transaction).ConfigureAwait(false);

            }


        }

        #endregion Save

        #region Delete

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the push subscription channel. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> Identifier for the device. </param>
        /// <param name="channel">  The channel. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeletePushSubscriptionChannel(string deviceId, string channel)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await DeletePushSubscriptionChannel(deviceId, channel, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the push subscription channel. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="channel">      The channel. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeletePushSubscriptionChannel(string deviceId, string channel, IDbConnection connection, IDbTransaction transaction = null)
        {

            if (channel != null)
            {
                int rows = await connection.ExecuteAsync(@"DELETE 
                                                FROM  [dbo].[DeviceMetadata] 
                                                WHERE [DeviceId] = @DeviceId and 
                                                      [Category] = @Category and 
                                                      [SubCategory] = @SubCategory and 
                                                      [Key] = @Channel",
                   new
                   {
                       DeviceId = deviceId,
                       Category = PushSubscriptionCategory,
                       SubCategory = ChannelSubcategory,
                       Channel = channel
                   }, transaction).ConfigureAwait(false);
            }
        }

        #endregion Delete

        #region Verify

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify mobile device. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="mobileDevice">     The mobile device. </param>
        /// <param name="errorMessages">    [out] The error messages. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private static bool VerifyMobileDevice(MobileDevice mobileDevice, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            bool valid = true;

            if (mobileDevice == null)
            {
                errorMessages.Add("MobileDevice Null request");
                return false;
            }

            if (string.IsNullOrEmpty(mobileDevice.DeviceId))
            {
                errorMessages.Add("DeviceId is required");
                return false;
            }

            if (mobileDevice.DeviceId.Length > DeviceIdMaxLenth)
            {
                errorMessages.Add(String.Format(BadParameterLengthErrorMessage, "DeviceId", DeviceIdMaxLenth, mobileDevice.DeviceId.Length, mobileDevice.DeviceId));
                valid = false;
            }

            if (!String.IsNullOrEmpty(mobileDevice.MemberId) && mobileDevice.MemberId.Length > MemberIdMaxLenth)
            {
                errorMessages.Add(String.Format(BadParameterLengthErrorMessage, "MemberId", MemberIdMaxLenth, mobileDevice.MemberId.Length, mobileDevice.MemberId));
                valid = false;
            }

            return valid;
        }


        #endregion Verify
    }
}