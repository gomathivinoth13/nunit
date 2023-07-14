using log4net;
using Newtonsoft.Json;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Reminder;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.SendGrid;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SalesForceLibrary.SendSMSEmail
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceSMSEmail
    {
        #region Static Variables
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string requestType = "SYNC";

        #endregion Static Variables  

        /// <summary>
        /// 
        /// </summary>
        public string WebApiEndPoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiEndPoint"></param>
        public SalesForceSMSEmail(string webApiEndPoint)
        {
            WebApiEndPoint = webApiEndPoint;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
        public async Task SendEmail(MessageObject messageObject)
        {
            Messaging messaging = new Messaging();
            From fromAddress = new From();
            To toAddress = new To();
            ContactAttributes contactAttributes = new ContactAttributes();
            SubscriberAttributes subscriberAttributes = new SubscriberAttributes();
            Options options = new Options();
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                if (messageObject != null)
                {
                    fromAddress.Address = messageObject.ReplyAddress;
                    fromAddress.Name = messageObject.Subject;

                    messaging.From = fromAddress;


                    toAddress.Address = messageObject.ToAddress;
                    if (messageObject.TemplateFields.ContainsKey("memberid") && messageObject.TemplateFields["memberid"] != null)
                    {
                        toAddress.SubscriberKey = messageObject.TemplateFields["memberid"];
                    }
                    if (messageObject.TemplateFields.ContainsKey("authorizationcode") && messageObject.TemplateFields["authorizationcode"] != null)
                    {
                        subscriberAttributes.authorizationcode = messageObject.TemplateFields["authorizationcode"];
                    }

                    //bannerurl
                    if (messageObject.TemplateFields.ContainsKey("link") && messageObject.TemplateFields["link"] != null)
                    {
                        subscriberAttributes.link = messageObject.TemplateFields["link"];
                    }

                    contactAttributes.SubscriberAttributes = subscriberAttributes;
                    toAddress.ContactAttributes = contactAttributes;

                    options.RequestType = requestType;

                    messaging.To = toAddress;
                    messaging.Options = options;


                    queryParams.Add("messageKey", messageObject.TemplateID);
                    queryParams.Add("banner", Convert.ToInt32(messageObject.Banner));

                    var response = await SEG.Shared.ApiUtility.RestfulPostAsync<MessageResponse>(messaging, "Send", WebApiEndPoint, QueryParams: queryParams).ConfigureAwait(false);

                }

            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An exception occurred while trying to SendEmail-SalesForce. Error {0}", ex.Message), ex);
                throw;
            }

        }


        /// <summary>
        /// send sms 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
        public async Task SendSMS(SMSReminder messageObject)
        {
            SmsMessaging smsMessaging = new SmsMessaging();
            List<string> mobile = new List<string>();
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                if (messageObject != null && !string.IsNullOrEmpty(messageObject.MobilePhoneNumber))
                {
                    //format phone number 
                    string formattedNumber = Regex.Replace(messageObject.MobilePhoneNumber ?? string.Empty, "[^0-9]", string.Empty);
                    if (messageObject.MobilePhoneNumber.Length >= 10)
                        formattedNumber = messageObject.MobilePhoneNumber.Substring(messageObject.MobilePhoneNumber.Length - 10);



                    //sfmc always need 1 before phone number 
                    mobile.Add("1" + formattedNumber);
                    smsMessaging.mobileNumbers = mobile;
                    if (messageObject.TemplateType == TemplateType.ResetPasswordAndCode.ToString())
                    {
                        smsMessaging.keyword = SFMCKeywordType.RESETPW.ToString();
                    }
                    else
                    {
                        smsMessaging.keyword = SFMCKeywordType.AUTHORIZATIONCODE.ToString();
                    }
                    smsMessaging.Subscribe = true;
                    smsMessaging.Resubscribe = true;
                    smsMessaging.Override = true;
                    smsMessaging.messageText = messageObject.Message;

                    var response = await SEG.Shared.ApiUtility.RestfulPostAsync<SmsMessagingResponse>(smsMessaging, "SendSMS", WebApiEndPoint).ConfigureAwait(false);

                }

            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An exception occurred while trying to SendEmail-SalesForce. Error {0}", ex.Message), ex);
                throw;
            }


        }
    }
}
