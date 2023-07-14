////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	CustomerService.cs
//
// summary:	Implements the customer service class
////////////////////////////////////////////////////////////////////////////////////////////////////

using AutoMapper;
using Flurl;
using Flurl.Http;
//using log4net   ;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.LoyaltyService.Models.Results;
using SEG.ApiService.Models.AppSettings;
using Microsoft.Extensions.Options;
using SEG.ApiService.Models.Clubs;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.PrimaryStore;
using SEG.CustomerWebService.Core.CustomExceptions;
using System.Net.Http;
using System.Net;

namespace SEG.CustomerWebService.Core
{
    public class CustomerService : ICustomerService
    {
        #region Static Variables

        private readonly AppSettingsOptions _settings;
        private readonly IMapper _mapper;

        // private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the connection string. </summary>
        ///
        /// <value> The connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ConnectionString { get; private set; }


        #endregion Static Variables  

        #region Properties    

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets URL of the base. </summary>
        ///
        /// <value> The base URL. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string BaseUrl { get; private set; }
        internal IMemoryCache _cache { get; set; }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the maximum number of search results. </summary>
        ///
        /// <value> The maximum number of search results. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int MaxNumberOfSearchResults { get; set; }

        #endregion Properties

        #region Constructors

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="baseUrl">                  The base URL. </param>
        /// <param name="databaseConnectionString"></param>
        /// <param name="maxNumberOfSearchResults"> The maximum number of search results. </param>

        ///
        /// <!-- Sam Nanduri changed the constructor to initialize maxNumberOfSearchResults to the DefaultMaxNumberOfSearchResults-->
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public CustomerService(IOptions<AppSettingsOptions> settings, IMapper mapper)
        {
            _settings = settings.Value;
            _mapper = mapper;
            _cache = null;


            MaxNumberOfSearchResults = Constants.DefaultMaxNumberOfSearchResults;
            BaseUrl =_settings.CustomerWebServiceBaseUrl;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => true;
            _mapper = mapper;
        }


        #endregion Constructors

        public async Task<CustomerV2> GetCustomerAsync(string memberId)
        {
            CustomerV2 customer = new CustomerV2();
            CustomerSearchRequest request = new CustomerSearchRequest()
            {
                MemberId = memberId
            };

            try
            {
                CustomerSearchResponse response = await CustomerSearchAsync(request);

                if (response != null)
                {
                    if (response.Customers != null && response.Customers.Count > 0)
                    {
                        customer = response.Customers.First();
                    }
                }
            }
            catch (Exception)
            {
                customer = null;
            }

            return customer;
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
            CustomerPreferenceRetrieveResponse customerPreferenceResponse = new CustomerPreferenceRetrieveResponse();

            try
            {
                if (string.IsNullOrWhiteSpace(memberId))
                    throw new ArgumentException("MemberID is a required field in the Request, and was not provided");
                if (string.IsNullOrWhiteSpace(chainId))
                    throw new ArgumentException("ChainId is a required field in the Request, and was not provided");

                var banner = (Banner)int.Parse(chainId);
                var UrlStringBase = BaseUrl.AppendPathSegment(Constants.RetrievePreference).AllowAnyHttpStatus();

                UrlStringBase = UrlStringBase.SetQueryParam(Constants.CustPreferenceMemberId, memberId).SetQueryParam(Constants.CustPreferenceBanner, banner);

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    CustomerPreference customerPreference = JsonConvert.DeserializeObject<CustomerPreference>(json);
                    customerPreferenceResponse.CustomerPreference = customerPreference;
                    customerPreferenceResponse.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                customerPreferenceResponse.IsSuccessful = false;
                customerPreferenceResponse.ErrorMessage = string.Format("Customer Preference not found with Error Message: {0}", ex.Message);
            }

            return customerPreferenceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="crcid"></param>
        /// <param name="aliasNumber"></param>
        /// <returns></returns>
        /// 

        /////##################################################################################################################################////////
        ////////////***********************************************Not required any more after CustODS redesign ******************************//////
        /////////////////########################################################################################################//////////////////


        public async Task<FRNStatusValidateResponse> GetFRNStatus(string memberid = null, string crcid = null, string aliasNumber = null)
        {
            FRNStatusValidateResponse fRNStatusValidateResponse = null;

            try
            {
                var UrlStringBase = BaseUrl.AppendPathSegment(Constants.OmniFRNStatusValidate).AllowAnyHttpStatus();


                if (!string.IsNullOrEmpty(memberid))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.CustPreferenceMemberId, memberid);
                }

                if (!string.IsNullOrEmpty(crcid))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("CRC_ID", crcid);
                }

