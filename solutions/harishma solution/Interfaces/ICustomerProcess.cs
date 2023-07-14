using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.PrimaryStore;
using SEG.ApiService.Models.Request;
using SEG.ApiService.Models.SendGrid;
using SEG.LoyaltyDatabase.Models;
using SEG.LoyaltyService.Models.Results;

namespace SEG.LoyaltyService.Process.Core.Interfaces
{
    public interface ICustomerProcess
    {
        Task<PrimaryStoreResponse> GetPrimaryStoreAsync(string memberId, int chainId);
        Task<PiiRequest> DeletePiiAsync(string membershipId);
        Task<CustomerPreferenceUpsertResponse> SaveCustomerEReceiptPreferenceAsync(CustomerPreference customerPreference);
        Task<CustomerPreferenceRetrieveResponse> GetCustomerEReceiptPreferenceAsync(string memberId = null, string mobilePhoneNumber = null, string emailAddess = null);

        /// <summary>   List children asynchronous. </summary>
        ///

        ///
        /// <param name="memberId">    Identifier for the memberId. </param>
        ///
        /// <returns>   An asynchronous result that yields the list children. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<CustomerChild>> ListChildrenAsync(string memberId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="crcid"></param>
        /// <param name="aliasNumber"></param>
        /// <returns></returns>
        Task<FRNStatusValidateResponse> FRNStatusValidate(string memberid = null, string crcid = null, string aliasNumber = null);

        /// <summary>   Validates the pin described by credentialRequest. </summary>
        ///
        ///
        ///
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have
        ///                                             unsupported or illegal values. </exception>
        /// <exception cref="HttpResponseException">    Thrown when a HTTP Response error condition
        ///                                             occurs. </exception>
        ///
        /// <param name="credentialRequest">    The credential request. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool?> ValidatePIN(CredentialRequest credentialRequest);

        /// <summary>   Validates the password described by credentialRequest. </summary>
        ///
        /// <remarks>   madhum, 3/19/2018. </remarks>
        ///
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have
        ///                                             unsupported or illegal values. </exception>
        /// <exception cref="HttpResponseException">    Thrown when a HTTP Response error condition
        ///                                             occurs. </exception>
        ///
        /// <param name="credentialRequest">    The credential request. </param>
        ///
        /// <returns>   An asynchronous result that returns CredRetrieveResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<CredRetrieveResult> ValidatePassword(CredentialRequest credentialRequest);

        /// <summary>   Gets the customer preferences. </summary>
        ///
        /// <remarks>   Mark Robinson, 09/01/2020. </remarks>
        ///
        /// <param name="memberId">   The member id. </param>
        /// 
        /// <param name="chainId">    The chainId. </param>
        ///
        /// <returns>   CustomerPreference. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<CustomerPreferenceRetrieveResponse> GetCustomerPreference(string memberId, string chainId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialRequest"></param>
        /// <returns></returns>
        Task<CredRetrieveResultV2> ValidatePasswordV2(CredentialRequest credentialRequest);

        /// <summary>   Saves a customer asynchronous. </summary>
        ///

        ///
        /// <param name="customer">             The customer. </param>
        /// <param name="returnHydratedObject"> True to return hydrated object. </param>
        /// <param name="savepin"> True to save pin  </param>
        /// <param name="savepassword"> True to save password </param>        ///
        /// <param name="saveCustomerInAzure"> True if comming from saveCustomerInAzure method web and mobile </param>        ///
        /// <returns>   An asynchronous result that yields the save customer. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<CustomerResponse> SaveCustomerAsync(CustomerV2 customer, bool savepin = false, bool savepassword = false, bool returnHydratedObject = false, bool saveCustomerInAzure = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        ////******performace issues ************////////
        Task<CustomerResponse> Save(CustomerV2 customer);

        /// <summary>   Sets validated phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="request">  The request. </param>
        ///
        /// <returns>   An asynchronous result that yields a SetValidatedPhoneNumberResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<SetValidatedPhoneNumberResponse> SetValidatedPhoneNumber(SetValidatedPhoneNumberRequest request);

        /// <summary>   Gets the customer preferences. </summary>
        ///
        /// <remarks>   Mark Robinson, 09/01/2020. </remarks>
        ///
        /// <param name="customerPreference">   The customer preferences </param>
        /// 
        /// <returns>   CustomerPreference. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<CustomerPreferenceUpsertResponse> SaveCustomerPreference(CustomerPreference customerPreference);

        Task<CustomerSearchResponse> CustomerSearchAsync(CustomerSearchRequest customerSearchRequest, bool viewPin = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="viewPin"></param>
        /// <returns></returns>
        Task<CustomerSearchResponse> GetCustomerRecord(CustomerSearchRequest customerSearchRequest, bool viewPin = false);

        /// <summary>   Customer search for CRC asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="customerSearchForCRCRequest">  The customer search for CRC request. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer search for CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<CustomerSearchForCRCResponse> CustomerSearchForCRCAsync(CustomerSearchForCRCRequest customerSearchForCRCRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CustomerSearchTruncatedResponse> CustomerSearchTruncatedAsync(CustomerSearchTruncatedRequest request);

        /// <summary>   Gets the SMS Templates . </summary>
        ///
        /// <remarks>   Sam nanduri, 2/28/2018. </remarks>
        ///
        /// <param name="templateType">  The TemplateType. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<SMSTemplate>> GetSmsTemplates(string templateType);

        ///
        Task<SMSHistory> ProcessSMSSendAuthorizationCode(CancellationToken token,
            CustomerV2 customer, string authCode,
            string bannerUrl,
            string customerUrl,
            string expiryMessage,
            string template,
            Banner banner,
            string templateType = "AuthorizationCode");

        /// <summary>   Process the send email to customer. </summary>
        ///
        /// <remarks>  Sam Nanduri, 2/26/2018. </remarks>
        ///
        /// <returns>   An result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<bool> ProcessSendEmailToCustomerAsync(MessageObject msgObj, CancellationToken token);
    }
}