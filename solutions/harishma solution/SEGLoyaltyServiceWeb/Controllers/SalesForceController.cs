

using Microsoft.AspNetCore.Mvc;
using SEG.SalesForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SEG.SalesForce.Models;
using SalesForceLibrary.Models;
using SalesForceLibrary.Queue;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.SendGrid;
using SalesForceLibrary.SendSMSEmail;
using System.Net.Mail;
using Polly;
using System.Net.Http;
using SEG.ApiService.Models.Utility;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Reminder;
//using log4net;
using SEG.ApiService.Models;
using SalesForceLibrary.SalesForceAPIM;
using SalesForceLibrary.SendJourney;
using log4net;
using Newtonsoft.Json;
using SEG.ApiService.Models.Pii;

namespace SEGLoyaltyServiceWeb.Controllers
{
    /// <summary>
    /// sales force controller - sfmc
    /// </summary>
    public class SalesForceController : Controller
    {
        IConfiguration Configuration;
        SalesForceAPIMService salesForceService;
        //SalesForceService salesForceServiceBabyClub;
        // SalesForceService salesForceServicePreferences;
        SalesForceQueueProcess salesForceQueueProcess;
        SalesForceSMSEmail salesForceSMSEmail;
        const string BiloBanner = "bi-lo";
        const string WinndixieBanner = "winn dixie";
        const string HarveysBanner = "harveys";
        const string FrescoBanner = "fresco y mas";
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RetryWait { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public SalesForceController(IConfiguration configuration)
        {

            Configuration = configuration;

            RetryCount = Convert.ToInt32(Configuration["Settings:SalesForce:RetryCount"]);
            RetryWait = Convert.ToInt32(Configuration["Settings:SalesForce:RetryWait"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/SalesForce/DeleteSFMCContact")]
        public async Task<ContactResponse> DeleteSFMCContact(string memberId)
        {
            var contactResponse = new ContactResponse();
            SalesForceAPIMService salesForceServiceDelete;
            var env = Configuration["Settings:Environment"];
            if (env == "QA")
            {
                //Set constructor with SFMC PROD credentials
                salesForceServiceDelete = new SalesForceAPIMService(
                    Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"],
                    Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"],
                    Configuration["Settings:SalesForce:SEG_ClientID_delete"],
                    Configuration["Settings:SalesForce:SEG_ClientSecret_delete"],
                    Configuration["Settings:SalesForce:redisConnectionString"],
                    Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

                //Delete contact from SFMC QA using PROD credentials
                contactResponse = await salesForceServiceDelete.DeletePiiAsync(memberId).ConfigureAwait(false);
            }
            else if (env == "PROD")
            {
                //Set constructor with SFMC PROD credentials
                salesForceServiceDelete = new SalesForceAPIMService(
                    Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"],
                    Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"],
                    Configuration["Settings:SalesForce:SEG_ClientID"],
                    Configuration["Settings:SalesForce:SEG_ClientSecret"],
                    Configuration["Settings:SalesForce:redisConnectionString"],
                    Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

                //Delete contact from SFMC QA using PROD credentials
                contactResponse = await salesForceServiceDelete.DeletePiiAsync(memberId).ConfigureAwait(false);
            }
            // no condition for DEV environment because there's no SFMC DEV to delete from

            return contactResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataExtentionsRequestInsert"></param>
        /// <param name="opt_Out"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/UpsertAsync")]
        public async Task<DataExtentionsResponse> UpsertAsync([FromBody]DataExtentionsRequestInsert dataExtentionsRequestInsert, Boolean opt_Out)
        {
            try
            {
                DataExtentionsResponse dataExtentionsResponse = null;

                //insert into master DE 
                if (dataExtentionsRequestInsert != null && dataExtentionsRequestInsert.dataExtentionsRequest != null && dataExtentionsRequestInsert.dataExtentionsRequest.items != null)
                {
                    if (dataExtentionsRequestInsert.dataExtentionsRequest.items.Count() > 0)
                    {
                        salesForceService = new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:SEG_ClientID"], Configuration["Settings:SalesForce:SEG_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

                        //if customer opt_ Out set to true . save them in staging environment . else save them in regular SEG BU . 
                        if (opt_Out)
                        {
                            dataExtentionsResponse = await Policy
                     .HandleResult<DataExtentionsResponse>(message => message == null || String.IsNullOrEmpty(message.requestId) || !string.IsNullOrEmpty(message.errorcode))
                       .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(RetryWait))
                       .ExecuteAsync(() => salesForceService.UpsertAsync(dataExtentionsRequestInsert.dataExtentionsRequest, Configuration["Settings:SalesForce:SEG_Key_Staging"]));
                        }
                        else
                        {
                            dataExtentionsResponse = await Policy
                     .HandleResult<DataExtentionsResponse>(message => message == null || String.IsNullOrEmpty(message.requestId) || !string.IsNullOrEmpty(message.errorcode))
                       .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(RetryWait))
                       .ExecuteAsync(() => salesForceService.UpsertAsync(dataExtentionsRequestInsert.dataExtentionsRequest, Configuration["Settings:SalesForce:SEG_Key"]));
                        }

                        if (dataExtentionsResponse == null || string.IsNullOrEmpty(dataExtentionsResponse.requestId) || !string.IsNullOrEmpty(dataExtentionsResponse.errorcode))
                        {
                            //* calling internal API to send email notifications
                            string json = SEG.Shared.Serializer.JsonSerialize<DataExtentionsRequest>(dataExtentionsRequestInsert.dataExtentionsRequest);
                            string subject = String.Format("SalesForce_Upsert failed to respond after Retry.");
                            await sendEmailInternally(json, subject, SEG.Shared.Serializer.JsonSerialize<DataExtentionsResponse>(dataExtentionsResponse)).ConfigureAwait(false);

                        }
                    }
                }

                return dataExtentionsResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in UpsertAsync :", e);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesForceQueueRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/QueueProcess")]
        public async Task QueueProcess([FromBody]SalesForceQueueRequest salesForceQueueRequest)
        {
            try
            {
                salesForceQueueProcess = new SalesForceQueueProcess(Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);
                await salesForceQueueProcess.ProcessSalesForceRequest(salesForceQueueRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception("Exception in QueueProcess :", e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messaging"></param>
        /// <param name="banner"></param>
        /// <param name="messageKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/Send")]
        public async Task<MessagingResponse> Send([FromBody] Messaging messaging, Banner banner, string messageKey)
        {
            try
            {
                salesForceService = setService(banner);

                MessagingResponse messagingResponse = await salesForceService.Send(messageKey, messaging).ConfigureAwait(false);


                return messagingResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in Send :", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        [HttpPost]
        [Route("api/SalesForce/SendEmail")]
        public async Task SendEmail([FromBody] MessageObject messageObject)
        {
            try
            {
                salesForceSMSEmail = new SalesForceSMSEmail(Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);
                await salesForceSMSEmail.SendEmail(messageObject).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception("Exception in SendEmail :", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messaging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/SendSms")]
        public async Task<SmsMessagingResponse> SendSms([FromBody] SmsMessaging messaging)
        {
            try
            {
                string messageKey;
                SmsMessagingResponse smsMessagingResponse = null;
                salesForceService = new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:SEG_ClientID_SMS"], Configuration["Settings:SalesForce:SEG_ClientSecret_SMS"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

                if (messaging != null && !string.IsNullOrEmpty(messaging.keyword))
                {
                    if (messaging.keyword.ToLower() == SFMCKeywordType.RESETPW.ToString().ToLower())
                    {
                        messageKey = Configuration["Settings:SalesForce:KeywordRESETPW"];
                    }
                    else
                    {
                        messageKey = Configuration["Settings:SalesForce:KeywordAUTHORIZATIONCODE"];

                    }

                    smsMessagingResponse = await salesForceService.SendSMS(messageKey, messaging).ConfigureAwait(false);

                }
                return smsMessagingResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in SendSms :", e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/SendSMSQueue")]
        public async Task SendSMSQueue([FromBody] SMSReminder messageObject)
        {
            try
            {
                salesForceSMSEmail = new SalesForceSMSEmail(Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);
                await salesForceSMSEmail.SendSMS(messageObject).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception("Exception in SendSMSQueue :", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesForceQueueRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/SalesForcePosMethodName")]
        public async Task SalesForcePosMethodName([FromBody]SalesForceQueueRequest salesForceQueueRequest)
        {
            try
            { 
                SalesForceJourney salesForceService = new SalesForceJourney(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:SEG_AccountID_POS"], Configuration["Settings:SalesForce:SEG_ClientID_POS"], Configuration["Settings:SalesForce:SEG_ClientSecret_POS"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                await salesForceService.ProcessSalesForceRequest(salesForceQueueRequest.customer, salesForceQueueRequest.EventDefinitionKey, salesForceQueueRequest.StoreId).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                Logging.Error(String.Format("An error occured while trying to run Salesforce api SalesForcePosMethodName .  Error {0}", ex.Message), ex);
            }
        }



        // 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/WelcomeJourney")]
        public async Task<WelcomeJourneyResponse> WelcomeJourney([FromBody] SEG.ApiService.Models.SalesForce.Data data)
        {
            try
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;


                if (data != null)
                {
                    WelcomeJourneyRequest welcomeJourneyRequest = new WelcomeJourneyRequest();
                    if (!string.IsNullOrEmpty(data.MEMBER_ID) && !string.IsNullOrEmpty(data.Enrollment_Banner))
                    {
                        welcomeJourneyRequest.ContactKey = data.MEMBER_ID;
                        welcomeJourneyRequest.EventDefinitionKey = setKey(data.Enrollment_Banner);
                        welcomeJourneyRequest.data = data;
                        salesForceService = setServiceJourney(data.Enrollment_Banner);



                        welcomeJourneyResponse = await salesForceService.WelcomeJourney(welcomeJourneyRequest).ConfigureAwait(false);
                    }
                }
                return welcomeJourneyResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in WelcomeJourney :", e);
            }


        }

        /// <summary>
        /// Handle Welcome Journey from Ecomm Vendors
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/WelcomeJourneyEcomm")]
        public async Task<WelcomeJourneyResponse> WelcomeJourneyEcomm([FromBody] SEG.ApiService.Models.SalesForce.Data data)
        {
            try
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;


                if (data != null)
                {
                    WelcomeJourneyRequest welcomeJourneyRequest = new WelcomeJourneyRequest();
                    if (!string.IsNullOrEmpty(data.MEMBER_ID) && !string.IsNullOrEmpty(data.Enrollment_Banner))
                    {
                        welcomeJourneyRequest.ContactKey = data.MEMBER_ID;
                        welcomeJourneyRequest.EventDefinitionKey = setKeyEcomm(data.Enrollment_Banner);
                        welcomeJourneyRequest.data = data;
                        salesForceService = setServiceJourney(data.Enrollment_Banner);
                        welcomeJourneyResponse = await salesForceService.WelcomeJourney(welcomeJourneyRequest).ConfigureAwait(false);
                    }
                }
                return welcomeJourneyResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in WelcomeJourneyEcomm :", e);
            }
        }

        // 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/WelcomeJourneyBabyClub")]
        public async Task<WelcomeJourneyResponse> WelcomeJourneyBabyClub([FromBody] SEG.ApiService.Models.SalesForce.Data data)
        {
            try
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;


                if (data != null)
                {
                    WelcomeJourneyRequest welcomeJourneyRequest = new WelcomeJourneyRequest();
                    if (!string.IsNullOrEmpty(data.MEMBER_ID) && !string.IsNullOrEmpty(data.Enrollment_Banner))
                    {
                        welcomeJourneyRequest.ContactKey = data.MEMBER_ID;
                        welcomeJourneyRequest.EventDefinitionKey = setKeyBabyClub(data.Enrollment_Banner);
                        welcomeJourneyRequest.data = data;
                        salesForceService = setServiceJourney(data.Enrollment_Banner);



                        welcomeJourneyResponse = await salesForceService.WelcomeJourney(welcomeJourneyRequest).ConfigureAwait(false);

                    }
                }
                return welcomeJourneyResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in WelcomeJourneyBabyClub :", e);
            }


        }



        private async Task sendEmailInternally(string methodRequest, string subject, string methodResponse)
        {

            try
            {
                //* calling internal API to send email notifications
                InternalEmail internalEmail = new InternalEmail();


                var timeUtc = DateTime.UtcNow;
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                internalEmail.errorResponse = string.Format("response {0} , Time : {1} ", methodResponse, TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone).ToString());
                internalEmail.methodRequest = methodRequest;
                internalEmail.subject = subject;
                internalEmail.toEmail = Configuration["Settings:SalesForce:MailAddressTo"];
                internalEmail.fromEmail = Configuration["Settings:SalesForce:MailAddressFrom"];
                internalEmail.ccEmail = Configuration["Settings:SalesForce:MailAddressCC"];

                await SEG.Shared.ApiUtility.RestfulCallAsync<string>(HttpMethod.Post, internalEmail, "api/Utility/SendInternalEmail", Configuration["Settings:LoyaltyEndPoint"], Headers: createHeader()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                throw new Exception("Exception in sendEmailInternally :", e);
            }
        }


        // 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SalesForce/WelcomeJourneyPetClub")]
        public async Task<WelcomeJourneyResponse> WelcomeJourneyPetClub([FromBody] SEG.ApiService.Models.SalesForce.Data data)
        {
            try
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;


                if (data != null)
                {
                    WelcomeJourneyRequest welcomeJourneyRequest = new WelcomeJourneyRequest();
                    if (!string.IsNullOrEmpty(data.MEMBER_ID) && !string.IsNullOrEmpty(data.Enrollment_Banner))
                    {
                        welcomeJourneyRequest.ContactKey = data.MEMBER_ID;
                        welcomeJourneyRequest.EventDefinitionKey = setKeyPetClub(data.Enrollment_Banner);
                        welcomeJourneyRequest.data = data;
                        salesForceService = setServiceJourney(data.Enrollment_Banner);



                        welcomeJourneyResponse = await salesForceService.WelcomeJourney(welcomeJourneyRequest).ConfigureAwait(false);
                    }
                }
                return welcomeJourneyResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in WelcomeJourneyBabyClub :", e);
            }


        }




        private string setKey(string banner)
        {
            switch (banner.ToLower())
            {
                case HarveysBanner:
                    return Configuration["Settings:SalesForce:HarveysEventDefinitionKey"];
                case WinndixieBanner:
                    return Configuration["Settings:SalesForce:WDEventDefinitionKey"];
                case FrescoBanner:
                    return Configuration["Settings:SalesForce:FrescoEventDefinitionKey"];
                default:
                    return Configuration["Settings:SalesForce:WDEventDefinitionKey"];
            }
        }


        private string setKeyEcomm(string banner)
        {
            switch (banner.ToLower())
            {
                case HarveysBanner:
                    return Configuration["Settings:SalesForce:HarveysEcommEventDefinitionKey"];
                case WinndixieBanner:
                    return Configuration["Settings:SalesForce:WDEcommEventDefinitionKey"];
                case FrescoBanner:
                    return Configuration["Settings:SalesForce:FrescoEcommEventDefinitionKey"];
                default:
                    return Configuration["Settings:SalesForce:WDEcommEventDefinitionKey"];
            }
        }

        private string setKeyBabyClub(string banner)
        {
            switch (banner.ToLower())
            {
                case HarveysBanner:
                    return Configuration["Settings:SalesForce:HarveysBabyClubEventDefinitionKey"];
                case WinndixieBanner:
                    return Configuration["Settings:SalesForce:WDBabyClubEventDefinitionKey"];
                case FrescoBanner:
                    return Configuration["Settings:SalesForce:FrescoBabyClubEventDefinitionKey"];
                default:
                    return Configuration["Settings:SalesForce:WDBabyClubEventDefinitionKey"];
            }
        }


        private string setKeyPetClub(string banner)
        {
            switch (banner.ToLower())
            {
                case HarveysBanner:
                    return Configuration["Settings:SalesForce:HarveysPetClubEventDefinitionKey"];
                case WinndixieBanner:
                    return Configuration["Settings:SalesForce:WDPetClubEventDefinitionKey"];
                case FrescoBanner:
                    return Configuration["Settings:SalesForce:FrescoPetClubEventDefinitionKey"];
                default:
                    return Configuration["Settings:SalesForce:WDPetClubEventDefinitionKey"];
            }
        }

        private SalesForceAPIMService setService(Banner banner)
        {
            switch (banner)
            {
                case Banner.Harveys:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:Harveys_ClientID"], Configuration["Settings:SalesForce:Harveys_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                case Banner.WD:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:WinnDixie_ClientID"], Configuration["Settings:SalesForce:WinnDixie_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                case Banner.Fresco:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:Fresco_ClientID"], Configuration["Settings:SalesForce:Fresco_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                default:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:WinnDixie_ClientID"], Configuration["Settings:SalesForce:WinnDixie_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            }
        }


        private SalesForceAPIMService setServiceJourney(string banner)
        {
            switch (banner.ToLower())
            {
                case HarveysBanner:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:Harveys_ClientID"], Configuration["Settings:SalesForce:Harveys_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                case WinndixieBanner:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:WinnDixie_ClientID"], Configuration["Settings:SalesForce:WinnDixie_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                case FrescoBanner:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:Fresco_ClientID"], Configuration["Settings:SalesForce:Fresco_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
                default:
                    return new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:WinnDixie_ClientID"], Configuration["Settings:SalesForce:WinnDixie_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            }
        }


        /// <returns></returns>
        private Dictionary<string, object> createHeader()
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("Ocp-Apim-Subscription-Key", Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            return headers;
        }

    }
}