                if (!string.IsNullOrEmpty(aliasNumber))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("ALIAS_NUMBER", aliasNumber);
                }

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    fRNStatusValidateResponse = JsonConvert.DeserializeObject<FRNStatusValidateResponse>(json);
                }
            }
            catch (Exception)
            {
                //Logging.Error("Error calling FRNStatusValidate", e);
            }

            return fRNStatusValidateResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<PhoneNumberValidationResponse> PhoneNumberValidation(string phoneNumber = null)
        {
            PhoneNumberValidationResponse phoneNumberValidationResponse = null;

            try
            {
                var UrlStringBase = BaseUrl.AppendPathSegment(Constants.CustPhoneValidation).AllowAnyHttpStatus();


                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("mobilePhone", phoneNumber);
                }

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    phoneNumberValidationResponse = JsonConvert.DeserializeObject<PhoneNumberValidationResponse>(json);
                }
            }
            catch (Exception)
            {
                // Logging.Error("Error calling FRNStatusValidate", e);
            }

            return phoneNumberValidationResponse;
        }

        #region Save

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer">             The customer. </param>
        ///
        /// <returns>   An asynchronous result that yields the save customer. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<CustomerResponse> SaveCustomerAsync(CustomerV2 customer)
        {

            try
            {


                CustomerResponse response = new CustomerResponse();

                SaveCustomerResult result = await SaveCustomerAsyncWebService(customer);
                if (result != null)
                {
                    response.IsSuccessful = result.Status != Constants.ErrorStatus;
                    response.WalletId = result.WalletId;
                    if (result.ErrorDesc != null)
                    {
                        response.ErrorMessages = new List<string>() { result.Error, result.ErrorCode, result.ErrorDesc };
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer preferences asynchronous. </summary>
        ///
        /// <remarks>   Mark, 9/1/2020. </remarks>
        ///
        /// <param name="customerPreference">                . </param>
        ///
        /// <returns>   An asynchronous result that yields the save customer preferences. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<CustomerPreferenceUpsertResponse> SaveCustomerPreference (CustomerPreference customerPreference)
        {
            try
            {
                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment(Constants.RetrieveUpsert).PostJsonAsync(customerPreference);

                return JsonConvert.DeserializeObject<CustomerPreferenceUpsertResponse>(await attrResp.Content.ReadAsStringAsync());
            }
            catch 
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer alias. </summary>
        ///
        /// <remarks>   Mcdand, 7/17/2018. </remarks>
        ///
        /// <param name="crcId">                . </param>
        /// <param name="alias">                The alias. </param>
        /// <param name="returnHydratedObject"> . </param>
        ///
        /// <returns>   An asynchronous result that yields a SaveMemberAliasResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<SaveMemberAliasResult> SaveCustomerAlias(string crcId, MemberAlias alias, bool returnHydratedObject)
        {
            try
            {

                alias.LastUpdateDate = DateTime.Now;

                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment(Constants.UpsertAlias).PostJsonAsync(alias);



                return JsonConvert.DeserializeObject<SaveMemberAliasResult>(await attrResp.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveCustomerPinPasswordResult> SaveCustomerPinPassword(SaveCustomerPinPasswordRequest request)
        {
            try
            {
                //alias.LastUpdateDate = DateTime.Now;

                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment(Constants.UpsertCred).PostJsonAsync(request);



                return JsonConvert.DeserializeObject<SaveCustomerPinPasswordResult>(await attrResp.Content.ReadAsStringAsync());

            }
            catch (Exception)
            {
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets validated phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="request">  The request. </param>
        ///
        /// <returns>   An asynchronous result that yields a SetValidatedPhoneNumberResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<SetValidatedPhoneNumberResponse> SetValidatedPhoneNumber(SetValidatedPhoneNumberRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.MemberId))
                throw new ArgumentException("MemberID is a required field in the Request, and was not provided");
            if (!string.IsNullOrWhiteSpace(request.MobilePhoneNumber) && !string.IsNullOrWhiteSpace(request.EmailAddress))
                throw new ArgumentException("Both MobilePHoneNumber and EmailAddress was provided.  This api allows only one or the other to be used");
            else if (string.IsNullOrWhiteSpace(request.MobilePhoneNumber) && string.IsNullOrWhiteSpace(request.EmailAddress))
                throw new ArgumentException("Neither MobilePHoneNumber OR EmailAddress was provided.  This api requires one or the other to be used");


            if (!string.IsNullOrEmpty(request.MobilePhoneNumber))
            {
                request.MobilePhoneNumber = Regex.Replace(request.MobilePhoneNumber, "[^0-9]", "");
            }


            if (request.ThrowErrorIfExistsOnOtherAccounts)
            {

                var searchResults = await HandleHardSearchAsync(new CustomerSearchRequest()
                {
                    MobilePhoneNumber = request.MobilePhoneNumber,
                    EmailAddress = request.EmailAddress
                });
                if (searchResults.IsSuccessful && searchResults.Customers != null && searchResults.Customers.Any())
                {
                    if (searchResults.Customers.Any(a => a.MemberId != request.MemberId))
                    {
                        string searchType = (string.IsNullOrWhiteSpace(request.MobilePhoneNumber) ? string.Empty : "Mobile Number") +
                            (string.IsNullOrWhiteSpace(request.EmailAddress) ? string.Empty : "Email Address");
                        return new SetValidatedPhoneNumberResponse()
                        {
                            Status = "EXISTS",
                            MobilePhoneNumber = request.MobilePhoneNumber,
                            MemberId = request.MemberId,
                            EmailAddress = request.EmailAddress,
                            ErrorDescription = $"{searchType} already exists on a different Member's account"
                        };
                    }
                }
            }

            var restResponse = await BaseUrl.AppendPathSegments(Constants.UpdateMobileResource).AllowAnyHttpStatus().PostJsonAsync(request);

            if (restResponse.IsSuccessStatusCode)
            {

                var json = await restResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SetValidatedPhoneNumberResponse>(json);
            }
            else
            {
                // Logging.Error("Error Setting Validated Phone NUmber: " + restResponse.ReasonPhrase);
                return new SetValidatedPhoneNumberResponse() { Status = "Error", ErrorDescription = restResponse.ReasonPhrase };
            }
        }

        #endregion Save

        #region Verify      

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer">         The customer. </param>
        /// <param name="errorMessages">    [out] The error messages. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool VerifyCustomer(CustomerV2 customer, out List<string> errorMessages)
        {
            bool valid = true;
            errorMessages = new List<string>();

            if (customer == null)
            {
                errorMessages.Add("Null request");
                return false;
            }

            if (customer.CustomerCRC != null && customer.CustomerCRC.Count > 0)
            {
                foreach (CustomerCRC customerCRC in customer.CustomerCRC)
                {
                    if (customerCRC != null || !string.IsNullOrEmpty(customerCRC.CrcId))
                    {
                        bool canConvert = decimal.TryParse(customerCRC.CrcId, out decimal convertedCrc);

                        if (!canConvert)
                        {
                            errorMessages.Add(string.Format("Invalid CrcId.  CrcId Value {0}", customerCRC.CrcId));
                            valid = false;
                        }


                        if (valid)
                        {
                            //Verify CRC is Valid for Domain

                            Banner banner = (Banner)(int.Parse(customerCRC.ChainId));
                            string staticCrcKey = string.Format("{0}.{1}", Constants.StaticCrcIdAppSettingKey, banner.GetAttribute<MetadataAttribute>().Value);
                            string bannerStaticKey = string.Empty;

                            switch (banner)
                            {
                                case Banner.WD:
                                    bannerStaticKey = Constants.BannerStaticKeyWD;
                                    break;
                                case Banner.Bilo:
                                    bannerStaticKey = Constants.BannerStaticKeyBilo;
                                    break;
                                case Banner.Harveys:
                                    bannerStaticKey = Constants.BannerStaticKeyHarveys;
                                    break;
                            }


                            if (customerCRC.CrcId == bannerStaticKey)
                            {
                                errorMessages.Add("CrcId is set to the static crc.  A new CrcId must be generated, before saving the customer.");
                                valid = false;
                            }
                            else if (decimal.Parse(customerCRC.CrcId) > Constants.BeginningSequenceForRainChecks && decimal.Parse(customerCRC.CrcId) < Constants.EndingSequenceForRainChecks)
                            {
                                errorMessages.Add("CrcId is set to the Raincheck ID.  A new CrcId must be generated, before saving the customer.");
                                valid = false;
                            }

                            if (!(string.IsNullOrWhiteSpace(customerCRC.ChainId)) && customerCRC.ChainId.Length > Constants.ChainIdMaxLength)
                            {
                                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, "ChainIdMaxLength", Constants.ChainIdMaxLength, customerCRC.ChainId.Length, customerCRC.ChainId));
                                valid = false;
                            }
                        }
                    }
                }
            }
            if (!(string.IsNullOrWhiteSpace(customer.FirstName)) && customer.FirstName.Length > Constants.FirstNameMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerFirstName, Constants.FirstNameMaxLength, customer.FirstName.Length, customer.FirstName));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customer.LastName)) && customer.LastName.Length > Constants.LastNameMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerLastName, Constants.LastNameMaxLength, customer.LastName.Length, customer.LastName));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customer.Salutation)) && customer.Salutation.Length > Constants.SalutationMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerSaluation, Constants.SalutationMaxLength, customer.Salutation.Length, customer.Salutation));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customer.EmailAddress)) && customer.EmailAddress.Length > Constants.EmailMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerEmailAddress, Constants.EmailMaxLength, customer.EmailAddress.Length, customer.EmailAddress));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customer.MobilePhone)) && customer.MobilePhone.Length > Constants.PhoneMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerMobilePhoneNumber, Constants.PhoneMaxLength, customer.MobilePhone.Length, customer.MobilePhone));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customer.LanguageCode)) && customer.LanguageCode.Length > Constants.LanguageCodeMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerLanguageCode, Constants.LanguageCodeMaxLength, customer.LanguageCode.Length, customer.LanguageCode));
                valid = false;
            }

            if (!(string.IsNullOrWhiteSpace(customer.MemberId)) && customer.MemberId.Length > Constants.MemberIdMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerMemberId, Constants.MemberIdMaxLength, customer.MemberId.Length, customer.MemberId));
                valid = false;
            }

            if (customer.CustomerAddress != null && customer.CustomerAddress.Count() > 0)
            {
                foreach (CustomerAddress address in customer.CustomerAddress)
                {
                    List<string> addressErrorMessages = new List<string>();
                    if (!VerifyCustomerAddress(address, out addressErrorMessages))
                    {
                        errorMessages.AddRange(addressErrorMessages);
                        valid = false;
                    }
                }
            }

            if (customer.CustomerChild != null && customer.CustomerChild.Count() > 0)
            {
                foreach (CustomerChild child in customer.CustomerChild)
                {
                    List<string> childErrorMessages = new List<string>();
                    if (!VerifyCustomerChild(child, out childErrorMessages))
                    {
                        errorMessages.AddRange(childErrorMessages);
                        valid = false;
                    }
                }
            }

            if (customer.CustomerAlias != null && customer.CustomerAlias.Count > 0)
            {
                foreach (MemberAlias alias in customer.CustomerAlias.Where(w => w != null))
                {
                    List<string> memberErrorMessages = new List<string>();
                    if (!VerifyMemberAlias(alias, out memberErrorMessages))
                    {
                        errorMessages.AddRange(memberErrorMessages);
                        valid = false;
                    }
                }
            }

            if (!valid)
            {
                //Logging.Error(string.Format("Customer validation failed with the following error(s) {0}", string.Join(", ", errorMessages)));
            }

            return valid;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify customer address. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerAddress">  The customer address. </param>
        /// <param name="errorMessages">    [out] The error messages. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool VerifyCustomerAddress(CustomerAddress customerAddress, out List<string> errorMessages)
        {
            bool valid = true;
            errorMessages = new List<string>();

            if (customerAddress == null)
            {
                errorMessages.Add("Address is null");

                return false;
            }

            if (!(string.IsNullOrWhiteSpace(customerAddress.AddressLine1)) && customerAddress.AddressLine1.Length > Constants.AddressLine1MaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerAddressLine1, Constants.AddressLine1MaxLength, customerAddress.AddressLine1.Length, customerAddress.AddressLine1));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerAddress.AddressLine2)) && customerAddress.AddressLine2.Length > Constants.AddressLine2MaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerAddressLine2, Constants.AddressLine2MaxLength, customerAddress.AddressLine2.Length, customerAddress.AddressLine2));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerAddress.City)) && customerAddress.City.Length > Constants.CityMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerCity, Constants.CityMaxLength, customerAddress.City.Length, customerAddress.City));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerAddress.State)) && customerAddress.State.Length > Constants.StateMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerState, Constants.StateMaxLength, customerAddress.State.Length, customerAddress.State));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerAddress.Country)) && customerAddress.Country.Length > Constants.CountryMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerCountry, Constants.CountryMaxLength, customerAddress.Country.Length, customerAddress.Country));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerAddress.PostalCode)) && customerAddress.PostalCode.Length > Constants.PostalCodeMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerPostalCode, Constants.PostalCodeMaxLength, customerAddress.PostalCode.Length, customerAddress.PostalCode));
                valid = false;
            }

            return valid;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify customer child. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerChild">    The customer child. </param>
        /// <param name="errorMessages">    [out] The error messages. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool VerifyCustomerChild(CustomerChild customerChild, out List<string> errorMessages)
        {
            bool valid = true;
            errorMessages = new List<string>();

            if (customerChild == null)
            {
                errorMessages.Add("Child is null");
                return false;
            }

            if (!(string.IsNullOrWhiteSpace(customerChild.FirstName)) && customerChild.FirstName.Length > Constants.FirstNameMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerFirstName, Constants.FirstNameMaxLength, customerChild.FirstName.Length, customerChild.FirstName));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(customerChild.LastName)) && customerChild.LastName.Length > Constants.LastNameMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerLastName, Constants.LastNameMaxLength, customerChild.LastName.Length, customerChild.LastName));
                valid = false;
            }


            return valid;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify member alias. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="alias">            The alias. </param>
        /// <param name="errorMessages">    [out] The error messages. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool VerifyMemberAlias(MemberAlias alias, out List<string> errorMessages)
        {
            bool valid = true;
            errorMessages = new List<string>();

            if (alias == null)
            {
                errorMessages.Add("MemberAlias is null");
                return false;
            }

            if (!(string.IsNullOrWhiteSpace(alias.AliasNumber)) && alias.AliasNumber.Length > Constants.AliasNumberMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, "AliasNumber", Constants.AliasNumberMaxLength, alias.AliasNumber.Length, alias.AliasNumber));
                valid = false;
            }
            if (!(string.IsNullOrWhiteSpace(alias.MemberId)) && alias.MemberId.Length > Constants.MemberIdMaxLength)
            {
                errorMessages.Add(string.Format(Constants.BadParameterLengthErrorMessage, Constants.CustomerMemberId, Constants.MemberIdMaxLength, alias.MemberId.Length, alias.MemberId));
                valid = false;
            }

            return valid;
        }

        #endregion Verify

        #region Search



        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Customer search asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerSearchRequest">    The customer search request. </param>
        /// <param name="retryCount">               (Optional) Number of retries. </param>
        /// <param name="hardPhoneNumberSearch">    (Optional) True to hard phone number search. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer search. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<CustomerSearchResponse> CustomerSearchAsync(CustomerSearchRequest customerSearchRequest, int retryCount = 0, bool hardPhoneNumberSearch = true)
        {
            try
            {
                if (customerSearchRequest == null) return null;
                CustomerSearchResponse customerSearchResponse = new CustomerSearchResponse
                {
                    IsSuccessful = true
                };

                string cacheKey = null;


                if (_cache != null)
                {
                    cacheKey = JsonConvert.SerializeObject(customerSearchRequest).GenerateHashString();
                    if (_cache.TryGetValue(cacheKey, out customerSearchResponse))
                        return customerSearchResponse;
                }

                if (customerSearchRequest != null && (!string.IsNullOrWhiteSpace(customerSearchRequest.MemberId)
                    || !string.IsNullOrWhiteSpace(customerSearchRequest.EmailAddress)
                    || !string.IsNullOrWhiteSpace(customerSearchRequest.CrcId)
                    || !string.IsNullOrWhiteSpace(customerSearchRequest.OmniId))
                    || (!string.IsNullOrWhiteSpace(customerSearchRequest.MobilePhoneNumber)))
                {
                    if (!(string.IsNullOrWhiteSpace(customerSearchRequest.MobilePhoneNumber)))
                        customerSearchRequest.MobilePhoneNumber = StripInternationPhoneNumberSpecs(customerSearchRequest.MobilePhoneNumber);

                    customerSearchResponse = await HandleHardSearchAsync(customerSearchRequest);
                }

                //***********************************fuzzy search  * *************************************//////
                else if (customerSearchRequest != null)
                {
                    if (customerSearchRequest.MaxNumberOfReturnedResults == 0)
                    {
                        customerSearchRequest.MaxNumberOfReturnedResults = MaxNumberOfSearchResults;
                    }

                    customerSearchResponse = await HandleFuzzySearchAsync(customerSearchRequest);
                }

                if (_cache != null && cacheKey != null)
                {
                    _cache.Set(cacheKey, customerSearchResponse, new DateTimeOffset(DateTime.Now.AddSeconds(10)));
                }

                return customerSearchResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the customer child asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="crcId">    . </param>
        /// <param name="childId">  Identifier for the child. </param>
        ///
        /// <returns>   An asynchronous result that yields the delete customer child. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> DeleteCustomerChildAsync(string crcId, int childId)
        {
            Stopwatch deleteCustomerChildWatch = Stopwatch.StartNew();
            Stopwatch deleteCustomerChildCustomerWatch = new Stopwatch();
            deleteCustomerChildWatch.Start();

            SaveCustomerResult saveCustomerResult = new SaveCustomerResult() { Status = Constants.ErrorStatus };

            try
            {
                deleteCustomerChildCustomerWatch.Start();
                var result = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment(Constants.UpsertDelete).SetQueryParam(Constants.CrcIdDeleteParameter, crcId).SetQueryParam(Constants.ChildIDDeleteParameter, childId).DeleteAsync();

                if (result.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                    };

                    var json = await result.Content.ReadAsStringAsync();
                    saveCustomerResult = JsonConvert.DeserializeObject<SaveCustomerResult>(json, settings);
                }
                else
                    saveCustomerResult.Status = Constants.ErrorStatus;


                deleteCustomerChildCustomerWatch.Stop();

            }
            catch (Exception)
            {
                //Logging.Error(string.Format("An exception occurred while trying to delete a child with id {0} from crcId {1}. Error {2}", childId, crcId, ex.Message), ex);
            }
            finally
            {
                deleteCustomerChildCustomerWatch.Stop();
                deleteCustomerChildWatch.Stop();
                // Logging.StatisticFormat("DeleteCustomerChild took {0} [Customer Service Delete Child Took: {1}", deleteCustomerChildWatch.Elapsed, deleteCustomerChildCustomerWatch.Elapsed);
            }

            return saveCustomerResult.Status != Constants.ErrorStatus;
        }
        #endregion

        /////
        private string StripInternationPhoneNumberSpecs(string input)
        {

            switch (input.Length)
            {
                case 11:
                case 12:
                    input = input.Trim().TrimStart('+').TrimStart('1');
                    break;
                default:
                    break;
            }
            return input;
        }

        #region Search

        private async Task<CustomerSearchResponse> HandleFuzzySearchAsync(CustomerSearchRequest customerSearchRequest)
        {
            Stopwatch handleFuzzySearchWatch = Stopwatch.StartNew();
            Stopwatch handleFuzzySearchCustomerWatch = new Stopwatch();
            handleFuzzySearchWatch.Start();
            var UrlStringBase = BaseUrl.AppendPathSegment(Constants.CustSearchFuzzy).AllowAnyHttpStatus();

            CustomerSearchResponse customerSearchResponse = new CustomerSearchResponse();
            try
            {

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.FirstName))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.FIRST_NAMESearchParameter, customerSearchRequest.FirstName);

                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.LastName))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.LAST_NAMESearchParameter, customerSearchRequest.LastName);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.AddressLine1))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.STREET_ADDRESS_1SearchParameter, customerSearchRequest.AddressLine1);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.City))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.CITY_NAMESearchParameter, customerSearchRequest.City);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.State))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.STATE_CODESearchParameter, customerSearchRequest.State);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.Zipcode))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.POSTAL_CODESearchParameter, customerSearchRequest.Zipcode);
                }
                if (customerSearchRequest.MaxNumberOfReturnedResults > 0)
                {
                    UrlStringBase = UrlStringBase.SetQueryParam(Constants.ROWSSearchParameter, customerSearchRequest.MaxNumberOfReturnedResults);
                }
                handleFuzzySearchCustomerWatch.Start();
                var response = await UrlStringBase.GetAsync();
                handleFuzzySearchCustomerWatch.Stop();

                FuzzySearchResult result = null;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (content.StartsWith(Constants.ErrorXmlTag))
                    {
                        XElement errorNode = XElement.Parse(content);
                        customerSearchResponse.ErrorMessage = errorNode.Value;
                        customerSearchResponse.IsSuccessful = false;
                        // Logging.Error(string.Format("HandleFuzzySearch failed with the following error {0}", errorNode.Value));
                    }
                    else
                    {
                        using (StringReader reader = new StringReader(content))
                        {
                            using (XmlReader xmlReader = XmlReader.Create(reader))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(FuzzySearchResult));
                                result = (FuzzySearchResult)serializer.Deserialize(xmlReader);
                            }

                            List<CustomerV2> customers = _mapper.Map<List<CustomerV2>>(result.Customers);

                            customerSearchResponse.Customers = new List<CustomerV2>();
                            customerSearchResponse.Customers = customers;
                            customerSearchResponse.IsSuccessful = true;
                        }
                    }
                }
                else
                {
                    customerSearchResponse.ErrorMessage = response?.ReasonPhrase ?? Constants.ResponseErrorMessage;
                    customerSearchResponse.IsSuccessful = false;
                    //Logging.Error(string.Format("HandleFuzzySearch failed with the following error {0}", customerSearchResponse.ErrorMessage));
                }
            }
            catch (Exception)
            {
                //Logging.Error(string.Format("An exception occurred while trying to search (HandleFuzzySearch) for a customer. Error {0}", ex.Message), ex);
                customerSearchResponse.IsSuccessful = false;
            }
            finally
            {
                handleFuzzySearchCustomerWatch.Stop();
                handleFuzzySearchWatch.Stop();
                //Logging.StatisticFormat("HandleFuzzySearch took {0} [Customer Service Fuzzy Search Took: {1}", handleFuzzySearchWatch.Elapsed, handleFuzzySearchCustomerWatch.Elapsed);
            }

            return customerSearchResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSearchRequest"></param>
        /// <returns></returns>
        public async Task<CustomerSearchResponse> GetCustRecordAsync(CustomerSearchRequest customerSearchRequest)
        {
            CustomerSearchResponse customerSearchResponse = new CustomerSearchResponse();
            try
            {
                var request = BaseUrl.AppendPathSegment(Constants.CustSearch).AllowAnyHttpStatus();
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.MemberId))
                {
                    request.SetQueryParam(Constants.MEMBER_IDSearchParameter, customerSearchRequest.MemberId);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.EmailAddress))
                {
                    request.SetQueryParam(Constants.EMAIL_IDSearchParameter, customerSearchRequest.EmailAddress);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.MobilePhoneNumber))
                {
                    request.SetQueryParam(Constants.MOBILE_PHONESearchParameter, customerSearchRequest.MobilePhoneNumber);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.CrcId))
                {
                    if (decimal.TryParse(customerSearchRequest.CrcId, out decimal crcId))
                    {
                        if (crcId >= Constants.CrcIdMax) crcId -= Constants.CrcIdMax;

                        var formatControl = new System.Globalization.NumberFormatInfo
                        {
                            NumberDecimalDigits = 0
                        };

                        request.SetQueryParam(Constants.CRC_IDSearchParameter, crcId.ToString(formatControl));
                    }
                    else
                        request.SetQueryParam(Constants.CRC_IDSearchParameter, customerSearchRequest.CrcId);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.OmniId))
                {
                    request.SetQueryParam(Constants.ALIAS_NUMBERSearchParameter, customerSearchRequest.OmniId);
                }

                var response = await request.GetAsync();
                var Content = await response.Content.ReadAsStringAsync();


                HardSearchResult result = null;
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (Content.StartsWith(Constants.ErrorXmlTag))
                    {
                        XElement errorNode = XElement.Parse(Content);
                        customerSearchResponse.ErrorMessage = errorNode.Value;
                        customerSearchResponse.IsSuccessful = false;

                        throw new ApplicationException(string.Format("SearchAsync failed with the following error {0}", errorNode.Value));
                    }
                    else
                    {
                        using (StringReader reader = new StringReader(Content))
                        {
                            using (XmlReader xmlReader = XmlReader.Create(reader))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(HardSearchResult));
                                result = (HardSearchResult)serializer.Deserialize(xmlReader);
                            }

                            List<CustomerV2> customers = _mapper.Map<List<CustomerV2>>(result.Customers);
                            customerSearchResponse.Customers = customers;
                            customerSearchResponse.IsSuccessful = true;
                        }
                    }
                }
                else
                {
                    throw new ApplicationException(string.Format("SearchAsync failed with the following error {0}", customerSearchResponse.ErrorMessage));
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };



                var payload = JsonConvert.SerializeObject(customerSearchRequest, settings);


                //Logging.Error(string.Format("An exception occurred while trying to search (HandleHardSearch) for a customer. Error {0}", ex.Message), ex);
            }
            finally
            {
                //Logging.StatisticFormat("HandleHardSearch took {0} [Customer Service Hard Search Took: {1}", handleHardSearchWatch.Elapsed, handleHardSearchCustomerWatch.Elapsed);
            }

            return customerSearchResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSearchRequest"></param>
        /// <returns></returns>
        public async Task<CustomerAliasSearchResponse> CustAliasSearchAsync(CustomerSearchRequest customerSearchRequest)
        {
            CustomerAliasSearchResponse aliasResponse = new CustomerAliasSearchResponse();
            try
            {
                var request = BaseUrl.AppendPathSegment(Constants.CustAliasRetrieve).AllowAnyHttpStatus();
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.MemberId))
                {
                    request.SetQueryParam(Constants.MEMBER_IDSearchParameter, customerSearchRequest.MemberId);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.OmniId))
                {
                    request.SetQueryParam(Constants.ALIAS_NUMBERSearchParameter, customerSearchRequest.OmniId);
                }

                var response = await request.GetAsync();
                var Content = await response.Content.ReadAsStringAsync();

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<MemberAlias> memberAliases = JsonConvert.DeserializeObject<List<MemberAlias>>(await response.Content.ReadAsStringAsync());
                    aliasResponse.Alias = memberAliases;
                    aliasResponse.IsSuccessful = true;
                }
                else
                {
                    throw new ApplicationException(string.Format("CustAliasSearchAsync failed with the following error {0}", aliasResponse.ErrorMessage));
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };



                var payload = JsonConvert.SerializeObject(customerSearchRequest, settings);


                //Logging.Error(string.Format("An exception occurred while trying to search (HandleHardSearch) for a customer. Error {0}", ex.Message), ex);
            }
            finally
            {
                //Logging.StatisticFormat("HandleHardSearch took {0} [Customer Service Hard Search Took: {1}", handleHardSearchWatch.Elapsed, handleHardSearchCustomerWatch.Elapsed);
            }

            return aliasResponse;
        }

        private async Task<CustomerSearchResponse> HandleHardSearchAsync(CustomerSearchRequest customerSearchRequest)
        {
            Stopwatch handleHardSearchWatch = Stopwatch.StartNew();
            Stopwatch handleHardSearchCustomerWatch = new Stopwatch();
            handleHardSearchWatch.Start();

            CustomerSearchResponse customerSearchResponse = new CustomerSearchResponse();
            try
            {
                var request = BaseUrl.AppendPathSegment(Constants.CustSearchHard).AllowAnyHttpStatus();
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.MemberId))
                {
                    request.SetQueryParam(Constants.MEMBER_IDSearchParameter, customerSearchRequest.MemberId);
                }
                if (!string.IsNullOrWhiteSpace(customerSearchRequest.CrcId))
                {
                    if (decimal.TryParse(customerSearchRequest.CrcId, out decimal crcId))
                    {
                        if (crcId >= Constants.CrcIdMax) crcId -= Constants.CrcIdMax;

                        var formatControl = new System.Globalization.NumberFormatInfo
                        {
                            NumberDecimalDigits = 0
                        };

                        request.SetQueryParam(Constants.CRC_IDSearchParameter, crcId.ToString(formatControl));
                    }
                    else
                        request.SetQueryParam(Constants.CRC_IDSearchParameter, customerSearchRequest.CrcId);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.OmniId))
                {
                    request.SetQueryParam(Constants.ALIAS_NUMBERSearchParameter, customerSearchRequest.OmniId);
                }

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.EmailAddress))
                {
                    request.SetQueryParam(Constants.EMAIL_IDSearchParameter, customerSearchRequest.EmailAddress);
                }

                //if (customerSearchRequest.Banner.HasValue)
                //{
                //    request.SetQueryParam(CHAIN_IDSearchParameter, customerSearchRequest.Banner.Value.GetAttribute<ChainIdAttribute>().Value);
                //}

                if (!string.IsNullOrWhiteSpace(customerSearchRequest.MobilePhoneNumber))
                {
                    request.SetQueryParam(Constants.MOBILE_PHONESearchParameter, customerSearchRequest.MobilePhoneNumber);
                }

                string baseUrl = BaseUrl;

                handleHardSearchCustomerWatch.Start();
                var response = await request.GetAsync();
                var Content = await response.Content.ReadAsStringAsync();
                handleHardSearchCustomerWatch.Stop();

                HardSearchResult result = null;
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (Content.StartsWith(Constants.ErrorXmlTag))
                    {
                        XElement errorNode = XElement.Parse(Content);
                        customerSearchResponse.ErrorMessage = errorNode.Value;
                        customerSearchResponse.IsSuccessful = false;

                        throw new NotFoundException(String.Format("The customer you are trying to search for has been deleted."), HttpStatusCode.BadRequest);

                    }
                    else
                    {
                        using (StringReader reader = new StringReader(Content))
                        {
                            using (XmlReader xmlReader = XmlReader.Create(reader))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(HardSearchResult));
                                result = (HardSearchResult)serializer.Deserialize(xmlReader);
                            }

                            var config = new MapperConfiguration(cfg => {
                                cfg.AddProfile<WebServiceMappingsProfile>();
                            });
                            var CustomerResultToCustomerMappingConfig = config;
                            IMapper mapper = config.CreateMapper();

                            List<CustomerV2> customers = mapper.Map<List<CustomerV2>>(result.Customers);
                            customerSearchResponse.Customers = customers;
                            customerSearchResponse.IsSuccessful = true;
                        }
                    }
                }
                else
                {
                    throw new ApplicationException(string.Format("HandleHardSearch failed with the following error {0}", customerSearchResponse.ErrorMessage));
                }
            }
            catch (NotFoundException ex)
            {
                var response = new HttpResponseMessage(ex.StatusCode)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(response);

                //HttpStatusCode statusCode = ex.StatusCode;
                //throw ex;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };



                var payload = JsonConvert.SerializeObject(customerSearchRequest, settings);


                //Logging.Error(string.Format("An exception occurred while trying to search (HandleHardSearch) for a customer. Error {0}", ex.Message), ex);
            }
            finally
            {
                handleHardSearchCustomerWatch.Stop();
                handleHardSearchWatch.Stop();
                //Logging.StatisticFormat("HandleHardSearch took {0} [Customer Service Hard Search Took: {1}", handleHardSearchWatch.Elapsed, handleHardSearchCustomerWatch.Elapsed);
            }

            return customerSearchResponse;
        }

        #endregion Search      


        private async Task<SaveCustomerResult> SaveCustomerAsyncWebService(CustomerV2 customer)
        {
            List<string> errorMessages = new List<string>();
            Stopwatch saveCustomerWatch = Stopwatch.StartNew();
            Stopwatch saveCustomerWatchCustomerWatch = new Stopwatch();
            saveCustomerWatch.Start();

            SaveCustomerResult saveCustomerResult = new SaveCustomerResult();

            try
            {
                var numberRegex = new Regex("[^0-9]");

                if (!string.IsNullOrWhiteSpace(customer.MobilePhone))
                {
                    customer.MobilePhone = numberRegex.Replace(customer.MobilePhone, string.Empty);
                    if (customer.MobilePhone.Trim().Length > 10)
                    {
                        var custMobileNum = customer.MobilePhone.Trim();

                        customer.MobilePhone = custMobileNum.Substring(custMobileNum.Length - 10);
                    }
                }

                JsonSerializerSettings settings = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };

                saveCustomerWatchCustomerWatch.Start();

                var restResponse = await BaseUrl.AppendPathSegment(Constants.Upsert).PostJsonAsync(customer);


                saveCustomerWatchCustomerWatch.Stop();

                if (restResponse.IsSuccessStatusCode)
                {
                    var content = await restResponse.Content.ReadAsStringAsync();

                    saveCustomerResult = JsonConvert.DeserializeObject<SaveCustomerResult>(content, settings);

                    if (saveCustomerResult.Status == Constants.ErrorStatus)
                    {
                        throw new ApplicationException(
                        string.Format("SaveCustomer failed with the following error Error {2},  Error Desc: {0}, Error Code {1}",
                             saveCustomerResult.ErrorDesc,
                             saveCustomerResult.ErrorCode,
                             saveCustomerResult.Error));
                    }
                }
                else
                {
                    saveCustomerResult.Status = Constants.ErrorStatus;
                    saveCustomerResult.Error = restResponse?.ReasonPhrase ?? "Unknown Error";
                    saveCustomerResult.ErrorCode = restResponse != null ? restResponse.StatusCode.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("An exception occurred while trying to save a customer. Error {0}", ex.Message);
                //Logging.Error(errorMessage, ex);

                saveCustomerResult.Status = Constants.ErrorStatus;
                saveCustomerResult.Error = errorMessage;
            }
            finally
            {

                saveCustomerWatchCustomerWatch.Stop();
                saveCustomerWatch.Stop();
            }

            return saveCustomerResult;
        }

        private int GetAge(DateTime birthday)
        {
            int age = DateTime.Now.Year - birthday.Year;
            if (birthday.Date > DateTime.Now)
            {
                age--;
            }

            return age;
        }

        public async Task<CustomerPreferenceUpsertResponse> SaveCustomerEReceiptPreferenceAsync(CustomerPreference customerPreference)
        {
            try
            {
                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment(_settings.RetrieveEReceiptUpsertPath).PostJsonAsync(customerPreference);

                var response = JsonConvert.DeserializeObject<CustomerPreferenceUpsertResponse>(await attrResp.Content.ReadAsStringAsync());
                if (!string.IsNullOrEmpty(response.ErrorMessage)) response.Status = "Error";

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CustomerPreferenceRetrieveResponse> GetCustomerEReceiptPreferenceAsync(string memberId = null, string mobilePhoneNumber = null, string emailAddess = null)
        {
            CustomerPreferenceRetrieveResponse customerPreferenceResponse = new CustomerPreferenceRetrieveResponse();

            try
            {
                if (string.IsNullOrWhiteSpace(memberId) &&
                string.IsNullOrWhiteSpace(mobilePhoneNumber) &&
                string.IsNullOrWhiteSpace(emailAddess))
                    throw new ArgumentException("Provide memberId, mobilePhoneNumber or emailAddress");

                var UrlStringBase = BaseUrl.AppendPathSegment(_settings.RetrieveEReceiptPath).AllowAnyHttpStatus();

                if (!string.IsNullOrWhiteSpace(memberId))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("MEMBER_ID", memberId);
                }
                else if (!string.IsNullOrWhiteSpace(mobilePhoneNumber))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("MOBILE_PHONE", mobilePhoneNumber);
                }
                else if (!string.IsNullOrWhiteSpace(emailAddess))
                {
                    UrlStringBase = UrlStringBase.SetQueryParam("EMAIL_ADDRESS", emailAddess);
                }

                var response = await UrlStringBase.GetAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    CustomerPreference customerPreference = JsonConvert.DeserializeObject<CustomerPreference>(json);
                    customerPreferenceResponse.CustomerPreference = customerPreference;
                    customerPreferenceResponse.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                customerPreferenceResponse.IsSuccessful = false;
                customerPreferenceResponse.ErrorMessage = string.Format("Customer Preference not found with Error Message: {0}", ex.Message);
            }

            return customerPreferenceResponse;
        }

        public async Task<BabyClubRequest> GetBabyClubInfoAsync(string memberId = null)
        {
            BabyClubRequest babyClubInfo = new BabyClubRequest();

            try
            {
                var UrlStringBase = BaseUrl.AppendPathSegment("OMNICHANNEL/BABYCLUB/RETRIEVE").AllowAnyHttpStatus();

                if (!string.IsNullOrEmpty(memberId)) UrlStringBase = UrlStringBase.SetQueryParam("MEMBER_ID", memberId);

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    babyClubInfo = JsonConvert.DeserializeObject<BabyClubRequest>(json);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return babyClubInfo;
        }

        public async Task<BabyClubResponse> SaveBabyClubInfoAsync(BabyClubRequest babyClubInfo)
        {
            try
            {
                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment("OMNICHANNEL/BABYCLUB/UPSERT").PostJsonAsync(babyClubInfo);
                return JsonConvert.DeserializeObject<BabyClubResponse>(await attrResp.Content.ReadAsStringAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<PetClubRequest> GetPetClubInfoAsync(string memberId)
        {
            PetClubRequest petClubRequest = new PetClubRequest();

            try
            {
                var UrlStringBase = BaseUrl.AppendPathSegment("OMNICHANNEL/PETCLUB/RETRIEVE").AllowAnyHttpStatus();

                if (!string.IsNullOrEmpty(memberId)) UrlStringBase = UrlStringBase.SetQueryParam("MEMBER_ID", memberId);

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    petClubRequest = JsonConvert.DeserializeObject<PetClubRequest>(json);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return petClubRequest;
        }

        public async Task<PetClubResponse> SavePetClubInfoAsync(PetClubRequest petClubInfo)
        {
            try
            {
                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment("OMNICHANNEL/PETCLUB/UPSERT").PostJsonAsync(petClubInfo);
                return JsonConvert.DeserializeObject<PetClubResponse>(await attrResp.Content.ReadAsStringAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<PetTypeRequest> GetPetTypeInfoAsync(string petType)
        {
            PetTypeRequest petTypeRequest = new PetTypeRequest();

            try
            {
                var UrlStringBase = BaseUrl.AppendPathSegment("OMNICHANNEL/PETTYPE/RETRIEVE").AllowAnyHttpStatus();

                if (!string.IsNullOrEmpty(petType)) UrlStringBase = UrlStringBase.SetQueryParam("PET_TYPE_NAME", petType);

                var response = await UrlStringBase.GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    petTypeRequest = JsonConvert.DeserializeObject<PetTypeRequest>(json);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return petTypeRequest;
        }

        public async Task<PetTypeResponse> SavePetTypeInfoAsync(PetTypeRequest petTypeInfo)
        {
            try
            {
                var attrResp = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment("OMNICHANNEL/PETTYPE/UPSERT").PostJsonAsync(petTypeInfo);
                return JsonConvert.DeserializeObject<PetTypeResponse>(await attrResp.Content.ReadAsStringAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<PiiRequest> DeletePiiAsync(string memberId)
        {
            try
            {
                var response = await BaseUrl.AppendPathSegment("CUSTODS/DELETEPII").SetQueryParam("MEMBER_ID", memberId).AllowAnyHttpStatus().DeleteAsync();
                var json = await response.Content.ReadAsStringAsync();
                var piiRequest = JsonConvert.DeserializeObject<PiiRequest>(json);
                return piiRequest;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<PrimaryStoreResponse> GetPrimaryStoreAsync(string memberId, int chainId)
        {
            try
            {
                var response = await BaseUrl.AllowAnyHttpStatus().AppendPathSegment("OMNICHANNEL/PRIMARYSTORE/RETRIEVE")
                    .PostJsonAsync(new { memberId = memberId, chainId = chainId });
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PrimaryStoreResponse>(json);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
