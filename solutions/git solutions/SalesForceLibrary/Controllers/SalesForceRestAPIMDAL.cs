using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SalesForceLibrary.Models;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.Surveys;
using SEG.SalesForce.Models;

namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceRestAPIMDAL
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
        public string ocpApimSubscriptionKeySecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseRestUrl"></param>
        /// <param name="ClientID"></param>
        /// <param name="ClientSecret"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public SalesForceRestAPIMDAL(string BaseRestUrl, string ClientID, string ClientSecret, string ocpApimSubscriptionKey)
        {
            clientId = ClientID;
            clientSecret = ClientSecret;
            baseRestUrl = BaseRestUrl;
            ocpApimSubscriptionKeySecret = ocpApimSubscriptionKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ContactResponse> DeletePiiAsync(string accessToken, ContactRequest request)
        {
            try
            {
                var path = "contacts/v1/contacts/actions/delete";
                var qParams = new Dictionary<string, object> { { "type", "keys" } };

                var dataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<ContactResponse>(HttpMethod.Post, request, path, baseRestUrl, qParams, createHeader(accessToken)).ConfigureAwait(false)).Result;

                return dataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncProductSurvey(string accessToken, DataExtensionsProductSurveyRequest surveyRequest, string keyBU)
        {

            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, surveyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        ///  Upsert the Campaign Data from EE to SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEECampaign(string accessToken, DataExtensionsEECampaignRequest campaignRequest, string keyBU)
        {

            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, campaignRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        ///  Upsert the Group Data from EE to SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEEGroupData(string accessToken, DataExtensionsEEGroupRequest dataExtensionsEEGroupRequest, string keyBU)
        {

            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtensionsEEGroupRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Upsert the Tag Data from EE to SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEETagData(string accessToken, DataExtensionsEETagRequest dataExtensionsEETagRequest, string keyBU)
        {

            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtensionsEETagRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///  Upsert the Tag Data from EE to SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEEUpcData(string accessToken, DataExtensionsEEUPCRequest dataExtensionsEEUPCRequest, string keyBU)
        {

            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtensionsEEUPCRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenV2> APIAccesstokenGenerate()
        {
            try
            {
                AccessTokenRequest request = new AccessTokenRequest();
                request.clientId = clientId;
                request.clientSecret = clientSecret;

                var apiAccessToken1 = (await SEG.Shared.ApiUtility.RestfulCallAsync<AccessTokenV2>(HttpMethod.Post, request, "/v1/requestToken", baseRestUrl, Headers: createHeader()).ConfigureAwait(false));

                return apiAccessToken1.Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="messageID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Shared.Response<pushNotificationResponse>> SendPushNotification(string accessToken, string messageID, SendPushNotificationRequest request)
        {
            Shared.Response<pushNotificationResponse> response = null;
            try
            {
                string pathParameters = string.Format("/push/v1/messageContact/{0}/send", messageID);



                response = (await SEG.Shared.ApiUtility.RestfulCallAsync<pushNotificationResponse>(HttpMethod.Post, request, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false));

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="welcomeJourneyRequest"></param>
        /// <returns></returns>
        public async Task<WelcomeJourneyResponse> WelcomeJourneyMBO(string accessToken, MBOIssuanceJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string pathParameters = string.Format("/interaction/v1/events");

                welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<WelcomeJourneyResponse>(HttpMethod.Post, welcomeJourneyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return welcomeJourneyResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dataExtentionsRequest"></param>
        /// <param name="keyBU"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsync(string accessToken, DataExtentionsRequest dataExtentionsRequest, string keyBU)
        {
            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> InsertAccountId(string accessToken, WalletAccountDataModel dataExtentionsRequest, string keyBU)
        {
            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Post, dataExtentionsRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dataExtentionsBabyClubChildRequest"></param>
        /// <param name="keyBU"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncBabyClub(string accessToken, DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest, string keyBU)
        {
            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsBabyClubChildRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dataExtentionsCustomerPreferenceRequest"></param>
        /// <param name="keyBU"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncPreferences(string accessToken, DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest, string keyBU)
        {
            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsCustomerPreferenceRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dataExtentionsPetClubChildRequest"></param>
        /// <param name="keyBU"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncPetClub(string accessToken, DataExtentionsPetClubChildRequest dataExtentionsPetClubChildRequest, string keyBU)
        {
            DataExtentionsResponse bataExtentionsResponse = null;
            try
            {
                string pathParameters = string.Format("data/v1/async/dataextensions/key:{0}/rows", keyBU);

                bataExtentionsResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<DataExtentionsResponse>(HttpMethod.Put, dataExtentionsPetClubChildRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return bataExtentionsResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="messageKey"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public async Task<MessagingResponse> Send(string accessToken, string messageKey, Messaging messaging)
        {
            MessagingResponse messagingResponse = null;
            try
            {
                string pathParameters = string.Format("/messaging/v1/messageDefinitionSends/key:{0}/send", messageKey);

                messagingResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<MessagingResponse>(HttpMethod.Post, messaging, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return messagingResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        /// <summary>
        /// send SMS 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="messageKey"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public async Task<SmsMessagingResponse> SendSMS(string accessToken, string messageKey, SmsMessaging messaging)
        {
            SmsMessagingResponse smsMessagingResponse = null;
            try
            {
                string pathParameters = string.Format("sms/v1/messageContact/{0}/send", messageKey);

                smsMessagingResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<SmsMessagingResponse>(HttpMethod.Post, messaging, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return smsMessagingResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// WelcomeJourney 
        /// </summary>

        public async Task<WelcomeJourneyResponse> WelcomeJourney(string accessToken, WelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string pathParameters = string.Format("/interaction/v1/events");

                welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<WelcomeJourneyResponse>(HttpMethod.Post, welcomeJourneyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return welcomeJourneyResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// PosWelcomeJourney
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="welcomeJourneyRequest"></param>
        /// <returns></returns>
        public async Task<WelcomeJourneyResponse> PosWelcomeJourney(string accessToken, POSWelcomeJourneyRequest welcomeJourneyRequest)
        {

            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string pathParameters = string.Format("/interaction/v1/events");

                welcomeJourneyResponse = (await SEG.Shared.ApiUtility.RestfulCallAsync<WelcomeJourneyResponse>(HttpMethod.Post, welcomeJourneyRequest, pathParameters, baseRestUrl, Headers: createHeader(accessToken)).ConfigureAwait(false)).Result;

                return welcomeJourneyResponse;
            }
            catch (Exception ex)

            {
                throw;
            }
        }

        private Dictionary<string, object> createHeader(string accessToken)
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("Authorization", $"Bearer {accessToken}");
            headers.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKeySecret);
            return headers;
        }

        private Dictionary<string, object> createHeader()
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKeySecret);
            return headers;
        }

    }
}
