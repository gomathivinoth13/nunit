////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	azureloyaltydatabasemanager.cs
//
// summary:	Implements the azureloyaltydatabasemanager class
////////////////////////////////////////////////////////////////////////////////////////////////////

using log4net;
using SEG.ApiService.Models;
using SEG.ApiService.Models.AuditCustomerServicePoints;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Jwt;
using SEG.ApiService.Models.Mobile;
using SEG.ApiService.Models.MobileFirst;
using SEG.ApiService.Models.Queueing;
using SEG.ApiService.Models.Surveys;
using SEG.AzureLoyaltyDatabase.DataAccess;
using SEG.LoyaltyDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manager for azure loyalty databases. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class AzureLoyaltyDatabaseManager
    {
        #region Static Variables

        private static ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging

        #endregion Static Variables    

        #region Public Methods

        #region Log

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a log. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="log">              The log. </param>
        /// <param name="checkIfExists">    True to check if exists. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveLog(AzureLog log, bool checkIfExists)
        {
            try
            {
                await LoggingDAL.SaveLog(log, checkIfExists).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save a log to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SaveLog", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the external log records by date described by deleteDate. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deleteDate">   The delete date. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task DeleteExternalLogRecordsByDate(DateTime deleteDate)
        {
            try
            {
                await LoggingDAL.DeleteExternalLogRecordsByDate(deleteDate).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to delete log records from the database. Method: {0},  Exception: {1}, Trace{2}",
                    "DeleteExternalLogRecordsByDate", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Log

        #region Queue

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets configuration by queue name. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="queueName">    Name of the queue. </param>
        ///
        /// <returns>   An asynchronous result that yields the configuration by queue name. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<QueueConfiguration> GetConfigurationByQueueName(string queueName)
        {
            try
            {
                return await new QueueConfigurationDAL().GetConfigurationByQueueName(queueName);
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to retreive the queue configuration for the {0} queue. Method: {1},
                    Exception: {2}, Trace{3}", queueName, "GetConfigurationByQueueName", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets active queue names. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields the active queue names. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<IEnumerable<String>> GetActiveQueueNames()
        {
            try
            {
                return await new QueueConfigurationDAL().GetActiveQueueNames();
            }
            catch (Exception ex)
            {
                string error = string.Format(@"An Exception occurred while trying to retreive the active queues. Method: {0},
                    Exception: {1}, Trace{2}", "GetActiveQueueNames", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an error queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueue">   Queue of errors. </param>
        ///
        /// <returns>   An asynchronous result that yields an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<int> SaveErrorQueue(ErrorQueueLog errorQueue)
        {
            try
            {
                return await ErrorQueueDAL.SaveErrorQueue(errorQueue);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save a error queue to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SaveErrorQueue", ex.Message, ex.StackTrace);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an error task to dead queue. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="errorQueueTask">   The error queue task. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task AddErrorTaskToDeadQueue(ErrorQueueTask errorQueueTask)
        {
            try
            {
                await DeadQueueDAL.AddErrorTaskToDeadQueue(errorQueueTask).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save an error task to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "AddErrorTaskToDeadQueue", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Queue

        #region Azure Customer

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByMemberId(string memberId)
        {
            try
            {

                return await AzureCustomerDAL.GetCustomerByMemberId(memberId).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by memberId. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetCustomerByMemberId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by external identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="provider"> The provider. </param>
        /// <param name="userid">   The userid. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by external identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByExternalId(string provider, string userid)
        {
            try
            {

                return await AzureCustomerDAL.GetCustomerByExternalLoginAsync(provider, userid).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by memberId. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetCustomerByExternalId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by email. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">    The email. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by email. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByEmail(string email)
        {
            try
            {
                return await AzureCustomerDAL.GetCustomerByEmail(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetCustomerByEmail", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="PhoneNumber">  The phone number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by phone number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByPhoneNumber(string PhoneNumber)
        {
            try
            {
                return await AzureCustomerDetailDAL.GetCustomerByPhoneNumber(PhoneNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCustomerByPhoneNumber", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by email or phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="EmailOrPhoneNumber">   The email or phone number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by email or phone number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AzureCustomer>> GetCustomerByEmailOrPhoneNumber(string EmailOrPhoneNumber)
        {
            try
            {
                return await AzureCustomerDAL.GetCustomersByEmailOrPhone(EmailOrPhoneNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCustomerByPhoneNumber", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailOrPhoneNumber"></param>
        /// <returns></returns>
        public static async Task<List<AzureCustomer>> GetCustomerByEmailOrPhoneNumberMobile(string EmailOrPhoneNumber)
        {
            try
            {
                return await AzureCustomerDAL.GetCustomersByEmailOrPhoneMobile(EmailOrPhoneNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCustomerByPhoneNumber", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by alias number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber">  The alias number. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by alias number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByAliasNumber(string AliasNumber)
        {
            try
            {
                return await AzureCustomerDetailDAL.GetCustomerByAliasNumber(AliasNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCustomerByAliasNumber", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by CRC. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="CRC">  The CRC. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomerDetail> GetCustomerByCRC(string CRC)
        {
            try
            {
                return await AzureCustomerDetailDAL.GetCustomerByCRC(CRC).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by email. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCustomerByCRC", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerDetail">   The customer detail. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveCustomer(AzureCustomerDetail customerDetail)
        {
            try
            {
                await AzureCustomerDetailDAL.SaveCustomerDetail(customerDetail).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save an Azure customer to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SaveCustomer", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        public static async Task SaveCustomer(AzureCustomer customer)
        {
            try
            {
                await AzureCustomerDAL.SaveCustomer(customer).ConfigureAwait(false);
                if (customer.AzureCustomerBannerMetadata != null && customer.AzureCustomerBannerMetadata.Any())
                {
                    foreach (var metadata in customer.AzureCustomerBannerMetadata)
                    {
                        await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(metadata).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save an Azure customer to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SaveCustomer", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="azureCustomer"></param>
        /// <returns></returns>
        public static async Task SaveAzureCustomer(AzureCustomer azureCustomer)
        {
            try
            {
                await AzureCustomerDAL.SaveCustomerMobile(azureCustomer).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save an Azure customer to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SaveCustomer", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts a customer banner metadata described by customerBannerMetadata.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerBannerMetadata">   The customer banner metadata. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task InsertCustomerBannerMetadata(AzureCustomerBannerMetadata customerBannerMetadata)
        {
            try
            {
                await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(customerBannerMetadata).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to insert an Azure customer Banner Metadata to the database. Method: {0},  Exception: {1}, Trace: {2}",
                    "InsertCustomerBannerMetadata", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get Application Setting.
        /// </summary>
        ///
        /// <remarks>   Mark 7/28/2020. </remarks>
        ///
        /// <param name="key">   Template id. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<ApplicationSetting>> GetApplicationSetting(string key)
        {
            try
            {
                return await ApplicationSettingDAL.GetApplicationSetting(key).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get an Application Setting to the database. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetApplicationSetting", ex.Message, ex.StackTrace);
                //Logging.Error(error, ex);
                throw;
            }
        }

        #endregion Azure Customer

        #region Store

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the store. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        /// <param name="storeId">  Identifier for the store. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> UpdateStore(string memberId, string chainId, int storeId)
        {
            try
            {
                await AzureCustomerDAL.UpdateStore(memberId, chainId, storeId).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to update an store to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "UpdateStore", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Store

        #region Coupons

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the coupon identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="chainId">      Identifier for the chain. </param>
        /// <param name="couponId">     Identifier for the coupon. </param>
        /// <param name="couponAlias">  The coupon alias. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> UpdateCouponId(string memberId, string chainId, string couponId, string couponAlias)
        {
            try
            {
                await AzureCustomerDAL.UpdateCouponId(memberId, chainId, couponId, couponAlias).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to update an store to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "UpdateStore", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the SEG categories. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<string>> GetCategories()
        {
            try
            {
                return await CategoryDAL.GetCategories().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the SEG categories from the database. Method: {0},  Exception: {1}, Trace{2}",
                    "GetCategories", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Coupons

        #region Shopping List

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the shopping list identifier by member identifier and chain identifier.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">         Identifier for the member. </param>
        /// <param name="chainId">          Identifier for the chain. </param>
        /// <param name="shoppingListId">   Identifier for the shopping list. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> UpdateShoppingListIdByMemberIdAndChainId(string memberId, string chainId, string shoppingListId)
        {
            try
            {
                await AzureCustomerDAL.UpdateShoppingListIdByMemberIdAndChainId(memberId, chainId, shoppingListId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to update an shoppinglist id to the database. Method: {0},  Exception: {1}, Trace: {2}",
                    "UpdateShoppingList", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the shopping list identifier by email and chain identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">            The email. </param>
        /// <param name="chainId">          Identifier for the chain. </param>
        /// <param name="shoppingListId">   Identifier for the shopping list. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task UpdateShoppingListIdByEmailAndChainId(string email, string chainId, string shoppingListId)
        {
            try
            {
                await AzureCustomerDAL.UpdateShoppingListIdByEmailAndChainId(email, chainId, shoppingListId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to update an shoppinglist id to the database. Method: {0},  Exception: {1}, Trace: {2}",
                    "UpdateShoppingList", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion Shopping List

        #region Mobile

        #region Read

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> Identifier for the device. </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<UserDetail> GetUserDetailByDeviceId(string deviceId, string chainId)
        {
            try
            {
                return await MobileDeviceDAL.GetUserDetailByDeviceId(deviceId, chainId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get user details from the database by deviceId. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetUserDetailByDeviceId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Links an external login. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="userid">   The userid. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task LinkExternalLogin(string memberId, string provider, string userid)
        {
            await AzureCustomerExernalLoginsDAL.LinkExternalReferencesAsync(memberId, provider, userid).ConfigureAwait(false);
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
            try
            {
                return await MobileDeviceDAL.GetDeviceTokensByMemberId(memberId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Device Tokens from the database by memberId. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetDeviceTokenByMemberId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
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
            try
            {
                return await DeviceDAL.CheckDeviceId(deviceId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to check deviceId from the database by deviceId. Method: {0}, Exception: {1}, Trace: {2}",
                    "CheckDeviceId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
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
            try
            {
                return await MobileDeviceDAL.GetMemberIdByDeviceId(deviceId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get memberId from the database by deviceId. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetDeviceTokenByMemberId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SEG.ApiService.Models.Offers.CustomerEnrollmentOffer>> GetCustomerEnrollmentOffer(DateTime currentDate, string chainId)
        {
            try
            {
                return await CustomerEnrollmentOfferDAL.getCustomerEnrollmentOffer(currentDate, chainId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerEnrollmentOffer from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerEnrollmentOffer", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SEG.ApiService.Models.Offers.CustomerEnrollmentOffer>> GetCustomerEnrollmentOfferEE(DateTime currentDate, string chainId)
        {
            try
            {
                return await CustomerEnrollmentOfferDAL.getCustomerEnrollmentOfferEE(currentDate, chainId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerEnrollmentOfferEE from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerEnrollmentOfferEE", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// GetCustomerPromoSliders
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerPromoSliders()
        {
            try
            {
                return await CustomerRewardPromoDAL.GetCustomerRewardPromo().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerRewardPromo from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerPromoSliders", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// GetCustomerPromoSliders
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV2>> GetCustomerPromoSlidersV2() // updated to V3
        {
            try
            {
                // Return is filtered by date in DAL
                return await CustomerRewardPromoDAL.GetCustomerRewardPromoV2().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerRewardPromo from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerPromoSliders", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// GetCustomerPromoSliders
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV3>> GetCustomerPromoSlidersV3(string language = null)
        {
            try
            {
                return await CustomerRewardPromoDAL.GetCustomerRewardPromoV3(language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerRewardPromo from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerPromoSliders", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="chainId"></param>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerRewardPromoDealOfWeek(DateTime currentDate, string chainId)
        {
            try
            {
                return await CustomerRewardPromoDAL.GetCustomerRewardPromoDealOfWeek(currentDate, chainId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get GetCustomerRewardPromoDealOfWeek from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerRewardPromoDealOfWeek", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// GetRewardPromoActions
        /// </summary>
        /// <returns></returns>
        public static async Task<List<RewardPromoAction>> GetRewardPromoActions()
        {
            try
            {
                return await CustomerRewardPromoDAL.GetRewardPromoActions().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerRewardPromo from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerPromoSliders", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// GetRewardPromoActions
        /// </summary>
        /// <returns></returns>
        public static async Task<List<RewardPromoTile>> GetRewardPromoTiles()
        {
            try
            {
                return await CustomerRewardPromoDAL.GetRewardPromoTiles().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustomerRewardPromo from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetRewardPromoTiles", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// GetCustomerRewardPromoById
        /// </summary>
        /// <returns></returns>
        public static async Task<CustomerRewardPromo> GetCustomerRewardPromoById(Guid id)
        {
            try
            {
                return await CustomerRewardPromoDAL.GetCustomerRewardPromoById(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get a single reward from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustomerRewardPromoById", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// InsertCustomerRewardPromo
        /// </summary>
        /// <returns></returns>
        public static async Task<int> InsertCustomerRewardPromo(CustomerRewardPromo customerRewardPromo)
        {
            try
            {
                return await CustomerRewardPromoDAL.InsertCustomerRewardPromo(customerRewardPromo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to insert Rewards into the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "InsertCustomerRewardPromo", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// DeleteCustomerRewardPromo
        /// </summary>
        /// <returns></returns>
        public static async Task<int> DeleteCustomerRewardPromo(Guid id)
        {
            try
            {
                return await CustomerRewardPromoDAL.DeleteCustomerRewardPromo(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to delete Rewards from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "DeleteCustomerRewardPromo", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// UpdateCustomerRewardPromo
        /// </summary>
        /// <returns></returns>
        public static async Task<int> UpdateCustomerRewardPromo(CustomerRewardPromo customerRewardPromo)
        {
            try
            {
                return await CustomerRewardPromoDAL.UpdateCustomerRewardPromo(customerRewardPromo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates Rewards from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "UpdateCustomerRewardPromo", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// UpdateCustomerRewardPromo
        /// </summary>
        /// <returns></returns>
        public static async Task<int> GetAppVersion(string chainId)
        {
            try
            {
                return await LoyaltyMobileDAL.GetAppVersion(chainId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates LoyaltyMobileDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetAppVersion", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// UpdateCustomerRewardPromo
        /// </summary>
        /// <returns></returns>
        public static async Task<List<AppVersionV2Response>> GetAppVersionV2()
        {
            try
            {
                return await LoyaltyMobileDAL.GetAppVersionV2().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates LoyaltyMobileDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetAppVersionV2", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// SaveSMSCode
        /// </summary>
        /// <returns>EncryptSMSCode</returns>
        public static async Task SaveSMSCode(EncryptSMSCode code)
        {
            try
            {
                await EncryptSMSCodeDAL.SaveSMSCode(code).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates EncryptSMSCodeDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "SaveSMSCode", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// GetSMSCode
        /// </summary>
        /// <returns>phoneNumber</returns>
        public static async Task<EncryptSMSCode> GetSMSCode(string phoneNumber)
        {
            try
            {
                return await EncryptSMSCodeDAL.GetSMSCode(phoneNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates EncryptSMSCodeDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetSMSCode", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);
                throw;
            }
        }

        /// <summary>
        /// SearchAppReviewStatus
        /// </summary>
        /// <returns>AppReviewStatusRequest</returns>
        public static async Task<AppReviewStatusRequest> SearchAppReviewStatus(string memberId)
        {
            try
            {
                return await AppReviewDAL.SearchAppReviewStatus(memberId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates AppReviewDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "SearchAppReviewStatus", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);
                throw;
            }
        }

        /// <summary>
        /// SaveAppReviewStatus
        /// </summary>
        /// <returns>Nothing</returns>
        public static async Task UpdateAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest)
        {
            try
            {
                await AppReviewDAL.UpdateAppReviewStatus(appReviewStatusRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to updates AppReviewDAL from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "UpdateAppReviewStatus", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);
                throw;
            }
        }

        /// <summary>
        /// SaveAppReviewStatus
        /// </summary>
        /// <returns>Nothing</returns>
        public static async Task SaveAppReviewStatus(AppReviewStatusRequest appReviewStatusRequest)
        {
            try
            {
                await AppReviewDAL.SaveAppReviewStatus(appReviewStatusRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save AppReviewDAL to the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "SaveAppReviewStatus", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);
                throw;
            }
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
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveDevice(Device mobileDevice)
        {
            try
            {
                await DeviceDAL.SaveDevice(mobileDevice).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a push subscribe request. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId">     Identifier for the device. </param>
        /// <param name="deviceToken">  The device token. </param>
        /// <param name="type">         The type. </param>
        /// <param name="channel">      The channel. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> AddPushSubscribeRequest(string deviceId, string deviceToken, string type, string channel)
        {
            bool success = true;
            try
            {
                await MobileDeviceDAL.SavePushSubscription(deviceId, deviceToken, type, channel).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save a push Notification request to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SavePushNotification", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                success = false;
            }

            return success;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the push subscribe channel request. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> Identifier for the device. </param>
        /// <param name="channel">  The channel. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> DeletePushSubscribeChannelRequest(string deviceId, string channel)
        {
            bool success = true;
            try
            {
                await MobileDeviceDAL.DeletePushSubscriptionChannel(deviceId, channel).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to delete a push Notification request to the database. Method: {0},  Exception: {1}, Trace{2}",
                    "SavePushNotification", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                success = false;
            }

            return success;
        }




        #endregion Save

        #endregion Mobile

        #region CustPhoneLookup

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static async Task<CustPhoneLookup> GetCustPhoneLookup(string phoneNumber)
        {
            try
            {

                return await AzureCustPhoneLookUpDAL.GetCustPhoneLookup(phoneNumber).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get CustPhoneLookup from the database by phoneNumber. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetCustPhoneLookup", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="custPhoneLookup"></param>
        /// <returns></returns>
        public static async Task<CustPhoneLookup> SaveCustPhoneLookup(CustPhoneLookup custPhoneLookup)
        {
            try
            {

                return await AzureCustPhoneLookUpDAL.SaveCustPhoneLookUp(custPhoneLookup).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get Azure customer from the database by memberId. Method: {0},  Exception: {1}, Trace: {2}",
                    "GetCustomerByMemberId", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }



        #endregion CustPhoneLookup

        #region Token
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="token"> Identifier for the device. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task SaveJwtToken(JwtToken token)
        {
            try
            {
                await JwtTokenDAL.SaveJwtToken(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save JwtToken to the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "SaveJwtToken", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }
        #endregion

        #region MBOCongratsData
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="mboCongratsData"> Identifier for the device. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task InsertMBOCongratsData(MBOCongratsData mboCongratsData)
        {
            try
            {
                await MBOCongratsDataDAL.InsertMBOCongratsData(mboCongratsData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to save MBOCongratsDataDAL to the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "InsertMBOCongratsData", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets user detail by device identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberID"> Identifier for the device. </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail by device identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<MBOCongratsData>> GetMBOCongratsData(string memberID)
        {
            try
            {
                return await MBOCongratsDataDAL.GetMBOCongratsData(memberID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the MBOCongratsData from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetMBOCongratsData", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static async Task<CustomerPersonalization> GetCustPersonalization(string memberID)
        {
            try
            {
                return await CustomerPersonalizationDAL.GetCustomerPersonalization(memberID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the Customer Personalzation from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCustPersonalization", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }



        #endregion

        #region AuditCustomerCareCenter Points 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<AuditCustomerServiceRep> GetAuditCustomerServiceRep(string userId)
        {
            try
            {
                return await AuditCustomerServiceRepDAL.GetAuditCustomerServiceRep(userId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the GetAuditCustomerServiceRep from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetAuditCustomerServiceRep", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditCustomerServiceRep"></param>
        /// <returns></returns>
        public static async Task<bool> InsertAuditCustomerServiceRep(AuditCustomerServiceRep auditCustomerServiceRep)
        {
            try
            {
                return await AuditCustomerServiceRepDAL.InsertAuditCustomerServiceRep(auditCustomerServiceRep).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the InsertAuditCustomerServiceRep from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "InsertAuditCustomerServiceRep", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public static async Task<AuditCustomerServiceTicket> GetAuditCustomerServiceTicket(string TicketNumber)
        {
            try
            {
                return await AuditCustomerServiceTicketDAL.GetAuditCustomerServiceTicket(TicketNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the GetAuditCustomerServiceTicket from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetAuditCustomerServiceTicket", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
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
                return await AuditCustomerServiceTicketDAL.InsertAuditCustomerServiceTicket(auditCustomerServiceTicket).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the InsertAuditCustomerServiceTicket from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "InsertAuditCustomerServiceTicket", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditProcessPoints"></param>
        /// <returns></returns>
        public static async Task<bool> InsertAuditProcessPoints(AuditProcessPoints auditProcessPoints)
        {
            try
            {
                return await AuditProcessPointsDAL.InsertAuditProcessPoints(auditProcessPoints).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the InsertAuditProcessPoints from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "InsertAuditProcessPoints", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }

        #endregion


        #region CampaignIssuanceCount 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="campaignID"></param>
        /// <returns></returns>
        public static async Task<Int64> GetCampaignIssuanceCount(string campaignID)
        {
            try
            {
                return await CampaignIssuanceCountDAL.getCampaignIssuanceCount(campaignID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the GetCampaignIssuanceCount from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetCampaignIssuanceCount", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }
        #endregion

        #region EReceiptsHtmlConfiguration 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<EReceiptsHtmlConfiguration> GetEReceiptsHtmlConfiguration(string key)
        {
            try
            {
                return await EReceiptsHtmlConfigurationDAL.GetEReceiptsHtmlConfiguration(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = string.Format("An Exception occurred while trying to get the EReceiptsHtmlConfiguration from the database. Method: {0}, Exception: {1}, Trace: {2}",
                    "GetEReceiptsHtmlConfiguration", ex.Message, ex.StackTrace);
                Logging.Error(error, ex);

                throw;
            }
        }
        #endregion

        #region Product Survey
        /// <summary>
        /// Save Product Survey
        /// </summary>
        public static async Task<bool> SaveProductSurveyAsync(ProductSurvey survey)
        {
            return await ProductSurveyDAL.SaveProductSurveyAsync(survey);
        }

        /// <summary>
        /// Get Product Survey by Member Id
        /// </summary>
        public static async Task<List<ProductSurvey>> GetProductSurveyByMemberIdAsync(string memberId)
        {
            return await ProductSurveyDAL.GetProductSurveyByMemberIdAsync(memberId);
        }
        #endregion Product Survey

        #region Purge Data
        /// <summary>
        /// Porge MBO Inssuance Data 
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> PurgeMBOInssuance()
        {
            return await MBOIssuanceCleanupDAL.PurgeMBOInssuance();
        }



        #endregion


        #endregion Public Methods
    }
}