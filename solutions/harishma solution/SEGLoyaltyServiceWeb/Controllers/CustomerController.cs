////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	controllers\customercontroller.cs
//
// summary:	Implements the customercontroller class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using SalesForceLibrary.Models;
using SalesForceLibrary.SalesForceAPIM;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Clubs;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Loyalty;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.Request;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.SMS;
using SEG.ApiService.Models.Stores;
using SEG.LoyaltyService.Models.Results;
using SEG.SalesForce;
using SEG.SalesForce.Models;
using SEG.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// 
/// </summary>
namespace SEG.LoyaltyServiceWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerController : Controller
    {
        // private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The log
        SEG.CustomerLibrary.CustomerService customerServiceAzure;
        SEG.CustomerLibrary.InternalCustomerController internalCustomerController;
        SalesForceAPIMService salesForceService;
        private string salt;

        SEG.CustomerLibrary.CustomerQueueProcess customerQueueProcess;

        IConfiguration Configuration;   ///< The configuration
                                        /// <summary>
                                        /// 
                                        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RetryWait { get; set; }
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public CustomerController(IConfiguration configuration)
        {
            Configuration = configuration;
            RetryCount = Convert.ToInt32(Configuration["Settings:SalesForce:RetryCount"]);
            RetryWait = Convert.ToInt32(Configuration["Settings:SalesForce:RetryWait"]);
            salesForceService = new SalesForceAPIMService(Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], Configuration["Settings:SalesForce:SEG_ClientID"], Configuration["Settings:SalesForce:SEG_ClientSecret"], Configuration["Settings:SalesForce:redisConnectionString"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

            internalCustomerController = new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            customerServiceAzure = new SEG.CustomerLibrary.CustomerService(Configuration["Settings:StoreWebAPIEndPoint"], Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:StorageConnectionString"], Configuration["Settings:SalesForce:SalesForceAPIEndPoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            customerQueueProcess = new SEG.CustomerLibrary.CustomerQueueProcess(Configuration["Settings:StorageConnectionString"], Configuration["Settings:AzureLoyaltyApiEndpoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
            salt = Configuration["Settings:EncryptedSalt"].DecryptStringAES2("LoyaltySalt");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (customerServiceAzure != null)
                {
                    customerServiceAzure.Dispose();
                    customerServiceAzure = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        //[HttpDelete]
        //[Route("api/Customer/DeletePii")]
        //public async Task<PiiRequest> DeletePii(string memberId)
        //{
        //    var response = new PiiRequest();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(memberId))
        //        {
        //            //Delete customer from CustODS
        //            response = await internalCustomerController.DeletePii(memberId).ConfigureAwait(false);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in DeletePii :", e);
        //    }

        //    return response;
        //}

        //private async Task<ContactResponse> DeleteSfmcContact(string memberId, ContactResponse contactResponse)
        //{
        //    SalesForceAPIMService salesForceServiceDelete;
        //    var isQA = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "QA";
        //    if (isQA)
        //    {
        //        //Set constructor with SFMC PROD credentials
        //        salesForceServiceDelete = new SalesForceAPIMService(
        //            Configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"],
        //            Configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"],
        //            Configuration["Settings:SalesForce:SEG_ClientID_delete"],
        //            Configuration["Settings:SalesForce:SEG_ClientSecret_delete"],
        //            Configuration["Settings:SalesForce:redisConnectionString"],
        //            Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);

        //        //Delete contact from SFMC QA using PROD credentials
        //        contactResponse = await salesForceServiceDelete.DeletePiiAsync(memberId).ConfigureAwait(false);
        //    }
        //    else
        //    {
        //        //Delete contact from SFMC
        //        contactResponse = await salesForceService.DeletePiiAsync(memberId).ConfigureAwait(false);
        //    }

        //    return contactResponse;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="petClubRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/UpsertPetClub")]
        public async Task<PetClubResponse> UpsertPetClub([FromBody]PetClubRequest petClubRequest)
        {
            List<PetClubChildItem> items = new List<PetClubChildItem>();
            PetClubResponse response = null;

            try
            {
                if (petClubRequest != null)
                {
                    response = await internalCustomerController.CustomerUpsertPetClub(petClubRequest).ConfigureAwait(false);

                    if (response.Status == "Success")
                    {
                        DataExtentionsPetClubChildRequest dataExtentionsPetClubChildRequest = new DataExtentionsPetClubChildRequest();
                        foreach (var i in petClubRequest.PetInfo)
                        {
                            PetClubChildItem item = new PetClubChildItem();
                            if (!string.IsNullOrEmpty(i.EnrollmentBanner))
                                item.EnrollmentBanner = i.EnrollmentBanner;
                            if (i.EnrollmentDate != null)
                                item.EnrollmentDate = i.EnrollmentDate.ToString();
                            if (!string.IsNullOrEmpty(i.EnrollmentSource))
                                item.EnrollementSource = i.EnrollmentSource;
                            if (!string.IsNullOrEmpty(i.LastUpdatedSource))
                                item.LAST_UPDATE_SOURCE = i.LastUpdatedSource;
                            if (i.LastUpdatedDate != null)
                                item.LAST_UPDATE_DT = i.LastUpdatedDate.ToString();
                            if (!string.IsNullOrEmpty(i.PetId))
                                item.PetID = i.PetId;
                            if (!string.IsNullOrEmpty(i.PetName))
                                item.PetName = i.PetName;
                            if (!string.IsNullOrEmpty(i.PetTypeId))
                                item.PetTypeID = i.PetTypeId;
                            if (!string.IsNullOrEmpty(i.PetTypeName))
                                item.PetTypeName = i.PetTypeName;

                            item.member_id = petClubRequest.MemberId;

                            items.Add(item);
                        }

                        dataExtentionsPetClubChildRequest.items = items;

                        var result = await salesForceService.UpsertAsyncPetClub(dataExtentionsPetClubChildRequest, petClubRequest.PetClubFlag, petClubRequest.MemberId, Configuration["Settings:SalesForce:SEG_Key"], Configuration["Settings:SalesForce:SEG_Key_PetClub"]).ConfigureAwait(false);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Exception in UpsertPetClub :", e);
            }
            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="babyClubRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/UpsertBabyClub")]
        public async Task<BabyClubResponse> UpsertBabyClub([FromBody]BabyClubRequest babyClubRequest)
        {
            List<BabyClubChildItem> items = new List<BabyClubChildItem>();
            BabyClubResponse response = null;

            try
            {

                if (babyClubRequest != null)
                {
                    response = await internalCustomerController.CustomerUpsertBabyClub(babyClubRequest).ConfigureAwait(false);

                    if (response.Status == "Success")
                    {
                        DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest = new DataExtentionsBabyClubChildRequest();

                        foreach (var i in babyClubRequest.ChildInfo)
                        {
                            BabyClubChildItem item = new BabyClubChildItem();
                            if (!string.IsNullOrEmpty(i.AgedOutIndicator))
                                item.Aged_Out_Indicator = i.AgedOutIndicator;
                            if (i.BirthDate != null)
                                item.Birth_Date = i.BirthDate.ToString();
                            if (!string.IsNullOrEmpty(i.DeceasedIndicator))
                                item.Deceased_Indicator = i.DeceasedIndicator;
                            if (!string.IsNullOrEmpty(i.ExpectedBabyIndicator))
                                item.Expected_Baby_Indicator = i.ExpectedBabyIndicator;
                            if (!string.IsNullOrEmpty(i.FirstName))
                                item.First_Name = i.FirstName;
                            if (!string.IsNullOrEmpty(i.GenderCode))
                                item.Gender_Code = i.GenderCode;

                            item.Last_Name = i.LastName;

                            item.Member_ID = babyClubRequest.MemberId;
                            //child ID - child number .
                            if (i.ChildId != 0)
                            {
                                item.Member_Child_ID = string.Format("{0}_{1}", babyClubRequest.MemberId, i.ChildId.ToString());
                                item.Child_ID = i.ChildId.ToString();
                            }
                            if (!string.IsNullOrEmpty(i.MiddleInitial))
                                item.Middle_Initial = i.MiddleInitial.ToString();
                            if (!string.IsNullOrEmpty(i.SpecialNeedIndicator))
                                item.Special_Needs_Indicator = i.SpecialNeedIndicator.ToString();

                            items.Add(item);
                        }

                        dataExtentionsBabyClubChildRequest.items = items;

                        var result = await salesForceService.UpsertAsyncBabyClub(dataExtentionsBabyClubChildRequest, babyClubRequest.BabyClubFlag, babyClubRequest.MemberId, Configuration["Settings:SalesForce:SEG_Key"], Configuration["Settings:SalesForce:SEG_Key_BabyClub"]).ConfigureAwait(false);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Exception in UpsertBabyClub :", e);
            }
            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerPreference"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/UpsertEreceiptEnrollment")]
        public async Task<CustomerPreferenceUpsertResponse> UpsertEreceiptEnrollment([FromBody]CustomerPreference customerPreference)
        {
            CustomerPreferenceUpsertResponse response;
            List<Item> list = new List<Item>();
            try
            {
                 response = await internalCustomerController.SaveCustomerEReceiptPreference(customerPreference).ConfigureAwait(false);

                if (response.Status == "Success")
                    {
                    DataExtentionsRequest dataExtentionRequest = new DataExtentionsRequest();
                    Item item = new Item();

                    item.MEMBER_ID = customerPreference.MemberID;
                    if (customerPreference.EReceiptEmailOptInStatus == true)
                    {
                        item.EReceipt_Email_OptIn_Status = "Y";
                    }
                    else{
                        item.EReceipt_Email_OptIn_Status = "N";
                    }
                    
                    if (customerPreference.EReceiptPaperLessOptInStatus == true)
                    {
                        item.EReceipt_Paperless_OptIn_Status = "Y";
                    }
                    else
                    {
                        item.EReceipt_Paperless_OptIn_Status = "N";
                    }

                    list.Add(item);
                    dataExtentionRequest.items = list;
                    DataExtentionsResponse dataExtentionsResponse = await salesForceService.UpsertAsync(dataExtentionRequest, Configuration["Settings:SalesForce:SEG_Key"]).ConfigureAwait(false);
                }
            }

            
            catch (Exception e)
            {
                throw new Exception("Exception in Upsert ERceiptEnrollment :", e);
            }
            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isSavePin"></param>
        /// <param name="isSavePassword"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/Customer/SaveCustomerInAzureEE")]
        //public async Task<IActionResult> SaveCustomerInAzureEE([FromBody]SEG.ApiService.Models.CustomerV2 customer, bool isSavePin = false, bool isSavePassword = false)
        //{

        //    try
        //    {
        //        customer = await GetObjectFromPostDataIfNull<CustomerV2>(customer);

        //        customer.LastUpdateSource = customer.LastUpdateSource ?? "Azure";

        //        if (customer.CustomerAlias != null && customer.CustomerAlias.Count() > 0)
        //        {
        //            foreach (MemberAlias custAlias in customer.CustomerAlias)
        //            {
        //                //If we're saving this from Azure, and it's an Enrollment we can safely assume that this is an active card.. otherwise
        //                //the customer is wasting their time, and Omni has bigger problems by allowing this!
        //                if (custAlias.AliasStatus == null)
        //                    custAlias.AliasStatus = (short)AliasStatusType.Active;
        //            }
        //        }

        //        await customerServiceAzure.SaveCustomerInAzureWebEE(customer, false, isSavePin, isSavePassword);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.ToString());
        //    }
        //    return Ok();

        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="loyaltyNumber"></param>
        ///// <returns></returns>
        //#region CheckCustomerStatus
        //[HttpPost]
        //[Route("api/Customer/CheckCustomerStatus")]
        //[Produces(typeof(SEG.ApiService.Models.Loyalty.ValidationResult))]
        //public async Task<IActionResult> CheckCustomerStatus(string loyaltyNumber)
        //{
        //    try
        //    {
        //        loyaltyNumber = loyaltyNumber?.Trim();

        //        var loyaltyCardResult = ProcessLoyaltyCard(loyaltyNumber);

        //        if (loyaltyCardResult == null) return BadRequest("Invalid Loyalty Number");

        //        switch (loyaltyCardResult.Item2)
        //        {
        //            case "WD":
        //            case "Bilo":
        //            case "Harveys":
        //            case "GnG":
        //            case "Plenti":
        //            case "Phone":
        //                return Ok(await ValidateCustomer(loyaltyCardResult.Item1, loyaltyCardResult.Item2));
        //            default:
        //                return BadRequest();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(e);
        //        return BadRequest(e.ToString());
        //    }

        //}


        //private async Task<ValidationResult> ValidateCustomer(string loyaltyNumber, string numberType)
        //{
        //    try
        //    {
        //        var csr = new CustomerSearchRequest();
        //        AliasRequest gadRequest = null;
        //        ValidationResult validationResult = new ValidationResult
        //        {
        //            LoyaltyCard = loyaltyNumber,
        //            LoyaltyCardType = numberType,
        //            EnrollmentStatus = "N",
        //            swapped = false

        //        };


        //        switch (numberType)
        //        {
        //            case "WD":
        //                csr.CrcId = loyaltyNumber;
        //                validationResult.Banner = Banner.WD;
        //                break;
        //            case "Bilo":
        //                csr.CrcId = loyaltyNumber;
        //                validationResult.Banner = Banner.Bilo;
        //                break;
        //            case "Harveys":
        //                csr.CrcId = loyaltyNumber;
        //                validationResult.Banner = Banner.Harveys;
        //                break;
        //            case "GnG":
        //                csr.OmniId = loyaltyNumber;
        //                break;
        //            case "Plenti":
        //                csr.OmniId = loyaltyNumber;
        //                gadRequest = new AliasRequest
        //                {
        //                    AliasType = AliasType.PlentiCardNumber,
        //                    Alias = loyaltyNumber
        //                };
        //                break;
        //            case "Phone":
        //                csr.MobilePhoneNumber = loyaltyNumber;
        //                gadRequest = new AliasRequest
        //                {
        //                    AliasType = AliasType.PhoneNumber,
        //                    Alias = loyaltyNumber
        //                };
        //                break;
        //            default:
        //                return null;

        //        }

        //        var icc = new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
        //        CustomerSearchResponse results = await icc.GetCustomerRecord(csr).ConfigureAwait(false);


        //        if (results != null && results.IsSuccessful && results.Customers.Any())
        //        {
        //            validationResult.EnrollmentStatus = results.Customers.First().EnrollmentStatus ?? "P"; //Default Customer to Pre-Enrolled if Null
        //            validationResult.MEMBER_ID = results.Customers.FirstOrDefault().MemberId;

        //            if (results.Customers.First().CustomerAlias != null && results.Customers.First().CustomerAlias.Count > 0)
        //            {
        //                if (results.Customers.First().CustomerAlias.Any(a => a.AliasType == 2))
        //                {
        //                    validationResult.swapped = true;
        //                }
        //            }
        //        }


        //        string[] validEnrollmentStatus = new string[] { "P", "N", "E" };


        //        if (!validEnrollmentStatus.Contains(validationResult.EnrollmentStatus))  //Any other result should default to Pre-Enrolled
        //            validationResult.EnrollmentStatus = "P";

        //        return validationResult;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in ValidateCustomer :", e);
        //    }
        //}

        //private Tuple<string, string> ProcessLoyaltyCard(string loyaltyNumber)
        //{
        //    try
        //    {
        //        string cardType = string.Empty;

        //        if (string.IsNullOrWhiteSpace(loyaltyNumber)) throw new ArgumentException("Invalid Loyalty Number");

        //        loyaltyNumber = loyaltyNumber.Trim();

        //        decimal.TryParse(loyaltyNumber, out decimal cardNumber);

        //        if (loyaltyNumber.Length == 12)
        //        {
        //            loyaltyNumber = loyaltyNumber.Substring(0, 11);
        //            decimal.TryParse(loyaltyNumber, out cardNumber);
        //        }
        //        else if (loyaltyNumber.StartsWith("9800"))
        //        {
        //            if (cardNumber >= 980000000000000)
        //                cardNumber = cardNumber - 980000000000000;

        //        }
        //        else if (loyaltyNumber.Length == 10)
        //        {
        //            var regMatch = @"^([0-9]( |-)?)?(\(?[0-9]{3}\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})$";
        //            var reg = new Regex(regMatch);
        //            var match = reg.Match(loyaltyNumber);
        //            if (match != null && match.Success)
        //            {
        //                cardType = "Phone";


        //            }

        //            decimal.TryParse(loyaltyNumber, out cardNumber);

        //        }
        //        else if (loyaltyNumber.Length < 16)
        //        {
        //            loyaltyNumber = loyaltyNumber.Substring(0, 11);
        //            decimal.TryParse(loyaltyNumber, out cardNumber);
        //        }

        //        else if (loyaltyNumber.StartsWith("3104"))
        //            cardType = "Plenti";
        //        else if (loyaltyNumber.StartsWith("72211"))
        //            cardType = "GnG";



        //        if ((cardNumber < 42099999999 && cardNumber > 42065000000) ||
        //            (cardNumber < 48199999999 && cardNumber > 48100000000))
        //        {
        //            cardType = Banner.WD.ToString();
        //        }
        //        else if ((cardNumber < 44189999999 && cardNumber > 44000000000) ||
        //                 (cardNumber < 44999999999 && cardNumber > 44200000000) ||
        //                 (cardNumber < 48299999999 && cardNumber > 48200000000) ||
        //                 (cardNumber < 44199999999 && cardNumber > 44190000000))
        //        {
        //            cardType = Banner.Bilo.ToString();
        //        }
        //        else if (
        //             (cardNumber < 48399999999 && cardNumber > 48300000000) ||
        //             (cardNumber < 44189999999 && cardNumber > 44000000000) ||
        //             (cardNumber < 44999999999 && cardNumber > 44200000000))
        //        {
        //            cardType = Banner.Harveys.ToString();
        //        }

        //        var formatControl = new System.Globalization.NumberFormatInfo
        //        {
        //            NumberDecimalDigits = 0
        //        };
        //        if (string.IsNullOrEmpty(cardType)) return null;
        //        return new Tuple<string, string>(cardNumber.ToString(formatControl), cardType);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}
        //#endregion





        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="customerCheckPassword"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Customer/CheckPassword")]
        //public bool CheckPassword([FromBody] CustomerCheckPassword customerCheckPassword)
        //{
        //    bool checkPassword = false;
        //    try
        //    {
        //        if (customerCheckPassword != null && !string.IsNullOrEmpty(customerCheckPassword.CurrentPassword) && !string.IsNullOrEmpty(customerCheckPassword.EncryptedPassword))
        //        {
        //            var password = $"{customerCheckPassword.CurrentPassword}{salt}".GenerateHashString(HashingExtensions.HashType.SHA256);

        //            if (customerCheckPassword.EncryptedPassword.Equals(password))
        //            {
        //                checkPassword = true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in CheckPassword :", e);
        //    }
        //    return checkPassword;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="isQueueRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Customer/SaveCustomerInAzureQueueProcess")]
        //public async Task SaveCustomerInAzureQueueProcess([FromBody]CustomerV2 customer, bool isQueueRequest)
        //{
        //    try
        //    {
        //        await customerQueueProcess.SaveCustomerInAzure(customer, isQueueRequest).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(e);
        //        throw new Exception("Exception in SaveCustomerInAzureQueueProcess :", e);
        //    }
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) gets customer by external identifier.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="provider"> The provider. </param>
        /// <param name="userid">   The userid. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by external identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetCustomerByExternalId")]
        //public async Task<AzureCustomer> GetCustomerByExternalId(string provider, string userid)
        //{
        //    return await customerServiceAzure.GetCustomerByExternalId(provider, userid).ConfigureAwait(false);
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) gets customer by member identifier.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="MemberId"> . </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetCustomerByMemberId")]
        //public async Task<AzureCustomer> GetCustomerByMemberId(string MemberId)
        //{

        //    var azureCustomer = await customerServiceAzure.GetCustomerByMemberId(MemberId).ConfigureAwait(false);
        //    //reduntant code 
        //    //if (azureCustomer == null)
        //    //{
        //    //    var customerSearchResults = await new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"]).CustomerSearch(new CustomerSearchRequest() { MemberId = MemberId }).ConfigureAwait(false);
        //    //    if (customerSearchResults != null && customerSearchResults.IsSuccessful && customerSearchResults.Customers.Any())
        //    //    {
        //    //        customerSearchResults.Customers.ForEach(async cust =>
        //    //        {
        //    //            await EnrollMissingCustomerDetails(MemberId, customerServiceAzure, customerSearchResults).ConfigureAwait(false);
        //    //        });

        //    //        azureCustomer = await customerServiceAzure.GetCustomerByMemberId(MemberId).ConfigureAwait(false);
        //    //    }
        //    //}
        //    return azureCustomer;

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) links an external login. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="userid">   The userid. </param>
        ///
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/LinkExternalLogin")]
        //public async Task<IActionResult> LinkExternalLogin(string memberId, string provider, string userid)
        //{
        //    await customerServiceAzure.LinkExternalLogin(memberId, provider, userid).ConfigureAwait(false);
        //    return Ok();
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get customer by DeviceId. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="deviceId"> . </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        ///
        /// <returns>   CustomerAzure. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetcustomerByDeviceId")]
        //public async Task<AzureCustomerDetail> GetCustomerByDeviceId(string deviceId, string chainId)
        //{
        //    AzureCustomerDetail azureCustomer = null;
        //    string memberId = await customerServiceAzure.GetMemberIdByDeviceId(deviceId).ConfigureAwait(false);
        //    if (!string.IsNullOrEmpty(memberId))
        //    {
        //        //optimised this call as this has major load on the service. 
        //        azureCustomer = await customerServiceAzure.GetCustomerByMemberIdAndChainIdOptimizedForDeviceID(memberId, chainId).ConfigureAwait(false);
        //        //if (azureCustomer == null)
        //        //{
        //        //    var customerSearchResults = await new CustomerLibrary.InternalCustomerController(Configuration["Settings:LoyaltyEndPoint"]).CustomerSearch(new CustomerSearchRequest() { MemberId = memberId }).ConfigureAwait(false);

        //        //    if (customerSearchResults != null && customerSearchResults.IsSuccessful)
        //        //    {
        //        //        azureCustomer = await EnrollMissingCustomerDetails(memberId, chainId, customerServiceAzure, customerSearchResults).ConfigureAwait(false);
        //        //    }
        //        //}
        //    }
        //    return azureCustomer;

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Enroll missing customer details. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="MemberId">                 . </param>
        /// <param name="chainId">                  Identifier for the chain. </param>
        /// <param name="customerServiceAzure">     The customer service azure. </param>
        /// <param name="customerSearchResults">    The customer search results. </param>
        ///
        /// <returns>   An asynchronous result that yields an AzureCustomerDetail. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //private async Task<AzureCustomerDetail> EnrollMissingCustomerDetails(string MemberId, string chainId, CustomerLibrary.CustomerService customerServiceAzure, CustomerSearchResponse customerSearchResults)
        //{
        //    AzureCustomerDetail azureCustomer = null;


        //    customerSearchResults.Customers.ForEach(async customer =>
        //    {
        //        azureCustomer = await UpdateProcessing(customer, azureCustomer);
        //    });


        //    azureCustomer = await customerServiceAzure.GetCustomerByMemberIdAndChainId(MemberId, chainId).ConfigureAwait(false);
        //    return azureCustomer;
        //}

        //private async Task<AzureCustomerDetail> UpdateProcessing(CustomerV2 customer, AzureCustomerDetail azureCustomer)
        //{
        //    try
        //    {
        //        var crcId = customer.CrcId;
        //        if (customer.ChainId == "1") crcId = "9800" + crcId;
        //        azureCustomer = new AzureCustomerDetail()
        //        {
        //            ChainId = customer.ChainId,
        //            CouponAlias = crcId,
        //            CreatedDate = DateTime.Now,
        //            FirstName = customer.FirstName,
        //            LastName = customer.LastName,
        //            EmailAddress = customer.EmailAddress,
        //            MemberId = customer.MemberId,
        //            MobilePhoneNumber = customer.MobilePhoneNumber,
        //            StoreId = customer.EnrollmentLocationId
        //        };
        //        await AzureLoyaltyDatabaseManager.SaveCustomer(azureCustomer);
        //        Shared.Utility utility = new Shared.Utility();
        //        var appCode = utility.SetAppCode(customer.ChainId);
        //        //coupon updatecustomer updates
        //        OfferQueueRequest offerQueueRequest = new OfferQueueRequest
        //        {
        //            emailAddress = customer.EmailAddress,
        //            transactionID = customer.EmailAddress,
        //            appCode = appCode,
        //            userDetail = new ApiService.Models.Offers.UserDetail
        //            {
        //                CRCNumber = azureCustomer.CouponAlias,
        //                LoyaltyNumber = azureCustomer.CouponAlias,
        //                Email = customer.EmailAddress,
        //                FirstName = customer.FirstName,
        //                LastName = customer.LastName,
        //                PhoneNumber = customer.MobilePhoneNumber,
        //                Address = new ApiService.Models.Offers.Address()
        //            },
        //            chainId = customer.ChainId,
        //            memberId = customer.MemberId
        //        };
        //        if (customer.CustomerAddress != null && customer.CustomerAddress.Any())
        //        {
        //            offerQueueRequest.userDetail.Address.Address1 = customer.CustomerAddress[0].AddressLine1;
        //            offerQueueRequest.userDetail.Address.Address2 = customer.CustomerAddress[0].AddressLine2;
        //            offerQueueRequest.userDetail.Address.City = customer.CustomerAddress[0].City;
        //            offerQueueRequest.userDetail.Address.State = customer.CustomerAddress[0].State;
        //            offerQueueRequest.userDetail.Address.Zip = customer.CustomerAddress[0].PostalCode;
        //        }
        //        var offerService = new OfferService(Configuration["Settings:StorageConnectionString"]);
        //        offerService.InsertOfferQueue(offerQueueRequest, true);

        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(e);
        //    }

        //    return azureCustomer;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get customer by Email. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="id">   . </param>
        ///
        /// <returns>   CustomerAzure. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpGet]
        //[Route("api/Customer/GetCustomersByEmailOrPhone")]
        //public async Task<List<AzureCustomer>> GetCustomersByEmailOrPhone(string id)
        //{
        //    List<AzureCustomer> azureCustomer = null;
        //    try
        //    {

        //        azureCustomer = await customerServiceAzure.GetCustomerByEmailOrPhone(id).ConfigureAwait(false);

        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception("Exception in GetCustomersByEmailOrPhone :", e);
        //    }
        //    return azureCustomer;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get customer by Email. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">    . </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        ///
        /// <returns>   CustomerAzure. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetCustomerByEmailAndChainId")]
        //public async Task<AzureCustomerDetail> GetCustomerByEmailAndChainId(string email, string chainId)
        //{
        //    AzureCustomerDetail azureCustomerDetail = null;
        //    try
        //    {
        //        azureCustomerDetail = await customerServiceAzure.GetCustomerByEmailAndChainIdAsync(email, chainId);
        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception("Exception in GetCustomerByEmailAndChainId :", e);

        //    }
        //    return azureCustomerDetail;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) generates a barcode. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="value">    The value. </param>
        /// <param name="size">     The size. </param>
        ///
        /// <returns>   An asynchronous result that yields the barcode. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //public async Task<BarcodeResponse> GenerateBarcode(string value, Shared.Barcodes.BarcodeSize size)
        //{
        //    try
        //    {
        //        return await Task.Run(() =>
        //        {

        //            var barcode = new BarcodeResponse()
        //            {
        //                BarcodeValue = value,
        //                ImageBase64 = Shared.BarcodeGenerator.GenerateBarcodeBase64(value, ZXing.BarcodeFormat.CODE_128, size)
        //            };
        //            barcode.Size = (int)size;
        //            return barcode;
        //        }).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception("Exception in GenerateBarcode :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="MemberId"></param>
        ///// <param name="ChainId"></param>
        ///// <param name="StoreId"></param>
        ///// <param name="lastUpdateSource"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Customer/UpdateStoreInAzure")]
        //public async Task<IActionResult> UpdateStoreInAzure(string MemberId, string ChainId, int StoreId, string lastUpdateSource)
        //{

        //    try
        //    {
        //        StoreUpdateResult storeUpdateResult = await customerServiceAzure.UpdateStoreInAzure(MemberId, ChainId, StoreId, lastUpdateSource).ConfigureAwait(false);

        //        await SFMCSelfSelectedStoreUpdate(storeUpdateResult).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(String.Format("An error in UpdateStoreInAzure .  Error {0}", e.Message), e);
        //        return BadRequest(e.ToString());
        //    }
        //    return Ok();

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="ChainId"></param>
        /// <param name="StoreId"></param>
        /// <param name="lastUpdateSource"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/UpdateStoreInAzureEE")]
        public async Task<IActionResult> UpdateStoreInAzureEE(string MemberId, string ChainId, int StoreId, string lastUpdateSource)
        {
            StoreUpdateResult storeUpdateResult = null;
            try
            {
                storeUpdateResult = await customerServiceAzure.UpdateStoreInAzureEE(MemberId, ChainId, StoreId, lastUpdateSource).ConfigureAwait(false);

                await SFMCSelfSelectedStoreUpdate(storeUpdateResult).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                //log.Error(String.Format("An error in UpdateStoreInAzure .  Error {0}", e.Message), e);
                return BadRequest(e.ToString());
            }
            return Ok(storeUpdateResult);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) updates my store described by
        /// updateMyStoreRequest.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="updateMyStoreRequest"> . </param>
        ///
        /// <returns>   An asynchronous result that yields a StoreUpdateResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/UpdateMyStore")]
        //public async Task<StoreUpdateResult> UpdateMyStore([FromBody]UpdateMyStoreRequest updateMyStoreRequest)
        //{
        //    StoreUpdateResult storeUpdateResult = null;
        //    try
        //    {

        //        storeUpdateResult = await customerServiceAzure.UpdateMyStore(updateMyStoreRequest.DeviceId, updateMyStoreRequest.TransactionID, updateMyStoreRequest.AppId, updateMyStoreRequest.AppVer, updateMyStoreRequest.EmailAddress, updateMyStoreRequest.StoreID).ConfigureAwait(false);

        //        await SFMCSelfSelectedStoreUpdate(storeUpdateResult).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in UpdateMyStore :", e);
        //    }

        //    return storeUpdateResult;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="smsWebRequest"></param>        
        ///// 
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Customer/SendAuthCodebyPhoneNumber")]
        //public async Task<bool> SendAuthCodebyPhoneNumber([FromBody]SMSWebRequest smsWebRequest)
        //{
        //    bool result = false;

        //    try
        //    {
        //        var _authCode = smsWebRequest.AuthCode.DecryptStringAES(smsWebRequest.PhoneNumber, salt);
        //        var response = await customerServiceAzure.SendSMSAuthCode(smsWebRequest.PhoneNumber, _authCode, smsWebRequest.ChainId).ConfigureAwait(false);
        //        if (response.Equals("true"))
        //        {
        //            result = true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ////log.Error(e);                
        //        throw new Exception("Exception in SendAuthCodebyPhoneNumber :", e);
        //    }
        //    return result;
        //}


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Method to SEND SMS or EMAIL to customer .... </summary>
        ///
        /// <remarks>  Sam Nanduri, 3/5/2018.   A call to this method, can contain an authcode , or a true for forceResetAuthCode 
        /// which will reset the Customer Attribute of AuthorizationCode in the loyalty Database
        /// If the authcode is blank, it will automatically do a forceResetCode 
        /// You can either send a SMS or Email or both.Make sure the customer object has a CRCId, banner, email / Ph#
        /// in their information </remarks>
        ///
        /// <param name="memberId">  The Customer MemberId. </param>
        /// <param name="appCode">  The appCode or ChainId </param>
        /// <param name="resetPassword">  A bool value of true/false if we want to append aliasNumber & MemberID as a Base64String to any URL's for any TemplateTypes. ( Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(aliasNumber + "|" + customer.MemberId)); ) </param>
        /// <param name="sendSMS">  A bool value of true/false if we are send a authorization code using SMS </param>
        /// <param name="sendEmail">  A bool value of true/false if we are send a authorization code using SendGrid Email </param>
        /// <param name="templateType">  A string value specifying the template "AuthorizeCode" and if sending a password reset, it's "ResetPasswordAndCode" </param>
        ///   
        /// 
        /// <returns>   An asynchronous CustomerResponse object </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/SendAuthorizationCodeToCustomer")]
        //[Produces(typeof(CustomerResponse))]
        //public async Task<CustomerResponse> SendAuthorizationCodeToCustomer([FromBody] CustomerSearchRequest searchRequest,
        //                                                                    string appCode,
        //                                                            bool resetPassword,
        //                                                            bool sendSMS,
        //                                                            bool sendEmail,
        //                                                            string templateType,
        //                                                            string source,
        //                                                            string authCode)
        //{
        //    try
        //    {
        //        return await customerServiceAzure.SendAuthorizationCodeToCustomer(searchRequest, appCode, resetPassword, sendSMS, sendEmail, templateType, source, authCode);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}







        // <summary>

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// generate guid
        /// </summary>
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="generateGuidRequest">  . </param>
        ///
        /// <returns>   IActionResult. </returns>
        ///
        /// ### <param name="appId">    . </param>
        ///
        /// ### <param name="appVer">       . </param>
        /// ### <param name="macAddress">   . </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GenerateGuid")]
        //public async Task<string> GenerateGuid([FromBody]GenerateGuidRequest generateGuidRequest)
        //{
        //    try
        //    {
        //        return await customerServiceAzure.GenerateGuid(generateGuidRequest.TransactionID, generateGuidRequest.AppId, generateGuidRequest.AppVer, generateGuidRequest.MacAddress);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in GenerateGuid :", e);
        //    }
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) initializes the application.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="initAppRequest">   . </param>
        ///
        /// <returns>   An asynchronous result that yields a string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/InitApp")]
        //public async Task<string> InitApp([FromBody]InitAppRequest initAppRequest)
        //{
        //    try
        //    {
        //        return await customerServiceAzure.InitApp(initAppRequest.TransactionID, initAppRequest.AppId, initAppRequest.AppVer, initAppRequest.MacAddress, initAppRequest.DeviceId);
        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception("Exception in InitApp :", e);
        //    }

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) gets user detail. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="HttpRequestException"> Thrown when a HTTP Request error condition occurs. </exception>
        ///
        /// <param name="userDetailRequest">    . </param>
        ///
        /// <returns>   An asynchronous result that yields the user detail. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetUserDetail")]
        //public async Task<ApiService.Models.Mobile.UserDetail> GetUserDetail([FromBody]UserDetailRequest userDetailRequest)
        //{
        //    try
        //    {


        //        if (userDetailRequest == null || string.IsNullOrEmpty(userDetailRequest.DeviceId)) return null;
        //        return await customerServiceAzure.GetUserDetail(userDetailRequest.TransactionID, userDetailRequest.AppCode, userDetailRequest.AppVer, userDetailRequest.DeviceId, userDetailRequest.TokenId).ConfigureAwait(false);

        //    }
        //    catch (Exception e)
        //    {
        //        log.Fatal("Error Running GetUserDetail", e);
        //        throw new HttpRequestException(e.Message, e);
        //    }

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) saves a web attributes. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer"> Customer Object. </param>
        ///
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/SaveWebAttributes")]
        //public async Task<IActionResult> SaveWebAttributes([FromBody]Customer customer)
        //{

        //    try
        //    {
        //        await customerServiceAzure.SaveWebAttributes(customer).ConfigureAwait(false);

        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(e);
        //        return BadRequest(e.ToString());
        //    }



        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) saves an email preferences.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="saveCustomerEmailPrefRequest"> The save customer email preference request. </param>
        ///
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/SaveEmailPreferences")]
        //public async Task<IActionResult> SaveEmailPreferences([FromBody]SaveCustomerEmailPrefRequest saveCustomerEmailPrefRequest)
        //{

        //    try
        //    {

        //        await customerServiceAzure.SaveEmailPreferences(saveCustomerEmailPrefRequest.customer, saveCustomerEmailPrefRequest.customerEmailPreferenceList).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        //log.Error(e);
        //        return BadRequest(e.ToString());
        //    }
        //    return Ok();

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) gets email preferences. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerEmailPrefRequest"> The customer email preference request. </param>
        ///
        /// <returns>   An asynchronous result that yields the email preferences. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/GetEmailPreferences")]
        //public async Task<CustomerEmailPreferenceList> GetEmailPreferences([FromBody]CustomerEmailPrefRequest customerEmailPrefRequest)
        //{

        //    return await customerServiceAzure.GetEmailPreferences(customerEmailPrefRequest).ConfigureAwait(false);

        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) saves a pin. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="credentialRequest">    The credential request. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/SavePIN")]
        //public async Task<bool> SavePIN([FromBody]CredentialRequest credentialRequest)
        //{

        //    bool isSuccessfull = await customerServiceAzure.SavePIN(credentialRequest).ConfigureAwait(false);
        //    return isSuccessfull;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) saves a password. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="credentialRequest">    The credential request. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpPost]
        //[Route("api/Customer/SavePassword")]
        //public async Task<bool> SavePassword([FromBody]CredentialRequest credentialRequest)
        //{

        //    bool isSuccessfull = await customerServiceAzure.SavePassword(credentialRequest).ConfigureAwait(false);
        //    return isSuccessfull;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// (An Action that handles HTTP POST requests) validates the password described by
        /// credentialRequest.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <exception cref="HttpResponseException">    Thrown when a HTTP Response error condition
        ///                                             occurs. </exception>
        ///
        /// <param name="credentialRequest">    The credential request. </param>
        ///
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////[HttpPost]
        //////////[Route("api/Customer/ValidatePasswordEE")]
        //////////[Produces(typeof(ApiResponse<CustomerV2>))]
        //////////[ProducesResponseType(401)]
        //////////[ProducesResponseType(200)]
        //////////[ProducesResponseType(201)]
        //////////public async Task<IActionResult> ValidatePasswordEE([FromBody]CredentialRequest credentialRequest)
        //////////{
        //////////    var response = new ApiResponse<CustomerV2>();
        //////////    int statusCode = 200;
        //////////    try
        //////////    {
        //////////        string chainId = new Utility().SetChainId(string.IsNullOrEmpty(credentialRequest.AppCode) ? "00" : credentialRequest.AppCode);

        //////////        CredRetrieveResultV2 credRetrieveResultV2 = await customerServiceAzure.ValidatePasswordV2(credentialRequest).ConfigureAwait(false);

        //////////        if (credRetrieveResultV2 == null)
        //////////        {
        //////////            Exception ex = new Exception("Internal Loyalty service error. Can not retrieve credentials for the user.");
        //////////            ex.Data.Add("code", "5001");
        //////////            throw ex;
        //////////        }
        //////////        if (String.IsNullOrEmpty(credRetrieveResultV2.status) || string.IsNullOrEmpty(credRetrieveResultV2.customer.MemberId) || credRetrieveResultV2.customer == null)
        //////////        {
        //////////            Exception ex = new Exception("User not found");
        //////////            ex.Data.Add("code", "5005");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.customer.EnrollmentStatus == null || credRetrieveResultV2.customer.EnrollmentStatus == "C")
        //////////        {
        //////////            Exception ex = new Exception("User is Cancelled");
        //////////            ex.Data.Add("code", "5007");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.status.ToLower().Equals("unauthenticated"))
        //////////        {
        //////////            UnauthorizedAccessException ex = new UnauthorizedAccessException("Authentication failed for the user");
        //////////            ex.Data.Add("code", "5002");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.status.ToLower().Equals("accountlockedout"))
        //////////        {
        //////////            Exception ex = new Exception("User Locked out");
        //////////            ex.Data.Add("code", "5003");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.status.ToLower().Equals("passwordnotset"))
        //////////        {
        //////////            Exception ex = new Exception("Password not set for the user");
        //////////            ex.Data.Add("code", "5006");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.status.ToLower().Equals("error") && credRetrieveResultV2.error_Description.Equals("User not found"))
        //////////        {
        //////////            Exception ex = new Exception("User not found");
        //////////            ex.Data.Add("code", "5005");
        //////////            throw ex;
        //////////        }
        //////////        else if (credRetrieveResultV2.error_Description != null)
        //////////        {
        //////////            Exception ex = new Exception(credRetrieveResultV2.error_Description);
        //////////            ex.Data.Add("code", "5004");
        //////////            throw ex;
        //////////        }
        //////////        else
        //////////        {
        //////////            if (credRetrieveResultV2.customer == null)
        //////////            {
        //////////                Exception ex = new Exception("User not found");
        //////////                ex.Data.Add("code", "5005");
        //////////                throw ex;
        //////////            }
        //////////            response.Object = credRetrieveResultV2.customer;
        //////////        }

        //////////    }
        //////////    catch (UnauthorizedAccessException e)
        //////////    {
        //////////        response.FaultMessage = new FaultDescriptor
        //////////        {
        //////////            Message = e.Message,
        //////////            Code = (e.Data["code"] != null) ? e.Data["code"].ToString() : "5000",
        //////////            RequestID = Guid.NewGuid().ToString()
        //////////        };

        //////////        statusCode = 401;
        //////////    }
        //////////    catch (Exception e)
        //////////    {
        //////////        //updated to 403 forbidden error . 
        //////////        response.FaultMessage = new FaultDescriptor
        //////////        {
        //////////            Message = e.Message,
        //////////            Code = (e.Data["code"] != null) ? e.Data["code"].ToString() : "5000",
        //////////            RequestID = Guid.NewGuid().ToString()
        //////////        };

        //////////        statusCode = 403;
        //////////    }
        //////////    return StatusCode(statusCode, response);

        //////////}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerPreference"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/SavePreference")]
        public async Task<CustomerPreferenceUpsertResponse> SaveCustomerPreference([FromBody] CustomerPreference customerPreference)
        {
            try
            {
                CustomerPreferenceUpsertResponse response = await internalCustomerController.SaveCustomerPreference(customerPreference).ConfigureAwait(false);

                if (response != null)
                {

                    #region Mapping
                    DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest = new DataExtentionsCustomerPreferenceRequest();
                    dataExtentionsCustomerPreferenceRequest.items = new List<CustomerPreferenceItem>()
                    {
                        new CustomerPreferenceItem()
                        {
                            MemberID = customerPreference.MemberID,
                            Banner = customerPreference.Banner,
                            InterestDairyFree =  string.IsNullOrEmpty(customerPreference.InterestDairyFree.ToString()) ? null : customerPreference.InterestDairyFree.ToString(),
                            InterestGlutenFree = string.IsNullOrEmpty(customerPreference.InterestGlutenFree.ToString()) ? null : customerPreference.InterestGlutenFree.ToString(),
                            InterestGrilling =string.IsNullOrEmpty(customerPreference.InterestGrilling.ToString()) ? null : customerPreference.InterestGrilling.ToString(),
                            InterestGroceryDelivery = string.IsNullOrEmpty(customerPreference.InterestGroceryDelivery.ToString()) ? null : customerPreference.InterestGroceryDelivery.ToString(),
                            InterestHappyHour = string.IsNullOrEmpty(customerPreference.InterestHappyHour.ToString()) ? null : customerPreference.InterestHappyHour.ToString(),
                            InterestKidsBaby = string.IsNullOrEmpty(customerPreference.InterestKidsBaby.ToString()) ? null : customerPreference.InterestKidsBaby.ToString(),
                            InterestLowCarb = string.IsNullOrEmpty(customerPreference.InterestLowCarb.ToString()) ? null : customerPreference.InterestLowCarb.ToString(),
                            InterestOrganic = string.IsNullOrEmpty(customerPreference.InterestOrganic.ToString()) ? null : customerPreference.InterestOrganic.ToString(),
                            InterestPets = string.IsNullOrEmpty(customerPreference.InterestPets.ToString()) ? null : customerPreference.InterestPets.ToString(),
                            InterestRecipes = string.IsNullOrEmpty(customerPreference.InterestRecipes.ToString()) ? null : customerPreference.InterestRecipes.ToString(),
                            InterestSavings = string.IsNullOrEmpty(customerPreference.InterestSavings.ToString()) ? null : customerPreference.InterestSavings.ToString(),
                            InterestVegan = string.IsNullOrEmpty(customerPreference.InterestVegan.ToString()) ? null : customerPreference.InterestVegan.ToString(),
                            InterestVegetarian = string.IsNullOrEmpty(customerPreference.InterestVegetarian.ToString()) ? null : customerPreference.InterestVegetarian.ToString(),
                            EmailBirthdays = string.IsNullOrEmpty(customerPreference.EmailBirthdays.ToString()) ? null : customerPreference.EmailBirthdays.ToString(),
                            EmailSurveys = string.IsNullOrEmpty(customerPreference.EmailSurveys.ToString()) ? null : customerPreference.EmailSurveys.ToString(),
                            EmailWeekendSale = string.IsNullOrEmpty(customerPreference.EmailWeekendSale.ToString()) ? null : customerPreference.EmailWeekendSale.ToString(),
                            EmailWeeklyAds = string.IsNullOrEmpty(customerPreference.EmailWeeklyAds.ToString()) ? null : customerPreference.EmailWeeklyAds.ToString(),
                            OnlineShoppingVendor = customerPreference.OnlineShoppingVendor,
                            DisableBabyClubMessage = string.IsNullOrEmpty(customerPreference.DisableBabyClubMessage.ToString()) ? null : customerPreference.DisableBabyClubMessage.ToString(),
                        }
                    };
                    #endregion

                    DataExtentionsResponse dataExtentionsResponsePreference = await salesForceService.UpsertAsyncPreferences(dataExtentionsCustomerPreferenceRequest, Configuration["Settings:SalesForce:SEG_Key_CustomerPreferences"]).ConfigureAwait(false);

                    if (dataExtentionsResponsePreference == null || String.IsNullOrEmpty(dataExtentionsResponsePreference.requestId) || !string.IsNullOrEmpty(dataExtentionsResponsePreference.errorcode))
                        response.Status = "Error in Customer Preference SalesForce Upsert: " + dataExtentionsResponsePreference.message;

                    var customer = new CustomerV2
                    {
                        MemberId = customerPreference.MemberID,
                        EmailOptOutStatus = IsEmailOptOut(customerPreference.EmailBirthdays, customerPreference.EmailSurveys, customerPreference.EmailWeekendSale, customerPreference.EmailWeeklyAds)
                    };

                    var customerSaveResponse = await internalCustomerController.CustomerSave(customer);
                    if (!customerSaveResponse.IsSuccessful)
                    {
                        var errors = $"Error in Customer EmailOptOutStatus Save: { string.Join(",", customerSaveResponse.ErrorMessages) }";
                        if (string.IsNullOrEmpty(response.Status))
                        {
                            response.Status = errors;
                        }
                        else
                        {
                            response.Status += $"\n{ errors }";
                        }
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in SaveCustomerPreference :", e);
            }
        }

        private bool IsEmailOptOut(bool? emailBirthdays, bool? emailSurveys, bool? emailWeekendSale, bool? emailWeeklyAds)
        {
            return !(emailBirthdays.HasValue && (bool)emailBirthdays)
                    && !(emailSurveys.HasValue && (bool)emailSurveys)
                    && !(emailWeekendSale.HasValue && (bool)emailWeekendSale)
                    && !(emailWeeklyAds.HasValue && (bool)emailWeeklyAds);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="chainId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/GetPreference")]
        public async Task<CustomerPreferenceRetrieveResponse> GetPreference(string memberId, string chainId)
        {
            try
            {
                return await internalCustomerController.GetPreference(memberId, chainId);
            }
            catch (Exception e)
            {
                throw new Exception("Exception in GetCustomerPreference :", e);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a customer attribute. </summary>
        ///
        /// <remarks>   guptaj, 3/06/2018. </remarks>
        ///
        /// <param name="memberid">        (Optional) </param>
        /// 
        /// <param name="crcId">        (Optional) </param>
        /// 
        /// <param name="attributeId">  (Optional) Identifier for the attribute. </param>
        ///
        /// <returns>    An asynchronous result that yields a List of CustAttributeRetrieve. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpGet]
        //[Route("api/Customer/GetCustomerAttributes")]
        //public async Task<List<CustAttributeRetrieve>> GetCustomerAttributes(string memberid = null, string crcId = null, string attributeId = null)
        //{
        //    return await customerServiceAzure.GetCustomerAttributes(memberid, crcId, attributeId).ConfigureAwait(false);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        protected internal async Task<T> GetObjectFromPostDataIfNull<T>(T requestObject)
        {
            try
            {
                if (requestObject == null)
                {
                    var json = await GetPostDataFromStream().ConfigureAwait(false);
                    Newtonsoft.Json.JsonSerializerSettings settings = new JsonSerializerSettings();

                    requestObject = JsonConvert.DeserializeObject<T>(json, settings);

                }
                return requestObject;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets post data from stream. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <returns>   The post data from stream. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected internal async Task<string> GetPostDataFromStream()
        {
            try
            {
                Request.Body.Position = 0;
                string xml = string.Empty;

                using (var sr = new StreamReader(Request.Body))
                {
                    xml = await sr.ReadToEndAsync();
                }

                return xml;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeUpdateResult"></param>
        /// <returns></returns>
        private async Task SFMCSelfSelectedStoreUpdate(StoreUpdateResult storeUpdateResult)
        {
            try
            {
                if (storeUpdateResult != null && storeUpdateResult.Success == "true")
                {

                    //upsert in SFMC  with self selected Store 
                    DataExtentionsRequest dataExtentionsRequest = new DataExtentionsRequest();
                    List<SalesForce.Models.Item> list = new List<SalesForce.Models.Item>();
                    SalesForce.Models.Item item = new SalesForce.Models.Item();

                    if (!string.IsNullOrEmpty(storeUpdateResult.MemberId) && !string.IsNullOrEmpty(storeUpdateResult.ChainId))
                    {
                        item.MEMBER_ID = storeUpdateResult.MemberId;

                        switch (storeUpdateResult.ChainId.Trim())
                        {
                            case "1":
                                item.Self_Selected_Store_WD = storeUpdateResult.StoreId;
                                break;
                            case "2":
                                item.Self_Selected_Store_BL = storeUpdateResult.StoreId;
                                break;
                            case "3":
                                item.Self_Selected_Store_HVY = storeUpdateResult.StoreId;
                                break;
                            case "4":
                                item.Self_Selected_Store_FYM = storeUpdateResult.StoreId;
                                break;
                            default:
                                break;
                        }

                        list.Add(item);
                        dataExtentionsRequest.items = list;


                        //sfmc 
                        DataExtentionsResponse dataExtentionsResponse = await salesForceService.UpsertAsync(dataExtentionsRequest, Configuration["Settings:SalesForce:SEG_Key"]).ConfigureAwait(false);

                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}