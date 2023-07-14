////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	customerprocess.cs
//
// summary:	Implements the customerprocess class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SalesForceLibrary.Models;
using SEG.ApiService.Models;
using SEG.ApiService.Models.AppSettings;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.PrimaryStore;
using SEG.ApiService.Models.Queueing;
using SEG.ApiService.Models.Reminder;
using SEG.ApiService.Models.Request;
using SEG.ApiService.Models.SendGrid;
using SEG.CustomerWebService.Core;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyDatabase.Models;
using SEG.LoyaltyService.Models.Results;
using SEG.LoyaltyService.Process.Core.Interfaces;
using SEG.Shared;

namespace SEG.LoyaltyService.Process.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A customer process. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class CustomerProcess : ICustomerProcess
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the customer service. </summary>
        ///
        /// <value> The customer service. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        readonly bool OMNI_ENABLED = true;

        #region Constructors
        private readonly IAdHocSMSJobService _adHocSmsJobService;
        private readonly ISMSHistoryRecordsService _smsHistoryRecordsService;
        private readonly ISMSTemplateService _smsTemplateService;
        private readonly IAdHocSMSJobItemService _adHocSmsJobItemService;
        private readonly ICustomerService _customerService;
        private readonly ICardRangeService _cardRangeService;
        private readonly AppSettingsOptions _settings;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public CustomerProcess(IAdHocSMSJobService adHocSmsJobService, ISMSHistoryRecordsService smsHistoryRecordsService,
            ISMSTemplateService smsTemplateService, IAdHocSMSJobItemService adHocSmsJobItemService,
            IOptions<AppSettingsOptions> settings, ICustomerService customerService, ICardRangeService cardRangeService, IAzureQueueService azureQueueService)
        {
            _settings = settings.Value;
            _adHocSmsJobService = adHocSmsJobService;
            _smsHistoryRecordsService = smsHistoryRecordsService;
            _smsTemplateService = smsTemplateService;
            _adHocSmsJobItemService = adHocSmsJobItemService;
            _customerService = customerService;
            _cardRangeService = cardRangeService;
        }

        string Salt => _settings.EncryptedSalt.DecryptStringAES2("LoyaltySalt");
       
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   List children asynchronous. </summary>
        ///

        ///
        /// <param name="memberId">    Identifier for the memberId. </param>
        ///
        /// <returns>   An asynchronous result that yields the list children. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<List<CustomerChild>> ListChildrenAsync(string memberId)
        {
            List<CustomerChild> children = new List<CustomerChild>();
            CustomerV2 customer = new CustomerV2();
            customer = await _customerService.GetCustomerAsync(memberId);

            if (customer != null && customer.CustomerChild != null && customer.CustomerChild.Count > 0)
            {
                children = customer.CustomerChild;
            }

            return children;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="crcid"></param>
        /// <param name="aliasNumber"></param>
        /// <returns></returns>
        public async Task<FRNStatusValidateResponse> FRNStatusValidate(string memberid = null, string crcid = null, string aliasNumber = null)
        {
            List<CustomerChild> children = new List<CustomerChild>();
            Customer customer = new Customer();

            FRNStatusValidateResponse FRNResponse = await _customerService.GetFRNStatus(memberid, crcid, aliasNumber);

            return FRNResponse;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<bool?> ValidatePIN(CredentialRequest credentialRequest)
        {
            CustomerSearchRequest request = new CustomerSearchRequest();
            CustomerSearchResponse response = null;

            switch (credentialRequest.AliasType)
            {
                case AliasType.PlentiCardNumber:
                    throw new ArgumentException("Plenti Card Is Not a Valid Alias");
                case AliasType.SEGCardNumber:
                    request.OmniId = credentialRequest.Alias;
                    response = await _customerService.GetCustRecordAsync(request);
                    break;
                case AliasType.PhoneNumber:
                    request.MobilePhoneNumber = credentialRequest.Alias;
                    response = await _customerService.GetCustRecordAsync(request);
                    break;
                case AliasType.EmailAddress:
                    request.EmailAddress = credentialRequest.Alias;
                    response = await _customerService.GetCustRecordAsync(request);

                    break;
                default:
                    break;
            }

            if (response == null || response.IsSuccessful == false || response.Customers == null || response.Customers.Count <= 0) return null;


            if (response.IsSuccessful == true && response.Customers != null && response.Customers.Count >= 0)
            {
                var customer = response.Customers.FirstOrDefault();
                if (customer != null && !string.IsNullOrEmpty(customer.AccountPin))
                {
                    if (customer.AccountPin.DecryptStringAES(customer.MemberId, Salt).Equals(credentialRequest.PinPassword))
                        return true;
                }
            }

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<CredRetrieveResult> ValidatePassword(CredentialRequest credentialRequest)
        {
            CustomerSearchRequest request = new CustomerSearchRequest();
            CustomerSearchResponse response = null;

            try
            {
                CustomerResponse customerResponse = new CustomerResponse();
                CredRetrieveResult credRetrieveResult = new CredRetrieveResult();
                switch (credentialRequest.AliasType)
                {
                    case AliasType.PlentiCardNumber:
                        throw new ArgumentException("Plenti Card Is Not a Valid Alias");

                    case AliasType.SEGCardNumber:
                        request.OmniId = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);
                        break;
                    case AliasType.PhoneNumber:
                        request.MobilePhoneNumber = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);
                        break;
                    case AliasType.EmailAddress:
                        request.EmailAddress = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);

                        break;
                    default:
                        break;
                }

                if (response != null && response.IsSuccessful && response.Customers != null && response.Customers.Count > 0)
                {

                    CustomerV2 customer = response.Customers.FirstOrDefault();
                    MapMethod(customer, credRetrieveResult);
                    if (customer != null)
                    {

                        //checking for zip to see if it's soft login
                        if (Regex.IsMatch(credentialRequest.PinPassword, "^\\d{5}$"))
                        {
                            credRetrieveResult.Status = "Success";
                            string chainId = new Utility().SetChainId(string.IsNullOrEmpty(credentialRequest.AppCode) ? "00" : credentialRequest.AppCode);
                            //if the record for other chainid doesn't exist in the database then create it
                            await CheckAndSaveCustomerOnLogin(response.Customers.FirstOrDefault(), chainId);
                            return credRetrieveResult;
                        }

                        if (string.IsNullOrEmpty(customer.AccountPassword))
                        {
                            credRetrieveResult.Status = "passwordnotset";
                            credRetrieveResult.Error_Description = "Password not set for the user";
                        }
                        else
                        {
                            var password = $"{credentialRequest.PinPassword}{Salt}".GenerateHashString(HashingExtensions.HashType.SHA256);

                            if (customer.AccountPassword.Equals(password))
                            {
                                credRetrieveResult.Status = "Success";
                                string chainId = new Utility().SetChainId(string.IsNullOrEmpty(credentialRequest.AppCode) ? "00" : credentialRequest.AppCode);
                                //if the record for other chainid doesn't exist in the database then create it
                                await CheckAndSaveCustomerOnLogin(customer, chainId);
                            }
                            else
                            {
                                credRetrieveResult.Status = "unauthenticated";
                                credRetrieveResult.Error_Description = "Athentication failed for the user";
                            }
                        }
                    }
                }

                return credRetrieveResult;



            }
            catch (Exception)
            {
                return new CredRetrieveResult()
                {
                    Error_Description = "User not found",
                    Status = "Error"
                };
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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
        public async Task<CustomerPreferenceRetrieveResponse> GetCustomerPreference(string memberId, string chainId)
        {
            return await _customerService.GetCustomerPreference(memberId, chainId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialRequest"></param>
        /// <returns></returns>
        public async Task<CredRetrieveResultV2> ValidatePasswordV2(CredentialRequest credentialRequest)
        {
            CustomerSearchRequest request = new CustomerSearchRequest();
            CustomerSearchResponse response = null;
            try
            {
                CustomerResponse customerResponse = new CustomerResponse();
                CredRetrieveResultV2 credRetrieveResult = new CredRetrieveResultV2();
                switch (credentialRequest.AliasType)
                {
                    case AliasType.PlentiCardNumber:
                        throw new ArgumentException("Plenti Card Is Not a Valid Alias");

                    case AliasType.SEGCardNumber:
                        request.OmniId = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);
                        break;
                    case AliasType.PhoneNumber:
                        request.MobilePhoneNumber = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);
                        break;
                    case AliasType.EmailAddress:
                        request.EmailAddress = credentialRequest.Alias;
                        response = await _customerService.CustomerSearchAsync(request);
                        break;
                    default:
                        break;
                }

                if (response != null && response.IsSuccessful && response.Customers != null && response.Customers.Count > 0)
                {

                    CustomerV2 customer = response.Customers.FirstOrDefault();
                    if (customer != null)
                    {

                        //set address and child data to null as they are not required by website/mobile app  in validate password response  . 
                        if (customer.CustomerChild != null)
                            customer.CustomerChild = null;
                        if (customer.CustomerAddress != null)
                            customer.CustomerAddress = null;


                        credRetrieveResult.customer = customer;

                        if (string.IsNullOrEmpty(customer.AccountPassword))
                        {
                            credRetrieveResult.status = "passwordnotset";
                            credRetrieveResult.error_Description = "Password not set for the user";
                        }
                        else
                        {
                            var password = $"{credentialRequest.PinPassword}{Salt}".GenerateHashString(HashingExtensions.HashType.SHA256);

                            if (customer.AccountPassword.Equals(password))
                            {
                                credRetrieveResult.status = "Success";
                                string chainId = new Utility().SetChainId(string.IsNullOrEmpty(credentialRequest.AppCode) ? "00" : credentialRequest.AppCode);
                                //if the record for other chainid doesn't exist in the database then create it
                                var custResponse = await CheckAndSaveCustomerOnLoginV2(customer, chainId);
                                if (custResponse != null && custResponse.IsSuccessful && custResponse.Customer != null)
                                {
                                    credRetrieveResult.customer = custResponse.Customer;
                                }
                                return credRetrieveResult;
                            }
                            else
                            {
                                credRetrieveResult.status = "unauthenticated";
                                credRetrieveResult.error_Description = "Athentication failed for the user";
                            }
                        }
                    }
                }

                return credRetrieveResult;
            }
            catch (Exception)
            {
                return new CredRetrieveResultV2()
                {
                    error_Description = "User not found",
                    status = "Error"
                };
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Ff the record for other chainid doesn't exist in the database then create it. </summary>
        ///
        /// <remarks>   madhum, 3/13/2020. </remarks>
        ///
        /// <param name="customer">    The credential request. </param>
        /// 
        /// <param name="chainId">    The chainId. </param>
        ///
        /// <returns>   Void. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion Read

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fix winn dixie CRC number. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="customer"> The customer. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void FixWinnDixieCrcNumber(CustomerV2 customer)
        {
            if (customer.CustomerCRC != null && customer.CustomerCRC.Count() > 0)
            {
                foreach (var crc in customer.CustomerCRC)
                {
                    if (!string.IsNullOrEmpty(crc.ChainId) && !string.IsNullOrEmpty(crc.CrcId))
                    {

                        decimal.TryParse(crc.CrcId, out decimal crcId);

                        if (crc.ChainId.Trim() == Banner.WD.GetAttribute<ChainIdAttribute>().Value && crcId > 980000000000000)
                        {
                            crcId = crcId - 980000000000000;

                            //Handle 9800 series CRC's

                            crc.CrcId = crcId.ToString().Split('.')[0];
                        }

                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<CustomerResponse> SaveCustomerAsync(CustomerV2 customer, bool savepin = false, bool savepassword = false, bool returnHydratedObject = false, bool saveCustomerInAzure = false)
        {
            CustomerResponse customerResponse = new CustomerResponse();
            CustomerSearchRequest request = new CustomerSearchRequest();
            string duplicateError = string.Empty;
            try
            {


                FixWinnDixieCrcNumber(customer);

                //setpin 
                if (savepin)
                {
                    if (!string.IsNullOrEmpty(customer.AccountPin) && !string.IsNullOrEmpty(customer.MemberId))
                    {
                        string PIN = customer.AccountPin;
                        if (customer.AccountPin.Length < 24)
                        {
                            PIN = customer.AccountPin.EncryptStringAES(customer.MemberId, Salt);
                            customer.AccountPin = PIN;
                            customer.InvalidPinAttempts = 0;
                        }
                    }
                }
                else
                {
                    customer.AccountPin = null;
                }

                //set password 
                if (savepassword)
                {
                    if (!string.IsNullOrEmpty(customer.AccountPassword) && !string.IsNullOrEmpty(customer.MemberId))
                    {
                        string password = customer.AccountPassword;

                        if (customer.AccountPassword.Length < 24)
                        {
                            password = $"{password}{Salt}".GenerateHashString(HashingExtensions.HashType.SHA256);
                            customer.AccountPassword = password;
                            customer.InvalidPasswordAttempts = 0;
                        }
                    }
                }
                else
                {
                    customer.AccountPassword = null;
                }

                if (!String.IsNullOrEmpty(customer.MemberId))
                {
                    request.MemberId = customer.MemberId;
                    CustomerAliasSearchResponse customerAlias = await _customerService.CustAliasSearchAsync(request);


                    if (customerAlias == null || customerAlias.Alias == null || !customerAlias.Alias.Any(x => x.AliasType == 2 && x.AliasStatus == 0))
                    {
                        customer = await AssignSEGCard(customer, customer.MemberId);
                    }
                }

                if (customer.CustomerAddress != null && customer.CustomerAddress.Count > 0)
                {
                    foreach (CustomerAddress custAdd in customer.CustomerAddress)
                    {
                        custAdd.AddressType = 1;
                    }
                }

                bool validRequest = _customerService.VerifyCustomer(customer, out List<string> errorMessages);
                if (customer.CustomerCRC != null)
                {
                    //check for Supervisor Cards
                    foreach (var crc in customer.CustomerCRC)
                    {
                        if (crc.CrcId != null && crc.ChainId != null)
                        {
                            var crcIdDecimal = decimal.Parse(crc.CrcId);
                            var supervisorCards = await _cardRangeService.GetAsync(c => c.SupervisorCardRangeId == crcIdDecimal);
                            if (supervisorCards.Any())
                            {
                                errorMessages.Add("CRC is a SuperVisorCard.   A new CrcId must be generated, before saving the customer.");
                                validRequest = false;
                            }
                        }
                    }
                }

                if (!validRequest)
                {
                    customerResponse = new CustomerResponse() { IsSuccessful = false, ErrorMessages = errorMessages };
                }
                else
                {

                    if (customer.CustomerAlias != null)
                    {
                        customer.CustomerAlias.RemoveAll(a => string.IsNullOrWhiteSpace(a.AliasNumber));
                    }

                    customer.LastUpdateSource = customer.LastUpdateSource ?? "Save Customer";

                    customerResponse = await _customerService.SaveCustomerAsync(customer);


                    if (customerResponse != null && customerResponse.IsSuccessful)
                    {
                        customerResponse.Customer = customer;

                        var wallet = new CustomerWallet();
                        wallet.WalletId = customerResponse.WalletId;
                        wallet.MemberId = customer.MemberId;
                        //wallet.ConsumerId = "";
                        customerResponse.Customer.CustomerWallet = new List<CustomerWallet>() { wallet };

                        if (!string.IsNullOrEmpty(customer.MemberId))
                        {
                            await InsertSalesForceQueue(new SalesForceQueueRequest
                            {
                                customer = customer,
                                appCode = "",
                                transactionID = ""
                            });
                        }
                    }
                }
                return customerResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        private async Task InsertSalesForceQueue(SalesForceQueueRequest salesForceQueueRequest)
        {
            try
            {
                if (salesForceQueueRequest.customer == null) return;
                await AddToAzureQueueAsync(salesForceQueueRequest, QueueMethodNameType.SalesForceMethodName, QueueNameType.SalesForce);
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        ////******performace issues ************////////
        public async Task<CustomerResponse> Save(CustomerV2 customer)
        {
            customer.LastUpdateSource = customer.LastUpdateSource ?? "Save Customer";
            var customerResponse = await _customerService.SaveCustomerAsync(customer);

            return customerResponse;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets validated phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="request">  The request. </param>
        ///
        /// <returns>   An asynchronous result that yields a SetValidatedPhoneNumberResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<SetValidatedPhoneNumberResponse> SetValidatedPhoneNumber(SetValidatedPhoneNumberRequest request)
        {
            return await _customerService.SetValidatedPhoneNumber(request);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the customer preferences. </summary>
        ///
        /// <remarks>   Mark Robinson, 09/01/2020. </remarks>
        ///
        /// <param name="customerPreference">   The customer preferences </param>
        /// 
        /// <returns>   CustomerPreference. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<CustomerPreferenceUpsertResponse> SaveCustomerPreference(CustomerPreference customerPreference)
        {
            return await _customerService.SaveCustomerPreference(customerPreference);
        }

        public async Task<CustomerSearchResponse> CustomerSearchAsync(CustomerSearchRequest customerSearchRequest, bool viewPin = false)
        {
            try
            {
                CustomerSearchResponse customerSearchResponse = null;
                customerSearchResponse = await _customerService.CustomerSearchAsync(customerSearchRequest);

                if (viewPin)
                {
                    if (customerSearchResponse != null && customerSearchResponse.IsSuccessful == true && customerSearchResponse.Customers != null && customerSearchResponse.Customers.Count == 1)
                    {
                        var customer = customerSearchResponse.Customers.FirstOrDefault();
                        if (customer != null && !string.IsNullOrEmpty(customer.AccountPin))
                        {
                            var pin = customer.AccountPin.DecryptStringAES(customer.MemberId, Salt);

                            customer.AccountPin = pin;
                        }
                    }

                }

                return customerSearchResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="viewPin"></param>
        /// <returns></returns>
        public async Task<CustomerSearchResponse> GetCustomerRecord(CustomerSearchRequest customerSearchRequest, bool viewPin = false)
        {
            CustomerSearchResponse customerSearchResponse = null;
            try
            {
                if (customerSearchRequest != null)
                {
                    customerSearchResponse = await _customerService.GetCustRecordAsync(customerSearchRequest);

                    if (viewPin)
                    {
                        if (customerSearchResponse != null && customerSearchResponse.IsSuccessful == true && customerSearchResponse.Customers != null && customerSearchResponse.Customers.Count == 1)
                        {
                            var customer = customerSearchResponse.Customers.FirstOrDefault();
                            if (customer != null && !string.IsNullOrEmpty(customer.AccountPin))
                            {
                                var pin = customer.AccountPin.DecryptStringAES(customer.MemberId, Salt);

                                customer.AccountPin = pin;
                            }
                        }
                    }
                }
                return customerSearchResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Customer search for CRC asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="customerSearchForCRCRequest">  The customer search for CRC request. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer search for CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<CustomerSearchForCRCResponse> CustomerSearchForCRCAsync(CustomerSearchForCRCRequest customerSearchForCRCRequest)
        {
            CustomerSearchRequest customerSearchRequest = new CustomerSearchRequest();
            CustomerSearchResponse customerSearchResponse;
            CustomerSearchForCRCResponse customerSearchForCRCResponse = new CustomerSearchForCRCResponse();
            AliasRequest aliasRequest = new AliasRequest();
            string inComingChainId = string.Empty;

            if (customerSearchForCRCRequest != null)
            {
                if (customerSearchForCRCRequest.Banner == null)
                {
                    customerSearchForCRCResponse.ErrorMessage = "Banner is required";
                    return customerSearchForCRCResponse;
                }
                switch (customerSearchForCRCRequest.Banner)
                {
                    case Banner.WD:
                        inComingChainId = "1";
                        break;
                    case Banner.Bilo:
                        inComingChainId = "2";
                        break;
                    case Banner.Harveys:
                        inComingChainId = "3";
                        break;
                }

                if (string.IsNullOrEmpty(customerSearchForCRCRequest.PlentiId) && string.IsNullOrEmpty(customerSearchForCRCRequest.GNGCardNumber) && string.IsNullOrEmpty(customerSearchForCRCRequest.PhoneNumber))
                {
                    customerSearchForCRCResponse.ErrorMessage = "GNGCardNumber or PlentiId or PhoneNumber is required";
                    return customerSearchForCRCResponse;
                }

                if (!string.IsNullOrEmpty(customerSearchForCRCRequest.GNGCardNumber))
                    customerSearchRequest.OmniId = customerSearchForCRCRequest.GNGCardNumber;
                else if (!string.IsNullOrEmpty(customerSearchForCRCRequest.PhoneNumber))
                    customerSearchRequest.MobilePhoneNumber = customerSearchForCRCRequest.PhoneNumber;
                else if (!string.IsNullOrEmpty(customerSearchForCRCRequest.PlentiId))
                    customerSearchRequest.OmniId = customerSearchForCRCRequest.PlentiId;

                customerSearchResponse = await _customerService.CustomerSearchAsync(customerSearchRequest);

                if (customerSearchResponse != null && customerSearchResponse.Customers?.Count > 0)
                {
                    var customer = customerSearchResponse.Customers.FirstOrDefault();
                    CustomerCRC selectedCust = null;
                    if (customer != null && customer.CustomerCRC != null && customer.CustomerCRC.Count > 0)
                    {
                        if (inComingChainId == Banner.WD.GetAttribute<ChainIdAttribute>().Value)
                            selectedCust = customer.CustomerCRC.Where(a => a.ChainId.Trim() == inComingChainId).OrderBy(a => a.LastUpdateDate).Reverse().FirstOrDefault();
                        else
                            selectedCust = customer.CustomerCRC.Where(a => a.ChainId.Trim() != Banner.WD.GetAttribute<ChainIdAttribute>().Value).OrderBy(a => a.LastUpdateDate).Reverse().FirstOrDefault();

                        if (selectedCust != null)
                        {
                            //check for G&G card else generate one and assign 
                            if (customer.CustomerAlias == null || !customer.CustomerAlias.Any(x => x.AliasType == 2 && x.AliasStatus == 0))
                            {

                                //if G&G card does not exists throw error : 
                                customerSearchForCRCResponse.CRC = null;
                                customerSearchForCRCResponse.ErrorMessage = "Rewards cards does not exists for provided user";
                            }
                            else
                            {
                                customerSearchForCRCResponse.CRC = selectedCust.CrcId;
                            }
                        }
                        else
                            customerSearchForCRCResponse.CRC = null;
                    }
                    else
                        customerSearchForCRCResponse.CRC = null;
                }
                else
                    customerSearchForCRCResponse.CRC = null;

                //for winndixie we need to preappend the 9800 to the CRC
                if (inComingChainId == Banner.WD.GetAttribute<ChainIdAttribute>().Value && (!string.IsNullOrEmpty(customerSearchForCRCResponse.CRC)) && (!customerSearchForCRCResponse.CRC.StartsWith(Constants.WinnDixieCouponAliasPrefix)))
                    customerSearchForCRCResponse.CRC = string.Format("{0}{1}", Constants.WinnDixieCouponAliasPrefix, customerSearchForCRCResponse.CRC);
            }

            return customerSearchForCRCResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerSearchTruncatedResponse> CustomerSearchTruncatedAsync(CustomerSearchTruncatedRequest request)
        {
            CustomerSearchRequest customerSearchRequest = new CustomerSearchRequest();
            CustomerSearchTruncatedResponse customerSearchTruncatedResponse = new CustomerSearchTruncatedResponse();
            List<CustODSInfo> custODs = new List<CustODSInfo>();
            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.MobilePhoneNumber) && !string.IsNullOrWhiteSpace(request.EmailAddress))
                {
                    customerSearchTruncatedResponse.ErrorMessage = "Both MobilePHoneNumber and EmailAddress was provided.  This api allows only one or the other to be used";
                    return customerSearchTruncatedResponse;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.MobilePhoneNumber) && string.IsNullOrWhiteSpace(request.EmailAddress))
                    {
                        customerSearchTruncatedResponse.ErrorMessage = "Neither MobilePHoneNumber OR EmailAddress was provided.This api requires one or the other to be used";
                        return customerSearchTruncatedResponse;
                    }
                }


                if (!string.IsNullOrWhiteSpace(request.MobilePhoneNumber))
                {
                    customerSearchRequest.MobilePhoneNumber = request.MobilePhoneNumber;

                    if (!string.IsNullOrEmpty(customerSearchRequest.MobilePhoneNumber))
                    {
                        PhoneNumberValidationResponse phoneNumberValidationResponse = await _customerService.PhoneNumberValidation(customerSearchRequest.MobilePhoneNumber);
                        if (phoneNumberValidationResponse != null && phoneNumberValidationResponse.Status == "Success")
                        {
                            if (phoneNumberValidationResponse.mobilePhoneInvalid)
                            {
                                customerSearchTruncatedResponse.TransactionTypeMessage = phoneNumberValidationResponse.TransactionType;
                                customerSearchTruncatedResponse.mobilePhoneInvalid = phoneNumberValidationResponse.mobilePhoneInvalid;
                                return customerSearchTruncatedResponse;
                            }
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.EmailAddress))
                    {
                        customerSearchRequest.EmailAddress = request.EmailAddress;
                    }

                }

                CustomerSearchResponse customerSearchResponse = await _customerService.GetCustRecordAsync(customerSearchRequest);

                if (customerSearchResponse != null && customerSearchResponse.Customers != null)
                {
                    if (customerSearchResponse.Customers.Count > 0)
                    {
                        foreach (var cust in customerSearchResponse.Customers)
                        {
                            var custInfo = new CustODSInfo()
                            {
                                MobilePhoneNumber = cust.MobilePhone,
                                EmailAddress = cust.EmailAddress,
                                CreatedDate = cust.CreatedDate,
                                LastName = cust.LastName,
                                FirstName = cust.FirstName,
                                MemberId = cust.MemberId,
                                BirthDate = cust.BirthDate
                            };

                            custODs.Add(custInfo);
                        }


                        customerSearchTruncatedResponse.Customers = custODs;
                        customerSearchTruncatedResponse.IsSuccessful = true;
                    }

                }
                else
                {
                    customerSearchTruncatedResponse.ErrorMessage = "No customers Found";
                }
            }

            return customerSearchTruncatedResponse;
        }

        static object smsLockObject = new object();
        #region Api Calls

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets active jobs. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields the active jobs. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        async Task<List<AdhocSMSJob>> GetActiveJobs()
        {
            try
            {
                var results = await _adHocSmsJobService.GetJobsAsync(w => w.Finalized && (!w.ScheduledDateTime.HasValue || w.ScheduledDateTime <= DateTime.Now));
                return results.OrderBy(a => a.Id)
                               .Distinct()
                               .Take(100)
                               .ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets job items. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields the job items. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        async Task<List<AdHocSMSJobItem>> GetJobItems(int itemCount = 1000)
        {
            try
            {
                var results = await _adHocSmsJobItemService.GetJobItemsAsync(w => w.Processed == false &&
                        w.Parent.Finalized &&
                       w.Error == false &&
                      (w.Parent.ScheduledDateTime.HasValue == false || w.Parent.ScheduledDateTime <= DateTime.Now) &&
                      (w.DelayedSendDate.HasValue == false || w.DelayedSendDate <= DateTime.Now));
                var orderedResults = results.OrderBy(a => a.JobId)
                       .Distinct()
                       .Take(itemCount)
                       .ToList();

                return orderedResults;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds the SMS history record. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="history">  The history. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        long AddSmsHistoryRecord(SMSHistory history)
        {
            try
            {
                _smsHistoryRecordsService.Add(ref history);
                return history.Id;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the SMS Templates . </summary>
        ///
        /// <remarks>   Sam nanduri, 2/28/2018. </remarks>
        ///
        /// <param name="templateType">  The TemplateType. </param>
        ///
        /// <returns>   An asynchronous result that yields a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<List<SMSTemplate>> GetSmsTemplates(string templateType)
        {
            var results = await _smsTemplateService.GetTemplatesAsync();
            return results.ToList();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a job. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="job">  The job. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        async Task SaveJob(AdhocSMSJob job)
        {
            await _adHocSmsJobService.AddAsync(job);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a job item. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="job">  The job. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        async Task SaveJobItem(AdHocSMSJobItem item)
        {
            try
            {
                var isSMSJobItemSaved = await _adHocSmsJobItemService.UpdateAsync(item);
                if (!isSMSJobItemSaved) throw new Exception("AdHoc SMS job item not updated");

                var jobs = await _adHocSmsJobService.GetJobsAsync(j => j.Id == item.JobId);
                var adhocSmsJobs = jobs as AdhocSMSJob[] ?? jobs.ToArray();
                if (adhocSmsJobs.Any())
                {
                    var job = adhocSmsJobs.FirstOrDefault();
                    job.Completed = await _adHocSmsJobItemService.IsJobItemCompletedByJobIdAsync(job.Id);
                    if (job.Completed) job.CompletedDate = DateTime.Now;
                    else job.CompletedDate = null;

                    var missingItems = await _adHocSmsJobItemService.GetJobItemsAsync(a => a.MissingPhoneNumber && a.JobId == job.Id);
                    job.MissingPhoneNumber = missingItems.Count();

                    var notMissingItems = await _adHocSmsJobItemService.GetJobItemsAsync(a => !a.MissingPhoneNumber && a.JobId == job.Id);
                    job.MessagesSent = notMissingItems.Count();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private async Task AddToAzureQueueAsync(object queueObject, string queueMethodName, string queueNameType, bool isQueueRequest = false)
        {
            var queueClient = new QueueClient(_settings.AzureStorageConnectionString, queueNameType, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            {
                QueueTask queueTask = new QueueTask
                {
                    MethodName = queueMethodName,
                    QueueName = queueNameType,
                    QueueObject = queueObject,
                    ContinueOnError = true,
                    IsQueueRequest = isQueueRequest,
                    ApiTransactionId = log4net.LogicalThreadContext.Properties["apitransactionid"] == null ? String.Empty : log4net.LogicalThreadContext.Properties["apitransactionid"].ToString()
                };

                JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };
                string json = JsonConvert.SerializeObject(queueTask, settings).ToString();
                await queueClient.SendMessageAsync(json);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Process the SMS send authorization code. </summary>
        ///
        /// <remarks>   Sam Nanduri, 3/7/2018. </remarks>
        ///
        /// <returns>   An asynchronous result that yields a SMSHistory object if success or a null if failed. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        public async Task<SMSHistory> ProcessSMSSendAuthorizationCode(CancellationToken token,
                                                                            CustomerV2 customer, string authCode,
                                                                            string bannerUrl,
                                                                            string customerUrl,
                                                                           string expiryMessage,
                                                                            string template,
                                                                            Banner banner,
                                                                            string templateType = "AuthorizationCode")
        {
            try
            {
                if (!token.IsCancellationRequested)
                {


                    if (customer != null && !string.IsNullOrWhiteSpace(customer.MobilePhone))
                    {
                        templateType = templateType ?? "AuthorizationCode";

                        //There is going to be only one template ...
                        if (!string.IsNullOrWhiteSpace(template))
                        {
                            string authorizationCodeTemplateMessage = template.Clone() as string;


                            if (authorizationCodeTemplateMessage.Contains("{url}"))
                            {
                                if (!string.IsNullOrWhiteSpace(customerUrl))
                                {
                                    authorizationCodeTemplateMessage = authorizationCodeTemplateMessage.Replace("{url}", customerUrl);
                                }
                            }
                            if (authorizationCodeTemplateMessage.Contains("{banner}"))
                            {
                                if (!string.IsNullOrEmpty(bannerUrl))
                                {
                                    authorizationCodeTemplateMessage = authorizationCodeTemplateMessage.Replace("{banner}", bannerUrl);
                                }
                                else
                                {
                                    authorizationCodeTemplateMessage = authorizationCodeTemplateMessage.Replace("{banner}", banner.GetAttribute<BrandAttribute>().Value);
                                }
                            }
                            if (authorizationCodeTemplateMessage.Contains("{random}"))
                            {
                                authorizationCodeTemplateMessage = authorizationCodeTemplateMessage.Replace("{random}", authCode);
                            }

                            //New - add Expiry
                            if (authorizationCodeTemplateMessage.Contains("{expires}"))
                            {
                                authorizationCodeTemplateMessage = authorizationCodeTemplateMessage.Replace("{expires}", expiryMessage);
                            }


                            SMSReminder smsReminder = new SMSReminder
                            {
                                MobilePhoneNumber = customer.MobilePhone.Replace("+1", ""),
                                Message = authorizationCodeTemplateMessage,
                                MemberId = $"{customer.MemberId}",
                                Banner = banner,
                                AdhocSMSMessage = true,
                                TemplateType = templateType
                            };

                            string crcId = customer.CustomerCRC.FirstOrDefault().CrcId;

                            var smsHistory = new SMSHistory()
                            {
                                AdhocMesssage = true,
                                CrcId = Convert.ToDecimal(crcId),
                                SendDate = DateTime.Now,
                                MessageText = authorizationCodeTemplateMessage,
                                PhoneNumber = customer.MobilePhone,
                            };

                            smsReminder.SMSREminderID = AddSmsHistoryRecord(smsHistory);


                            string apiTransactionId = null;
                            await AddToAzureQueueAsync(smsReminder, QueueMethodNameType.SendSMS, "messaging", true);
                            return smsHistory;
                        }
                        else
                        {
                            //throw exception 
                            throw new Exception("AuthorizationCode template not found in the Loyalty Database");
                        }
                    }
                    else
                    {
                        throw new Exception("Customer cannot be null and should contain a CrCID, Phone #");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region SendGrid

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Process the send email to customer. </summary>
        ///
        /// <remarks>  Sam Nanduri, 2/26/2018. </remarks>
        ///
        /// <returns>   An result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        public async Task<bool> ProcessSendEmailToCustomerAsync(MessageObject msgObj, CancellationToken token)
        {
            bool success = true;
            try
            {
                if (!token.IsCancellationRequested)
                {
                    await AddToAzureQueueAsync(msgObj, QueueMethodNameType.SendEmailCustomer, "messaging", true);
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }


        #endregion

        #region Private Methods


        private void MapMethod(CustomerV2 customer, CredRetrieveResult credRetrieveResult)
        {
            if (customer != null)
            {
                credRetrieveResult.DOB = customer.BirthDate.ToString();
                if (!string.IsNullOrEmpty(customer.EmailAddress))
                    credRetrieveResult.EmailAddress = customer.EmailAddress;
                if (customer.CustomerAlias.Any(x => x.AliasType == 2 && x.AliasStatus == 0))
                {
                    credRetrieveResult.GNG_CARD = customer.CustomerAlias.FirstOrDefault(x => x.AliasType == 2 && x.AliasStatus == 0).AliasNumber;
                }

                credRetrieveResult.MEMBER_ID = customer.MemberId;
                credRetrieveResult.LockedOut = customer.PinLockout;
                credRetrieveResult.Password = customer.AccountPassword;
                if (customer.InvalidPasswordAttempts.HasValue)
                    credRetrieveResult.PasswordAttemptCount = customer.InvalidPasswordAttempts.Value;


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private async Task<CustomerV2> AssignSEGCard(CustomerV2 customer, string memberId)
        {
            if (customer == null || customer.CustomerAlias == null || !customer.CustomerAlias.Any(x => x.AliasType == 2 && x.AliasStatus == 0))
            {
                string newSEGCardNumber = await _cardRangeService.GetGeneratedCardNumberAsync();

                customer.CustomerAlias = new List<MemberAlias>();

                customer.CustomerAlias.Add(new MemberAlias()
                {
                    MemberId = memberId,
                    AliasNumber = newSEGCardNumber,
                    AliasStatus = (int)AliasStatusType.Active,
                    AliasType = (int)AliasType.SEGCardNumber,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateSource = customer.LastUpdateSource ?? "Loyalty save"

                });
            }

            return customer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        ///<param name="chainId"></param>
        /// <returns></returns>
        private async Task<CustomerResponse> CheckAndSaveCustomerOnLoginV2(CustomerV2 customer, string chainId)
        {

            CustomerResponse customerResponse = new CustomerResponse();

            CustomerCRC customerCRC = new CustomerCRC();
            Banner banner = Banner.WD;

            if (chainId.Trim().Equals("1"))
                banner = Banner.WD;
            if (chainId.Trim().Equals("2"))
                banner = Banner.Bilo;
            if (chainId.Trim().Equals("3"))
                banner = Banner.Harveys;

            if (customer != null && customer.CustomerCRC != null && customer.CustomerCRC.Count() > 0)
            {

                if (chainId == "1")
                    customerCRC = customer.CustomerCRC.FirstOrDefault(a => a.ChainId.Trim().Equals("1"));
                else
                    customerCRC = customer.CustomerCRC.FirstOrDefault(a => a.ChainId.Trim().Equals("2") || a.ChainId.Trim().Equals("3"));

                if (customerCRC == null)
                {
                    if (_settings.SaveCustOnLogin != null && _settings.SaveCustOnLogin == "true")
                    {

                        CustomerCRC newcustomerCRC = new CustomerCRC();
                        newcustomerCRC.ChainId = chainId;
                        var crcId = await _cardRangeService.GetGeneratedCrcAsync(banner);
                        newcustomerCRC.CrcId = crcId.ToString();
                        newcustomerCRC.MemberId = customer.MemberId;
                        customer.CustomerCRC.Add(newcustomerCRC);
                        //set passwor attempt to zero 
                        if (customer.InvalidPasswordAttempts > 0)
                        {
                            customer.InvalidPasswordAttempts = 0;
                        }

                        customerResponse = await _customerService.SaveCustomerAsync(customer);
                    }
                }
                else
                //set invalid password attempts 
                if (customer.InvalidPasswordAttempts > 0)
                {
                    CustomerV2 customerv2 = new CustomerV2();
                    customerv2.InvalidPasswordAttempts = 0;
                    customerv2.MemberId = customer.MemberId;
                    customerResponse = await _customerService.SaveCustomerAsync(customerv2);
                }
            }

            return customerResponse;
        }

        private async Task CheckAndSaveCustomerOnLogin(CustomerV2 customer, string chainId)
        {

            CustomerResponse customerResponse = new CustomerResponse();
            CustomerCRC customerCRC = new CustomerCRC();
            Banner banner = Banner.WD;

            if (chainId.Trim().Equals("1"))
                banner = Banner.WD;
            if (chainId.Trim().Equals("2"))
                banner = Banner.Bilo;
            if (chainId.Trim().Equals("3"))
                banner = Banner.Harveys;

            if (customer != null && customer.CustomerCRC != null && customer.CustomerCRC.Count() > 0)
            {

                if (chainId == "1")
                    customerCRC = customer.CustomerCRC.FirstOrDefault(a => a.ChainId.Trim().Equals("1"));
                else
                    customerCRC = customer.CustomerCRC.FirstOrDefault(a => a.ChainId.Trim().Equals("2") || a.ChainId.Trim().Equals("3"));

                if (customerCRC == null)
                {
                    if (_settings.SaveCustOnLogin != null && _settings.SaveCustOnLogin == "true")
                    {
                        CustomerCRC newcustomerCRC = new CustomerCRC();
                        newcustomerCRC.ChainId = chainId;
                        var crcId = await _cardRangeService.GetGeneratedCrcAsync(banner);
                        newcustomerCRC.CrcId = crcId.ToString();
                        newcustomerCRC.MemberId = customer.MemberId;
                        customer.CustomerCRC.Add(newcustomerCRC);
                        //set passwor attempt to zero 
                        if (customer.InvalidPasswordAttempts > 0)
                        {
                            customer.InvalidPasswordAttempts = 0;
                        }

                        customerResponse = await _customerService.SaveCustomerAsync(customer);
                    }
                }
                else
                //set invalid password attempts 
                if (customer.InvalidPasswordAttempts > 0)
                {
                    CustomerV2 customerv2 = new CustomerV2();
                    customerv2.InvalidPasswordAttempts = 0;
                    customerv2.MemberId = customer.MemberId;
                    customerResponse = await _customerService.SaveCustomerAsync(customerv2);
                }
            }
        }

        public async Task<CustomerPreferenceUpsertResponse> SaveCustomerEReceiptPreferenceAsync(CustomerPreference customerPreference)
        {
            return await _customerService.SaveCustomerEReceiptPreferenceAsync(customerPreference);
        }

        public async Task<CustomerPreferenceRetrieveResponse> GetCustomerEReceiptPreferenceAsync(string memberId = null, string mobilePhoneNumber = null, string emailAddess = null)
        {
            return await _customerService.GetCustomerEReceiptPreferenceAsync(memberId, mobilePhoneNumber, emailAddess);
        }

        public async Task<PiiRequest> DeletePiiAsync(string membershipId)
        {
            return await _customerService.DeletePiiAsync(membershipId);
        }

        public async Task<PrimaryStoreResponse> GetPrimaryStoreAsync(string memberId, int chainId)
        {
            return await _customerService.GetPrimaryStoreAsync(memberId, chainId);
        }

        #endregion Private Methods
    }
}