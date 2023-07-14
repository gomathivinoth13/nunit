using Flurl.Http;
using log4net;
using SalesForceLibrary.Models;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.Surveys;
using SEG.SalesForce.Controllers;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceLibrary.SalesForceAPIM
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceAPIMService
    {
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ManageAccessTokenV2 manageAccessToken;

        SalesForceRestAPIMDAL serviceDAL;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseRestUrlAuth"></param>
        /// <param name="baseRestUrl"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="cacheConnectionString"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public SalesForceAPIMService(string baseRestUrlAuth, string baseRestUrl, string clientID, string clientSecret, string cacheConnectionString, string ocpApimSubscriptionKey)
        {
            serviceDAL = new SalesForceRestAPIMDAL(baseRestUrl, clientID, clientSecret, ocpApimSubscriptionKey);
            manageAccessToken = new ManageAccessTokenV2(baseRestUrlAuth, clientID, clientSecret, cacheConnectionString, ocpApimSubscriptionKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ContactResponse> DeletePiiAsync(string memberId)
        {
            ContactResponse response = null;
            try
            {
                var request = new ContactRequest
                {
                    Values = new List<string> { memberId }
                };
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {                    
                    response = await serviceDAL.DeletePiiAsync(accessToken, request).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertProductSurveyAsync(ProductSurvey survey, string keyBU)
        {
            DataExtentionsResponse response = null;
            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    var surveyItem = new ProductSurveyItem
                    {
                        Banner = survey.Banner,
                        BuyItAgain = survey.BuyItAgain,
                        MemberId = survey.MemberId,
                        CrcId = survey.CrcId,
                        LocId = survey.LocId,
                        UpcCode = survey.UpcCode,
                        TransactionDateTime = survey.TransactionDateTime,
                        Satisfaction = survey.Satisfaction,
                        Size = survey.Size,
                        Taste = survey.Taste,
                        Packaging = survey.Packaging,
                        VisualAppeal = survey.VisualAppeal,
                        Comments = survey.Comments
                    };
                    var surveyRequest = new DataExtensionsProductSurveyRequest();
                    surveyRequest.items.Add(surveyItem);
                    response = await serviceDAL.UpsertAsyncProductSurvey(accessToken, surveyRequest, keyBU);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        /// <summary>
        ///  Upsert the EE ProductData To SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEEProductData(DataExtensionsEEGroupRequest dataExtensionsEEGroupRequest,DataExtensionsEETagRequest dataExtensionsEETagRequest,DataExtensionsEEUPCRequest dataExtensionsEEUPCRequest, string keyGroup,string keyTag, string keyUpc)
        {
            DataExtentionsResponse response = null;
            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                  if (dataExtensionsEEUPCRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEEUpcData(accessToken, dataExtensionsEEUPCRequest, keyUpc);
                    }
                  if (dataExtensionsEETagRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEETagData(accessToken, dataExtensionsEETagRequest, keyTag);
                    }
                  if (dataExtensionsEEGroupRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEEGroupData(accessToken, dataExtensionsEEGroupRequest, keyGroup);                            
                    }                
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return response;
        }



        /// <summary>
        ///  Upsert the EE ProductData To SFMC
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncEECampaign(DataExtensionsEECampaignRequest eeCampaign, string keyBU)
        {
            DataExtentionsResponse response = null;
            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    response = await serviceDAL.UpsertAsyncEECampaign(accessToken, eeCampaign, keyBU);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<SEG.Shared.Response<pushNotificationResponse>> SendPushNotification(string messageID, SendPushNotificationRequest request)
        {
            SEG.Shared.Response<pushNotificationResponse> response = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    response = await serviceDAL.SendPushNotification(accessToken, messageID, request).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<WelcomeJourneyResponse> SendJourney(MBOIssuanceJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse response = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    response = await serviceDAL.WelcomeJourneyMBO(accessToken, welcomeJourneyRequest).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncBabyClub(DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest, bool babyClubFlag, string memberID, string keyBU, string babyKeyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;

            try
            {

                if (!string.IsNullOrEmpty(memberID))
                {
                    string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        DataExtentionsRequest request = new DataExtentionsRequest();
                        Item item = new Item();
                        item.MEMBER_ID = memberID;
                        if (babyClubFlag)
                        {
                            item.Baby_Club_Flag = "Y";
                        }
                        else
                        {
                            item.Baby_Club_Flag = "N";
                        }

                        List<Item> items = new List<Item>();
                        items.Add(item);
                        request.items = items;

                        var result = await serviceDAL.UpsertAsync(accessToken, request, keyBU).ConfigureAwait(false);
                        if (dataExtentionsBabyClubChildRequest != null && dataExtentionsBabyClubChildRequest.items != null && dataExtentionsBabyClubChildRequest.items.Count > 0)
                            dataExtentionsResponse = await serviceDAL.UpsertAsyncBabyClub(accessToken, dataExtentionsBabyClubChildRequest, babyKeyBU).ConfigureAwait(false);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return dataExtentionsResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncPetClub(DataExtentionsPetClubChildRequest dataExtentionsPetClubChildRequest, bool petClubFlag, string memberID, string keyBU, string petKeyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;

            try
            {
                if (!string.IsNullOrEmpty(memberID))
                {
                    string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        DataExtentionsRequest request = new DataExtentionsRequest();
                        Item item = new Item();
                        item.MEMBER_ID = memberID;
                        if (petClubFlag)
                        {
                            item.Pet_Club_Flag = "Y";
                        }
                        else
                        {
                            item.Pet_Club_Flag = "N";
                        }

                        List<Item> items = new List<Item>();
                        items.Add(item);
                        request.items = items;

                        var result = await serviceDAL.UpsertAsync(accessToken, request, keyBU).ConfigureAwait(false);
                        if (dataExtentionsPetClubChildRequest != null && dataExtentionsPetClubChildRequest.items != null && dataExtentionsPetClubChildRequest.items.Count > 0)
                            dataExtentionsResponse = await serviceDAL.UpsertAsyncPetClub(accessToken, dataExtentionsPetClubChildRequest, petKeyBU).ConfigureAwait(false);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return dataExtentionsResponse;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsync(DataExtentionsRequest dataExtentionsRequest, string keyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    dataExtentionsResponse = await serviceDAL.UpsertAsync(accessToken, dataExtentionsRequest, keyBU).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return dataExtentionsResponse;
        }
        public async Task<DataExtentionsResponse> InsertAccountId(WalletAccountDataModel dataExtentionsRequest, string keyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    dataExtentionsResponse = await serviceDAL.InsertAccountId(accessToken, dataExtentionsRequest, keyBU).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return dataExtentionsResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> UpsertAsyncPreferences(DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest, string keyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    dataExtentionsResponse = await serviceDAL.UpsertAsyncPreferences(accessToken, dataExtentionsCustomerPreferenceRequest, keyBU).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return dataExtentionsResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageKey"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        /// 
        public async Task<MessagingResponse> Send(string messageKey, Messaging messaging)
        {
            MessagingResponse messagingResponse = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    messagingResponse = await serviceDAL.Send(accessToken, messageKey, messaging).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return messagingResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageKey"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        /// 
        public async Task<SmsMessagingResponse> SendSMS(string messageKey, SmsMessaging messaging)
        {
            SmsMessagingResponse smsMessagingResponse = null;

            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    smsMessagingResponse = await serviceDAL.SendSMS(accessToken, messageKey, messaging).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return smsMessagingResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="welcomeJourneyRequest"></param>
        /// <returns></returns>
        public async Task<WelcomeJourneyResponse> WelcomeJourney(WelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;


            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    welcomeJourneyResponse = await serviceDAL.WelcomeJourney(accessToken, welcomeJourneyRequest).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return welcomeJourneyResponse;
        }

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
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(false);

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