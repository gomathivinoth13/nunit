////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Controllers\CallbackController.cs
//
// summary:	Implements the callback controller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Queueing;
using SEG.ApiService.Models.Twilio;
using SEG.ApiService.Models.Utility;
using SEG.CustomerLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

//using SEG.Twilio;

namespace SEG.LoyaltyServiceWeb.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling callbacks. </summary>
    ///*********************
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class CallbackController : Controller
    {
        ////private static log4net.ILog Logging = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging
        //static object callbackLockObject = new object();    ///< The callback lock object
        //private static Utilities azureQueueUtility; ///< The azure queue utility

        //IConfiguration Configuration;   ///< The configuration

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Constructor. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="configuration">    The configuration. </param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public CallbackController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    lock (callbackLockObject)
        //    {

        //        if (azureQueueUtility == null)
        //            azureQueueUtility = new Utilities(Configuration["Settings:StorageConnectionString"]);
        //    }
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// (An Action that handles HTTP POST requests) twilioes the given request.
        ///// </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="request">  The request. </param>
        /////
        ///// <returns>   An asynchronous result that yields a HttpResponseMessage. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //madhum: code has been modified from post to get to accomidate twilio . and also made chnages to call the loyalty endpoint locally . instead of using seg.tw3ilio . becasuse of .net core and asp.net 4.6 dll miss match . 
        //[HttpGet]
        //[Produces(contentType: "text/plain")]
        //[Route("api/Callback/Twilio")]
        //public async Task<IActionResult> Twilio(TwilMLMessageRequest request)
        //{

        //    request.MessageDate = DateTime.Now;
        //    Response response = new Response
        //    {
        //        Message = new List<string>()
        //    };

        //    try
        //    {


        //        if (!string.IsNullOrWhiteSpace(request.Body))
        //            switch (request.Body.Trim().ToUpper())
        //            {

        //                case "START":
        //                case "YES":
        //                case "DEALS":

        //                    try
        //                    {
        //                        InternalCustomerController internalCustomerUpdate = new InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"]);
        //                        CustomerSearchResponse results;
        //                        results = await CustomerSearch(request).ConfigureAwait(false);

        //                        if (results.IsSuccessful && results.Customers.Any())
        //                        {
        //                            results.Customers.ForEach(async cust =>
        //                            {

        //                                await internalCustomerUpdate.SaveAttribute(new ApiService.Models.Payload.SaveAttributeObject()
        //                                {
        //                                    CrcId = cust.CrcId,
        //                                    attribute = new ApiService.Models.CustomerAttribute()
        //                                    {
        //                                        AttributeId = ApiService.Models.Enum.AttributeType.SMSOptOutStatus,
        //                                        AttributeCode = "Opt-In",
        //                                        LastUpdateDate = DateTime.Now,
        //                                        LastUpdateSource = "SMS - " + request.Body,
        //                                        CrcId = cust.CrcId
        //                                    }
        //                                }).ConfigureAwait(false);
        //                            });
        //                            //response.Message.Add(Configuration["Twilio:StartResponse"]);
        //                            //Get the response from the Loyalty Database
        //                            var res = await GetSMSResponseMessage(request.Body.Trim().ToUpper()).ConfigureAwait(false);
        //                            response.Message.Add(res);

        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        Logging.Error("Error Saving Customer Attributes - SMS", ex);
        //                    }


        //                    break;
        //                case "STOP":

        //                    try
        //                    {


        //                        InternalCustomerController internalCustomerUpdate = new InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"]);
        //                        CustomerSearchResponse results;

        //                        results = await CustomerSearch(request).ConfigureAwait(false);

        //                        if (results.IsSuccessful && results.Customers.Any())
        //                        {
        //                            results.Customers.ForEach(async cust =>
        //                            {

        //                                await internalCustomerUpdate.DeleteAttribute(new ApiService.Models.Payload.DeleteAttributeObject()
        //                                {
        //                                    AttributeId = ApiService.Models.Enum.AttributeType.SMSPreEnrollmentMessageCount,
        //                                    CrcId = cust.CrcId
        //                                }).ConfigureAwait(false);
        //                                await internalCustomerUpdate.SaveAttribute(new ApiService.Models.Payload.SaveAttributeObject()
        //                                {
        //                                    CrcId = cust.CrcId,
        //                                    attribute = new ApiService.Models.CustomerAttribute()
        //                                    {
        //                                        AttributeId = ApiService.Models.Enum.AttributeType.SMSOptOutStatus,
        //                                        AttributeCode = "Opt-Out",
        //                                        LastUpdateDate = DateTime.Now,
        //                                        LastUpdateSource = "SMS - " + request.Body,
        //                                        CrcId = cust.CrcId
        //                                    }
        //                                }).ConfigureAwait(false);

        //                            });
        //                            //response.Message.Add(Configuration["Twilio:StopResponse"]);
        //                            response.Message.Add(await GetSMSResponseMessage("STOP").ConfigureAwait(false));

        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Logging.Error("Error Saving Customer Attributes - SMS", ex);
        //                    }


        //                    break;
        //                case "":
        //                case "HELP":
        //                    //If it's empty string , send back HELP response so that the customer knows what to type 
        //                    /*TODO: If *there* is a intermediary Controller that intercepts the request ( TwilioHack) , 
        //                     * make sure the call comments out the empty string response hardcoded there and let it flow to here. 
        //                    */
        //                    response.Message.Add(await GetSMSResponseMessage("HELP").ConfigureAwait(false));

        //                    break;
        //                default:
        //                    // response.Message.Add(Configuration["Twilio:HelpResponse"]);

        //                    //This will make sure that if marketing add's more templates, in future,
        //                    // it will dynamically get from database without having to hard code here
        //                    response.Message.Add(await GetSMSResponseMessage(request.Body.Trim().ToUpper()).ConfigureAwait(false));
        //                    break;
        //            }
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Error(e);
        //    }

        //    //var twilioResponseXml = GenerateTwilioResponseXml(response);

        //    //HttpResponseMessage responseMessage = new HttpResponseMessage()
        //    //{
        //    //    Content = new StringContent(response.Message.FirstOrDefault(), Encoding.UTF8, "text/xml")
        //    //};
        //    var cr = new ContentResult() { Content = response.Message.FirstOrDefault(), ContentType = "text/plain", StatusCode = 200 };
        //    return cr;

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Customer search. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="request">  The request. </param>
        ///
        /// <returns>   An asynchronous result that yields a CustomerSearchResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //private async Task<CustomerSearchResponse> CustomerSearch(TwilMLMessageRequest request)
        //{
        //    var internalCustomerUpdate = new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"]);
        //    CustomerSearchResponse results = await internalCustomerUpdate.CustomerSearch(new ApiService.Models.CustomerSearchRequest() { MobilePhoneNumber = request.From }).ConfigureAwait(false);
        //    if (results == null || (results.IsSuccessful && !results.Customers.Any()))
        //    {
        //        string newNumber = request.From.Replace("+1", "");
        //        results = await internalCustomerUpdate.CustomerSearch(new ApiService.Models.CustomerSearchRequest() { MobilePhoneNumber = newNumber }).ConfigureAwait(false);
        //    }

        //    if (results == null || (results.IsSuccessful && !results.Customers.Any()))
        //    {
        //        string newNumber = request.From.Replace("+1", "1");
        //        results = await internalCustomerUpdate.CustomerSearch(new ApiService.Models.CustomerSearchRequest() { MobilePhoneNumber = newNumber }).ConfigureAwait(false);
        //    }
        //    return results;
        //}


        //private async Task<string> GetSMSResponseMessage(string customerSMSMessageBody)
        //{
        //    HttpResponseMessage result;
        //    string LoyaltySVCBaseUrl = Configuration["Settings:LoyaltyEndPoint"];
        //    try
        //    {

        //        Dictionary<string, object> queryParams = new Dictionary<string, object>();
        //        queryParams.Add("customerSMSMessageBody", customerSMSMessageBody);

        //        var response = await SEG.Shared.ApiUtility.RestfulCallAsync<string>(HttpMethod.Get, null, "/api/SMSResponses/GetSMSResponseMessage", LoyaltySVCBaseUrl, QueryParams: queryParams).ConfigureAwait(false);

        //        return response.Result;



        //    }
        //    catch (Exception e)
        //    {
        //        ////Logging.Error(
        //        //    "LoyaltyApis:  Error Processing SMS Responses from Loyalty Database which is accessed through the LoyaltyService WebAPI - api/SMSResponses/GetSMSResponseMessageForCustomerInput ",
        //        //    e);
        //        return "Failure. Please try again later.";
        //    }
        //    finally
        //    {
        //        result = null;
        //    }

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   Generates a twilio response XML. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="response"> The response. </param>
        /////
        ///// <returns>   The twilio response XML. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //private string GenerateTwilioResponseXml(Response response)
        //{
        //    try
        //    {

        //        XElement ele = new XElement("Response");

        //        if (response.Message != null)
        //        {
        //            response.Message.ForEach(m => ele.Add(new XElement("Message", m)));
        //        }

        //        return ele.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in GenerateTwilioResponseXml :", e);
        //    }
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   (An Action that handles HTTP POST requests) delivery status. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        /////
        ///// <param name="status">   The status. </param>
        /////
        ///// <returns>   An asynchronous result that yields a HttpResponseMessage. </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Produces(contentType: "text/xml")]
        //[Route("api/Callback/DeliveryStatus")]
        //public async Task<IActionResult> DeliveryStatus([FromBody]DeliveryStatus status)
        //{
        //    try
        //    {
        //        QueueTask queueTask = new QueueTask
        //        {
        //            MethodName = QueueMethodNameType.SMSCallBack,
        //            QueueName = QueueNameType.SMSCallback,
        //            QueueObject = status,
        //            ContinueOnError = false,
        //            IsQueueRequest = true
        //        };
        //        await azureQueueUtility.AddQueueTaskAsync(queueTask);

        //        string sSyncData = "<?xml version=\"1.0\"?><Response/>";
        //        //HttpResponseMessage responseMessage = new HttpResponseMessage
        //        //{
        //        //    Content = new StringContent(sSyncData, System.Text.Encoding.UTF8, "application/xml")
        //        //};

        //        var cr = new ContentResult() { Content = sSyncData, ContentType = "text/xml", StatusCode = 200 };
        //        return cr;
        //    }
        //    catch (Exception e)
        //    {

        //        return BadRequest(e.ToString());
        //    }

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>   A response. </summary>
        /////
        ///// <remarks>   Mcdand, 2/20/2018. </remarks>
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //[DataContract(Namespace = "")]
        //[XmlRoot(ElementName = "Response")]
        //public class Response
        //{
        //    ////////////////////////////////////////////////////////////////////////////////////////////////////
        //    /// <summary>   Gets or sets the message. </summary>
        //    ///
        //    /// <value> The message. </value>
        //    ////////////////////////////////////////////////////////////////////////////////////////////////////

        //    [DataMember]
        //    [XmlElement(ElementName = "Message")]
        //    public List<string> Message { get; set; }
        //}



    }
}