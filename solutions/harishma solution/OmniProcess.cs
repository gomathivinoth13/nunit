////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	OmniProcess.cs
//
// summary:	Implements the omni process class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SEG.ApiService.Models;
using SEG.ApiService.Models.AppSettings;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Excentus;
using SEG.ApiService.Models.Payload;
using SEG.CustomerWebService.Core;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyDatabase.Models;
using SEG.LoyaltyService.Process.Core.Interfaces;
using SEG.LoyaltyService.Process.Core.Models;

namespace SEG.LoyaltyService.Process.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An omni process. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class OmniProcess : IDisposable, IOmniProcess
    {
        private readonly AppSettingsOptions _settings;
        private readonly IPointRemediationService _pointRemediationService;
        private readonly ILoyaltyProcess _loyaltyProcess;
        private readonly ICustomerPointTransactionService _customerPointTransactionService;
        private readonly ICustomerServiceRepService _customerServiceRepService;
        private readonly ICustomerServiceTicketService _customerServiceTicketService;
        private readonly IGoodwillAuditService _goodwillAuditService;
        private readonly ICustomerService _customerService;
        private readonly string omniCredentials;
        readonly bool OMNI_ENABLED = true;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public OmniProcess(IOptions<AppSettingsOptions> settings, IPointRemediationService pointRemediationService,
            ILoyaltyProcess loyaltyProcess, ICustomerPointTransactionService customerPointTransactionService,
            ICustomerServiceRepService customerServiceRepService, ICustomerServiceTicketService customerServiceTicketService,
            IGoodwillAuditService goodwillAuditService, ICustomerService customerService)
        {
            _settings = settings.Value;
            _pointRemediationService = pointRemediationService;
            _loyaltyProcess = loyaltyProcess;
            _customerPointTransactionService = customerPointTransactionService;
            _customerServiceRepService = customerServiceRepService;
            _customerServiceTicketService = customerServiceTicketService;
            _goodwillAuditService = goodwillAuditService;
            _customerService = customerService;
            IDistributedCache cache = null;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Process the good will asynchronous described by payload. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="payload">  The payload. </param>
        /// <param name="excentusRewards"></param>
        ///
        /// <returns>   An asynchronous result that yields the process good will. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<GoodwillAudit> ProcessGoodWillAsync(ProcessGoodwillEventPayload payload, bool excentusRewards)
        {
            if (!OMNI_ENABLED) throw new NotImplementedException();

            GoodwillAudit auditRecord = null;
            Stopwatch watch = Stopwatch.StartNew();
            if (payload != null)
            {
                try
                {

                    auditRecord = new GoodwillAudit()
                    {
                        ChainId = int.Parse(payload.Banner.GetAttribute<ChainIdAttribute>().Value),
                        CrcId = payload.CrcId,
                        ReceiptNumber = string.Format("{0}{1}{2}G", payload.CrcId, payload.StoreNumber.HasValue ? payload.StoreNumber.Value.ToString().PadLeft(5, '0') : "0".PadLeft(5, '0'), DateTime.Now.ToString("yyyyMMddHHmmss")),
                        StoreNumber = payload.StoreNumber,
                        TotalPoints = payload.TotalPoints,
                        LastUpdateDateTime = DateTime.Now,
                        CreateDateTime = DateTime.Now
                    };

                    List<string> errorMessages = null;
                    errorMessages = ValidateProcessGoodwillEventPayload(payload);

                    if (errorMessages != null && errorMessages.Any())
                    {
                        string error = string.Format("ProcessGoodWill failed with: Invalid ProcessGoodwillEventPayload Record.  Errors: {0}", string.Join(", ", errorMessages));

                        auditRecord.State = "Error";
                        auditRecord.ErrorMessages = errorMessages.ToArray();
                        auditRecord.ReceiptNumber = null;
                    }
                    else
                    {
                        AuditQueuePayload<ProcessGoodwillEventPayload, GoodwillAudit> queuePayload = new AuditQueuePayload<ProcessGoodwillEventPayload, GoodwillAudit>()
                        {
                            Payload = payload,
                            AuditRecord = auditRecord
                        };

                        var formatControl = new System.Globalization.NumberFormatInfo
                        {
                            NumberDecimalDigits = 0
                        };
                        if (auditRecord != null && !auditRecord.ErrorMessages.Any())
                        {
                            //search database for alias card
                            var memberAlias = await _loyaltyProcess.GetMemberAlias(payload.CrcId.ToString(formatControl));

                            await _customerPointTransactionService.Add(new CustomerPointTransaction
                            {
                                Banner = (int)payload.Banner,
                                GoodwillValue = payload.TotalPoints,
                                MemberId = memberAlias.MemberId,
                                TransactionDateTime = DateTime.Now,
                                ReceiptNumber = auditRecord.ReceiptNumber
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    auditRecord.ErrorMessages = ExtractErrorMessages(ex, auditRecord.ErrorMessages).ToArray();
                }
                finally
                {
                    watch.Stop();
                }
            }

            return auditRecord;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public PointRemediation ProcessExcentusPointRedemptionV2(ExcentusPointsRedemptionPayloadV2 payloadV2)
        {
            PointRemediation auditRecord = null;
            Stopwatch watch = Stopwatch.StartNew();
            if (payloadV2 != null)
            {
                try
                {
                    auditRecord = new PointRemediation()
                    {
                        Store = payloadV2.Store,
                        Points = payloadV2.Points,
                        LastUpdateDateTime = DateTime.Now,
                        CreateDateTime = DateTime.Now
                    };

                    List<string> errorMessages = null;
                    errorMessages = ValidateProcessExcentusPointsRedemptionPayloadV2(payloadV2);

                    if (errorMessages != null && errorMessages.Any())
                    {
                        string error = string.Format("ProcessExcentusPointRedemptionAsync failed with: Invalid ProcessExcentusPointRedemptionPayload Record.  Errors: {0}", string.Join(", ", errorMessages));

                        auditRecord.State = "Error";
                        auditRecord.ErrorMessages = errorMessages.ToArray();
                        auditRecord.ReceiptNumber = null;
                    }
                }
                catch (Exception ex)
                {
                    auditRecord.ErrorMessages = ExtractErrorMessages(ex, auditRecord.ErrorMessages).ToArray();
                }
                finally
                {
                    watch.Stop();
                }
            }

            return auditRecord;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Extracts the error messages. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="e">                An Exception to process. </param>
        /// <param name="errorMessages">    (Optional) The error messages. </param>
        ///
        /// <returns>   The extracted error messages. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private IList<string> ExtractErrorMessages(Exception e, IList<string> errorMessages = null)
        {
            errorMessages = errorMessages?.ToList() ?? new List<string>();

            errorMessages.Add(e.Message);
            if (e.InnerException != null)
                return ExtractErrorMessages(e.InnerException, errorMessages);

            return errorMessages;
        }

        #region Private Helper Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets principal variant type by alias type. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="aliasType">    . </param>
        ///
        /// <returns>   The principal variant type by alias type. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //private PrincipalVariantType GetPrincipalVariantTypeByAliasType(AliasType aliasType)
        //{
        //    PrincipalVariantType type = PrincipalVariantType.CARDNUMBER;
        //    switch (aliasType)
        //    {
        //        case AliasType.PlentiCardNumber:
        //            type = PrincipalVariantType.CARDNUMBER;
        //            break;
        //        case AliasType.PhoneNumber:
        //            type = PrincipalVariantType.PHONENUMBER;
        //            break;
        //    }

        //    return type;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the process goodwill event payload described by payload. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="payload">  The payload. </param>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<string> ValidateProcessGoodwillEventPayload(ProcessGoodwillEventPayload payload)
        {
            List<String> errors = new List<string>();
            if (payload != null)
            {
                if (payload.TicketInformation == null)
                {
                    errors.Add("CustomerServiceTicket record is required for the GoodWillAudit");
                }
                else
                {
                    if (string.IsNullOrEmpty(payload.TicketInformation.TicketNumber))
                        errors.Add("TicketNumber is a required field on the CustomerServiceTicket record");
                    if (string.IsNullOrEmpty(payload.TicketInformation.Title))
                        errors.Add("Title is a required field on the CustomerServiceTicket record");
                    if (string.IsNullOrEmpty(payload.TicketInformation.Description))
                        errors.Add("Description is a required field on the CustomerServiceTicket record");
                    if (payload.TicketInformation.TicketDate == default(DateTime))
                        errors.Add("TicketDate is a required field on the CustomerServiceTicket record");

                    if (payload.TicketInformation.CustomerServiceRep == null)
                    {
                        errors.Add("CustomerServiceRep record is required for the GoodWillAudit");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(payload.TicketInformation.CustomerServiceRep.FirstName))
                            errors.Add("FirstName is a required field on the CustomerServiceRep record");
                        if (string.IsNullOrEmpty(payload.TicketInformation.CustomerServiceRep.LastName))
                            errors.Add("LastName is a required field on the CustomerServiceRep record");
                        if (string.IsNullOrEmpty(payload.TicketInformation.CustomerServiceRep.UserId))
                            errors.Add("UserId is a required field on the CustomerServiceRep record");
                    }
                }

                if (payload.CrcId == default(decimal))
                    errors.Add("CrcId is a required field on the audit record");
                if (payload.Banner == 0)
                    errors.Add("ChainID is a required field on the audit record");
                if (payload.TotalPoints == 0)
                    errors.Add("TotalPoints is a required field on the audit record");
                if (payload.TicketInformation.Description.Length > 500)
                    errors.Add("Description Field is Greater than 500 Characters");
                return errors;
            }

            return errors;
        }


        private List<string> ValidateProcessExcentusPointsRedemptionPayloadV2(ExcentusPointsRedemptionPayloadV2 payload)
        {
            List<String> errors = new List<string>();
            if (payload != null)
            {
                if (string.IsNullOrEmpty(payload.CRC))
                    errors.Add("Crc is a required field on the payload");
                if (string.IsNullOrEmpty(payload.OfferID))
                    errors.Add("OfferID is a required field on the payload");
                if (string.IsNullOrEmpty(payload.OfferDescription))
                    errors.Add("OfferDescription is a required field on the payload");
                if (string.IsNullOrEmpty(payload.Store))
                    errors.Add("Store is a required field on the payload");
                if (payload.Points == 0)
                    errors.Add("Points is a required field on the audit record");
                if (payload.TicketDescription.Length > 500)
                    errors.Add("Description Field is Greater than 500 Characters");
                if (string.IsNullOrEmpty(payload.TicketNumber))
                    errors.Add("TicketNumber is a required field on the CustomerServiceTicket record");
                return errors;
            }

            return errors;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the goodwill audit record described by auditRecord. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="auditRecord">  The audit record. </param>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<string> ValidateGoodwillAuditRecord(GoodwillAudit auditRecord)
        {
            List<String> errors = new List<string>();
            if (auditRecord == null)
            {
                errors.Add("AuditRecord is null");
                return errors;
            }

            if (auditRecord.CustomerServiceTicket == null)
            {
                errors.Add("CustomerServiceTicket record is required for the GoodWillAudit");
            }
            else
            {
                if (auditRecord.CustomerServiceTicket.CustomerServiceRep == null)
                {
                    errors.Add("CustomerServiceRep record is required for the GoodWillAudit");
                }
                else
                {
                    if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.CustomerServiceRep.FirstName))
                        errors.Add("FirstName is a required field on the CustomerServiceRep record");
                    if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.CustomerServiceRep.LastName))
                        errors.Add("LastName is a required field on the CustomerServiceRep record");
                    if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.CustomerServiceRep.UserId))
                        errors.Add("UserId is a required field on the CustomerServiceRep record");
                }

                if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.TicketNumber))
                    errors.Add("TicketNumber is a required field on the CustomerServiceTicket record");
                if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.Title))
                    errors.Add("Title is a required field on the CustomerServiceTicket record");
                if (string.IsNullOrEmpty(auditRecord.CustomerServiceTicket.Description))
                    errors.Add("Description is a required field on the CustomerServiceTicket record");
                if (auditRecord.CustomerServiceTicket.TicketDate == default(DateTime))
                    errors.Add("TicketDate is a required field on the CustomerServiceTicket record");
            }

            if (auditRecord.CrcId == default(decimal))
                errors.Add("CrcId is a required field on the audit record");
            if (auditRecord.ChainId == 0)
                errors.Add("ChainID is a required field on the audit record");
            if (auditRecord.TotalPoints == 0)
                errors.Add("TotalPoints is a required field on the audit record");

            return errors;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Format phone number. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="phoneNumber">  The phone number. </param>
        ///
        /// <returns>   The formatted phone number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private string FormatPhoneNumber(string phoneNumber)
        {
            if (!phoneNumber.StartsWith("+"))
            {
                if (!phoneNumber.StartsWith("1"))
                {
                    phoneNumber = "+1" + phoneNumber;
                }
                else
                {
                    phoneNumber = "+" + phoneNumber;
                }
            }

            return phoneNumber;
        }
        #endregion Private Helper Methods

        #region IDisposable Support
        private bool disposedValue = false;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   This code added to correctly implement the disposable pattern. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="disposing">    True to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {


                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   This code added to correctly implement the disposable pattern. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <seealso cref="M:System.IDisposable.Dispose()"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public async Task<PointRemediation> ProcessEEPointRedemptionAsync(ExcentusPointsRedemptionPayloadV2 payloadV2)
        {
            PointRemediation auditRecord = new PointRemediation();
            if (payloadV2 != null)
            {
                try
                {
                    List<string> errorMessages = null;
                    errorMessages = ValidateProcessExcentusPointsRedemptionPayloadV2(payloadV2);

                    if (errorMessages != null && errorMessages.Any())
                    {
                        string error = String.Format("ProcessExcentusPointRedemptionAsync failed with: Invalid ProcessExcentusPointRedemptionPayload Record.  Errors: {0}", String.Join(", ", errorMessages));

                        auditRecord.State = "Error";
                        auditRecord.ErrorMessages = errorMessages.ToArray();
                        auditRecord.ReceiptNumber = null;
                    }
                    else
                    {
                        auditRecord = await ProcessEEPointRedemptionAsync(payloadV2, auditRecord);
                    }
                }
                catch (Exception ex)
                {

                 auditRecord.ErrorMessages = ExtractErrorMessages(ex, auditRecord.ErrorMessages).ToArray();

                }
            }

            if (auditRecord.ErrorMessages.Length == 1 && auditRecord.ErrorMessages[0].Trim() == string.Empty) auditRecord.ErrorMessages = null;
            return auditRecord;
        }

        public Dictionary<string, object> GetHeaders()
        {
            return new Dictionary<string, object>() { { "Ocp-Apim-Subscription-Key", _settings.SubscriptionKey } };
        }

        private async Task<PointRemediation> ProcessEEPointRedemptionAsync(ExcentusPointsRedemptionPayloadV2 payloadV2, PointRemediation auditRecord)
        {  
            CustomerSearchResponse results = null;
            string walletId = null;

            AdjustItemdetailsRequest request = new AdjustItemdetailsRequest();
            List<string> errorMessage = new List<string>();

            auditRecord.Store = payloadV2.Store;
            auditRecord.Points = payloadV2.Points;
            auditRecord.LastUpdateDateTime = DateTime.Now;
            auditRecord.CreateDateTime = DateTime.Now;
            auditRecord.ErrorMessages = new string[] { };
            auditRecord.CustomerServiceTicket = new CustomerServiceTicket
            {
                TicketNumber = payloadV2.TicketNumber,
                Description = payloadV2.TicketDescription
            };
            auditRecord.CRC = payloadV2.CRC;
            auditRecord.OfferID = payloadV2.OfferID;
            auditRecord.UserId = payloadV2.UserId;
            auditRecord.TransmitDate = payloadV2.TransmitDate;
            auditRecord.LastUpdateDateTime = DateTime.Now;
            auditRecord.OrderNumber = payloadV2.OrderNumber;

            try
            {
                if (!string.IsNullOrWhiteSpace(payloadV2.CRC))
                {
                    //check to see if its a  G&G card number 
                    if (payloadV2.CRC.StartsWith(_settings.GGCardNumberPrefix))
                    {
                        results = await _customerService.CustomerSearchAsync(new CustomerSearchRequest() { OmniId = payloadV2.CRC });
                        if (results != null && results.IsSuccessful && results.Customers.Any())
                        {
                            if (results.Customers.First().CustomerAlias.Any(a => a.AliasStatus == 0))
                            {
                                var customer = results.Customers.First();
                                walletId = customer.CustomerWallet.First().WalletId;
                            }
                            else
                            {
                                errorMessage.Add(String.Format("Card is invaild", auditRecord.ErrorMessages));
                                auditRecord.ErrorMessages = errorMessage.ToArray();
                                auditRecord.Status = "Failed";
                                auditRecord.State = "Failure";
                                var prResult = await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                                throw new ApplicationException("Unable to Apply Eagle Eye PointRedemption due to invalid cardNumber");
                            }
                        }
                        else
                        {
                            errorMessage.Add(String.Format("Card is invaild", auditRecord.ErrorMessages));
                            auditRecord.ErrorMessages = errorMessage.ToArray();
                            auditRecord.Status = "Failed";
                            auditRecord.State = "Failure";
                            var prResult = await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                            throw new ApplicationException("Unable to Apply Eagle Eye PointRedemption due to lack of cardNumber");
                        }

                    }
                    else
                    {
                        results = await _customerService.CustomerSearchAsync(new CustomerSearchRequest() { CrcId = auditRecord.CRC });
                        if (results != null && results.IsSuccessful && results.Customers.Any())
                        {
                            if (results.Customers.First().CustomerAlias.Any(a => a.AliasStatus == 0)) //Check if status is active
                            {
                                var customer = results.Customers.First();
                                walletId = customer.CustomerWallet.First().WalletId;
                            }
                            else
                            {
                                errorMessage.Add(String.Format("Alias Status is not active for Crc", auditRecord.ErrorMessages));
                                auditRecord.ErrorMessages = errorMessage.ToArray();
                                auditRecord.Status = "Failed";
                                auditRecord.State = "Failure";
                                var prResult = await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                                throw new ApplicationException("Unable to Apply Eagle Eye PointRedemption due to non-active crc");
                            }
                        }
                        else
                        {
                            errorMessage.Add(String.Format("Crc does not exist in ODS", auditRecord.ErrorMessages));
                            auditRecord.ErrorMessages = errorMessage.ToArray();
                            auditRecord.Status = "Failed";
                            auditRecord.State = "Failure";
                            var prResult = await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                            throw new ApplicationException("Unable to Apply Eagle Eye PointRedemption due to invalid Crc");
                        }
                    }
                }

                WalletBackEndPointsRequest backEndRequest = new WalletBackEndPointsRequest();
                backEndRequest.WalletId = walletId;
                backEndRequest.PointsValue = auditRecord.Points;
                backEndRequest.SchemeReference = _settings.SchemeReference;
                backEndRequest.ReasonCode = auditRecord.OfferID; 
                backEndRequest.Reason = payloadV2.OfferDescription;
                Models.Location location = new Models.Location();
                location.StoreId = auditRecord.Store;
                location.StoreParentId = _settings.OfferType;
                backEndRequest.Location = location;
                backEndRequest.WalletTransactionType = "ADJUSTMENT";
                backEndRequest.WalletTransactionState = "ORIGINAL";
                backEndRequest.WalletTransactionDescription = string.Format("{0}-{1}", auditRecord.CustomerServiceTicket.TicketNumber, auditRecord.CustomerServiceTicket.Description);

                Dictionary<string, object> headers = GetHeaders();

                //specify to use TLS 1.2 as default connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var responseShared = await SEG.Shared.ApiUtility.RestfulPostAsync<WalletBackEndPointsResponse>(backEndRequest, "WalletBackEndpoints", _settings.EagleEyeBaseUrl, null,
                headers).ConfigureAwait(false);
                WalletBackEndPointsResponse response = responseShared.Result;

                if (response != null && response.Status != null)
                {
                    if (string.IsNullOrEmpty(response.errorMessage))
                    {
                        try
                        {
                            auditRecord.ErrorMessages = new string[] { };
                            auditRecord.Status = "Successful";
                            auditRecord.State = "Success";
                            await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                        }
                        catch (Exception ex)
                        {
                            auditRecord.Status = "Failed";
                            auditRecord.State = "";
                            errorMessage.Add(String.Format("SQL Exception: {0}", ex.ToString()));
                            auditRecord.ErrorMessages = errorMessage.ToArray();
                            await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                        }
                    }
                    else
                    {
                        auditRecord.Status = "Failed";
                        auditRecord.State = "Invalid Eagle Eye Request";

                        List<string> msgs = new List<string>();
                        msgs.Add("Code: " + response.ErrorCode);
                        msgs.Add("Message: " + response.errorMessage);
                        msgs.Add("Desc: " + response.ErrorDescription);
                        auditRecord.ErrorMessages = msgs.ToArray();
                        errorMessage.Add(String.Format("Eagle Eye Service error: {0}", auditRecord.ErrorMessages));
                        await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                    }
                }
                else
                {
                    auditRecord.State = "Invalid Eagle Eye Null response";
                    auditRecord.Status = "Failed";
                    errorMessage.Add("Null response object from Eagle Eye PointRedemption");
                    //auditRecord.EE_errormsg = response.ErrorDescription;
                    auditRecord.ErrorMessages = errorMessage.ToArray();
                    await _pointRemediationService.InsertPointRemediationsV2Async(auditRecord);
                }
            }
            catch (Exception ex)
            {
                if (auditRecord != null && !auditRecord.ErrorMessages.Any())
                    auditRecord.ErrorMessages = ExtractErrorMessages(ex, auditRecord.ErrorMessages).ToArray();
            }

            return auditRecord;
        }
        #endregion

    }
}