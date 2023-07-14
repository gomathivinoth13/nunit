using log4net;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SalesForceLibrary.Models;
using SalesForceLibrary.Queue;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Queueing;
using SEG.ApiService.Models.SalesForce;
using SEG.SalesForce.Controllers;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEG.SalesForce
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceService
    {
        #region Static Variables
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Static Variables  

        /// <summary>   The manage access token. </summary>
        ManageAccessToken manageAccessToken;

        /// <summary>   The service dal. </summary>
        SalesForceRestDAL serviceDAL;
       
        /// <summary>
        /// 
        /// </summary>
        public SalesForceService(string baseRestUrl, string accountId, string clientID, string clientSecret)
        {
            serviceDAL = new SalesForceRestDAL(baseRestUrl, clientID, clientSecret);
            manageAccessToken = new ManageAccessToken(baseRestUrl, clientID, clientSecret);

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsync(DataExtentionsRequest dataExtentionsRequest)
        //{
        //    DataExtentionsResponse dataExtentionsResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            dataExtentionsResponse = await serviceDAL.UpsertAsync(accessToken, dataExtentionsRequest).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_UpsertAsync.  Error {0}", ex.Message), ex);
        //        //throw;
        //    }

        //    return dataExtentionsResponse;
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsyncBabyClub(DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest)
        //{
        //    DataExtentionsResponse dataExtentionsResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            dataExtentionsResponse = await serviceDAL.UpsertAsyncBabyClub(accessToken, dataExtentionsBabyClubChildRequest).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_UpsertAsyncBabyClub.  Error {0}", ex.Message), ex);
        //        //throw;
        //    }

        //    return dataExtentionsResponse;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsyncPreferences(DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest)
        //{
        //    DataExtentionsResponse dataExtentionsResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            dataExtentionsResponse = await serviceDAL.UpsertAsyncPreferences(accessToken, dataExtentionsCustomerPreferenceRequest).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_UpsertAsyncBabyClub.  Error {0}", ex.Message), ex);
        //        //throw;
        //    }

        //    return dataExtentionsResponse;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsync(DataExtentionsRequest dataExtentionsRequest, int retryCount)
        //{
        //    DataExtentionsResponse dataExtentionsResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            dataExtentionsResponse = await serviceDAL.UpsertAsync(accessToken, dataExtentionsRequest).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_UpsertAsync.  Error {0}", ex.Message), ex);

        //        if (retryCount > 0 && retryCount <= 3)
        //        {
        //            retryCount--;
        //            dataExtentionsResponse = await UpsertAsync(dataExtentionsRequest, retryCount).ConfigureAwait(false);
        //        }
        //        else

        //            throw;
        //    }

        //    return dataExtentionsResponse;
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="messageKey"></param>
        ///// <param name="messaging"></param>
        ///// <returns></returns>
        ///// 
        //public async Task<MessagingResponse> Send(string messageKey, Messaging messaging)
        //{
        //    MessagingResponse messagingResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            messagingResponse = await serviceDAL.Send(accessToken, messageKey, messaging).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_Send email.  Error {0}", ex.Message), ex);
        //        // throw;
        //    }

        //    return messagingResponse;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="messageKey"></param>
        ///// <param name="messaging"></param>
        ///// <returns></returns>
        ///// 
        //public async Task<SmsMessagingResponse> SendSMS(string messageKey, SmsMessaging messaging)
        //{
        //    SmsMessagingResponse smsMessagingResponse = null;

        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            smsMessagingResponse = await serviceDAL.SendSMS(accessToken, messageKey, messaging).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_SendSMS.  Error {0}", ex.Message), ex);
        //        //throw;
        //    }

        //    return smsMessagingResponse;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="welcomeJourneyRequest"></param>
        ///// <returns></returns>
        //public async Task<WelcomeJourneyResponse> WelcomeJourney(WelcomeJourneyRequest welcomeJourneyRequest)
        //{
        //    WelcomeJourneyResponse welcomeJourneyResponse = null;


        //    try
        //    {
        //        string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            welcomeJourneyResponse = await serviceDAL.WelcomeJourney(accessToken, welcomeJourneyRequest).ConfigureAwait(false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Error(String.Format("An error occured while trying to run SalesForce_WelcomeJourney.  Error {0}", ex.Message), ex);
        //        //throw;
        //    }

        //    return welcomeJourneyResponse;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="welcomeJourneyRequest"></param>
        /// <returns></returns>
        public async Task<WelcomeJourneyResponse> PosWelcomeJourney(POSWelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;


            try
            {
                string accessToken = await manageAccessToken.PosGetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    welcomeJourneyResponse = await serviceDAL.PosWelcomeJourney(accessToken, welcomeJourneyRequest).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An error occured while trying to run SalesForce_WelcomeJourney.  Error {0}", ex.Message), ex);
            }

            return welcomeJourneyResponse;
        }
    }
}
