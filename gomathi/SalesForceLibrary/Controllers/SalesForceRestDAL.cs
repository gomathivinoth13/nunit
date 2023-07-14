using Flurl.Http;
using SalesForceLibrary.Models;
using SalesForceLibrary.Queue;
using SEG.ApiService.Models;
using SEG.ApiService.Models.SalesForce;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceRestDAL
    {
        /// <summary>
        /// 
        /// </summary>
        public string baseRestUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseRestUrl"></param>

        /// <param name="ClientID"></param>
        /// <param name="ClientSecret"></param>
        public SalesForceRestDAL(string BaseRestUrl, string ClientID, string ClientSecret)
        {
            clientId = ClientID;
            clientSecret = ClientSecret;
            baseRestUrl = BaseRestUrl;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<AccessTokenResponse> APIAccesstokenGenerate()
        //{
        //    AccessTokenResponse apiAccessToken = null;
        //    try
        //    {
        //        AccessTokenRequest request = new AccessTokenRequest();
        //        request.clientId = clientId;
        //        request.clientSecret = clientSecret;

        //        apiAccessToken = (await SEG.Shared.ApiUtility.RestfulCallAsync<AccessTokenResponse>(HttpMethod.Post, request, "v1/requestToken", baseRestUrl).ConfigureAwait(false)).Result;

        //        return apiAccessToken;
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // FuelRewardsError e = ex.GetResponseJson<FuelRewardsError>();
        //        throw;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenResponse> PosAPIAccesstokenGenerate()
        {
            AccessTokenResponse apiAccessToken = null;
            try
            {
                AccessTokenRequest request = new AccessTokenRequest();
                request.clientId = clientId;
                request.clientSecret = clientSecret;

                apiAccessToken = (await SEG.Shared.ApiUtility.RestfulCallAsync<AccessTokenResponse>(HttpMethod.Post, request, "/SFMCAuth/requestToken", baseRestUrl).ConfigureAwait(false)).Result;                
                return apiAccessToken;
            }
            catch (FlurlHttpException ex)
            {
                // FuelRewardsError e = ex.GetResponseJson<FuelRewardsError>();
                throw;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="dataExtentionsRequest"></param>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsync(string accessToken, DataExtentionsRequest dataExtentionsRequest)
        //{
        //    DataExtentionsResponse bataExtentionsResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

        //        bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return bataExtentionsResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="dataExtentionsBabyClubChildRequest"></param>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsyncBabyClub(string accessToken, DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest)
        //{
        //    DataExtentionsResponse bataExtentionsResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

        //        bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsBabyClubChildRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return bataExtentionsResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="dataExtentionsCustomerPreferenceRequest"></param>
        ///// <returns></returns>
        //public async Task<DataExtentionsResponse> UpsertAsyncPreferences(string accessToken, DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest)
        //{
        //    DataExtentionsResponse bataExtentionsResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

        //        bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsCustomerPreferenceRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return bataExtentionsResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="messageKey"></param>
        ///// <param name="messaging"></param>
        ///// <returns></returns>
        //public async Task<MessagingResponse> Send(string accessToken, string messageKey, Messaging messaging)
        //{
        //    MessagingResponse messagingResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("/messaging/v1/messageDefinitionSends/key:{0}/send", messageKey);

        //        messagingResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<MessagingResponse>(HttpMethod.Post, messaging, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return messagingResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}



        ///// <summary>
        ///// send SMS 
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="messageKey"></param>
        ///// <param name="messaging"></param>
        ///// <returns></returns>
        //public async Task<SmsMessagingResponse> SendSMS(string accessToken, string messageKey, SmsMessaging messaging)
        //{
        //    SmsMessagingResponse smsMessagingResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("sms/v1/messageContact/{0}/send", messageKey);

        //        smsMessagingResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<SmsMessagingResponse>(HttpMethod.Post, messaging, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return smsMessagingResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}



        ///// <summary>
        ///// WelcomeJourney 
        ///// </summary>

        //public async Task<WelcomeJourneyResponse> WelcomeJourney(string accessToken, WelcomeJourneyRequest welcomeJourneyRequest)
        //{
        //    WelcomeJourneyResponse welcomeJourneyResponse = null;
        //    try
        //    {
        //        string pathParameters = string.Format("/interaction/v1/events");

        //        welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<WelcomeJourneyResponse>(HttpMethod.Post, welcomeJourneyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

        //        return welcomeJourneyResponse;
        //    }
        //    catch (FlurlHttpException ex)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        ///POS WelcomeJourney 
        /// </summary>

        public async Task<WelcomeJourneyResponse> PosWelcomeJourney(string accessToken, POSWelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string pathParameters = string.Format("/SFMCJourneyAPI/events");

                welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<WelcomeJourneyResponse>(HttpMethod.Post, welcomeJourneyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return welcomeJourneyResponse;
            }
            catch (FlurlHttpException ex)
            {

                throw;
            }
        }

        private Dictionary<string, object> createHeader(string accessToken)
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("Authorization", $"Bearer {accessToken}");

            return headers;
        }
    }
}
