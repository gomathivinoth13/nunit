using SEG.EagleEyeLibrary.Controllers;
using SEG.EagleEyeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using StackExchange.Redis;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Text.RegularExpressions;
using SEG.Shared;
using SEG.ApiService.Models;
using SEG.EagleEyeLibrary.Models.Enum;
using SEG.EagleEyeLibrary.Models.CustomerCareCenter;
using Newtonsoft.Json;
using SEG.ApiService.Models.AuditCustomerServicePoints;
using SEG.AzureLoyaltyDatabase;

namespace SEG.EagleEyeLibrary.Process
{
    public class EagleEyePointsProcess
    {
        EagleEyeDAL serviceDAL;

        private const string SchemeReference = "RETAILPOINTS";
        private const string OfferDesc = "GOODWILL_ADJOFFER";
        private const string OfferID = "1718";
        private const string OfferType = "Local";
        private const string SiteID = "8000";



        public EagleEyePointsProcess(string clientID, string secret, string baseUrlWallet, string baseUrlCampaign, string ocpApimSubscriptionKeySecret, string loyaltyAzureConnection)
        {
            serviceDAL = new Controllers.EagleEyeDAL(clientID, secret, baseUrlWallet, baseUrlCampaign, ocpApimSubscriptionKeySecret);
            SEG.AzureLoyaltyDatabase.DataAccess.DapperDalBase.ConnectionString = loyaltyAzureConnection;
        }

        //public async Task<Shared.Response<GetWalletAccountsTrasactionsResponse>> GetWalletTransactions(GetWalletAccountsRequest request)
        //{
        //    Shared.Response<GetWalletAccountsTrasactionsResponse> result = null;
        //    try
        //    {
        //        //wallet accounts 
        //        var response = await serviceDAL.GetWalletAccountPoints(request).ConfigureAwait(false);
        //        if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(response.Result.Results[0].AccountId))
        //            {
        //                request.AccountId = response.Result.Results[0].AccountId;
        //                result = await serviceDAL.GetWalletTransactions(request).ConfigureAwait(false);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //    return result;
        //}


        public async Task<PointsProcessCustomerServiceResponse> ProcessPointsCustomerService(ProcessPointsCustomerServiceRequest request)
        {
            PointsProcessCustomerServiceResponse response = new PointsProcessCustomerServiceResponse();

            if (request != null)
            {
                try
                {
                    List<string> errorMessages = null;
                    errorMessages = ValidateProcessPointsCustomerServiceRequest(request);

                    if (errorMessages != null && errorMessages.Any())
                    {
                        string error = String.Format("Process Points failed with Errors: {0}", String.Join(", ", errorMessages));

                        response.State = "Error";
                        response.ErrorMessages = errorMessages.ToArray();
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;

                        return response;
                    }
                    else
                    {
                        response = await ProcessPoints(request).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return response;
        }

        private async Task<PointsProcessCustomerServiceResponse> ProcessPoints(ProcessPointsCustomerServiceRequest request)
        {
            PointsProcessCustomerServiceResponse response = new PointsProcessCustomerServiceResponse();
            List<String> errors = new List<string>();
            AuditProcessPoints auditRecord = null;

            try
            {
                if (request.AuditCustomerServiceRep != null && !string.IsNullOrWhiteSpace(request.AuditCustomerServiceRep.UserId))
                {
                    ///INSERT SERVICE REP INTO DATABASE 
                    var rep = await AzureLoyaltyDatabaseManager.GetAuditCustomerServiceRep(request.AuditCustomerServiceRep.UserId).ConfigureAwait(false);
                    if (rep == null || string.IsNullOrEmpty(rep.UserId))
                    {
                        bool repSuccess = await AzureLoyaltyDatabaseManager.InsertAuditCustomerServiceRep(request.AuditCustomerServiceRep);
                    }


                    ///INSERT SERVICE TICKET INTO DATABASE 
                    var ticket = await AzureLoyaltyDatabaseManager.GetAuditCustomerServiceTicket(request.AuditCustomerServiceTicket.TicketNumber).ConfigureAwait(false);
                    if (ticket == null || string.IsNullOrEmpty(ticket.TicketNumber))
                    {
                        request.AuditCustomerServiceTicket.CustomerServiceRep_UserId = request.AuditCustomerServiceRep.UserId;
                        bool ticketSuccess = await AzureLoyaltyDatabaseManager.InsertAuditCustomerServiceTicket(request.AuditCustomerServiceTicket);
                    }

                }


                WalletBackEndPointsRequest backEndRequest = new WalletBackEndPointsRequest();
                backEndRequest.WalletId = request.WalletID;
                backEndRequest.PointsValue = request.TotalPoints;
                backEndRequest.SchemeReference = SchemeReference;
                backEndRequest.ReasonCode = OfferID;
                backEndRequest.Reason = OfferDesc;
                Location location = new Location();
                location.StoreId = request.StoreNumber.ToString();
                location.StoreParentId = OfferType;
                backEndRequest.Location = location;
                backEndRequest.WalletTransactionType = "ADJUSTMENT";
                backEndRequest.WalletTransactionState = "ORIGINAL";
                backEndRequest.WalletTransactionDescription = string.Format("{0}{1}", request.AuditCustomerServiceTicket.Description, request.AuditCustomerServiceTicket.Comments);

                var WalletBackEndpoints = await serviceDAL.WalletBackEndpoints(backEndRequest).ConfigureAwait(false);

                if (WalletBackEndpoints != null && WalletBackEndpoints.IsSuccessful && WalletBackEndpoints.Result != null)
                {

                    auditRecord = new AuditProcessPoints()
                    {
                        AuditID = Guid.NewGuid().ToString("N"),
                        MemberID = request.MemberID,
                        WalletID = request.WalletID,
                        ReceiptNumber = string.Format("{0}{1}{2}G", request.MemberID, request.StoreNumber.HasValue ? request.StoreNumber.Value.ToString().PadLeft(5, '0') : "0".PadLeft(5, '0'), DateTime.Now.ToString("yyyyMMddHHmmss")),
                        StoreNumber = request.StoreNumber,
                        TotalPoints = request.TotalPoints,
                        LastUpdateDateTime = DateTime.Now,
                        CreateDateTime = DateTime.Now,
                        CreateUser = request.AuditCustomerServiceRep.UserId,
                        LastUpdateUser = request.AuditCustomerServiceRep.UserId,
                        CustomerServiceTicket_TicketNumber = request.AuditCustomerServiceTicket.TicketNumber,
                        ServicePointsResponse = JsonConvert.SerializeObject(WalletBackEndpoints.Result)
                    };

                    ///INSERT PROCESS INTO DATABASE 
                    bool audit = await AzureLoyaltyDatabaseManager.InsertAuditProcessPoints(auditRecord);


                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.State = "SUCCESS";
                    response.AuditProcessPoints = auditRecord;
                    response.AuditCustomerServiceTicket = request.AuditCustomerServiceTicket;

                }
                else
                {
                    if (!WalletBackEndpoints.IsSuccessful)
                    {
                        errors.Add(string.Format("ErrorCode = {0} ,ErrorDescription {1},ErrorDetails ={2}", WalletBackEndpoints.Result.ErrorCode, WalletBackEndpoints.Result.ErrorDescription, WalletBackEndpoints.Result.Details));
                        response.ErrorMessages = errors.ToArray();
                        response.StatusCode = WalletBackEndpoints.StatusCode;
                        response.State = "ERROR";
                    }
                    else
                    {
                        errors.Add(string.Format("ErrorCode = {0} ,ErrorDescription {1},ErrorDetails ={2}", "NP", "Empty List", null));
                        response.ErrorMessages = errors.ToArray();
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.State = "ERROR";
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private List<string> ValidateProcessPointsCustomerServiceRequest(ProcessPointsCustomerServiceRequest payload)
        {
            List<String> errors = new List<string>();
            if (payload != null)
            {
                if (payload.AuditCustomerServiceTicket == null)
                {
                    errors.Add("CustomerServiceTicket record is required to process Points");
                }
                else
                {
                    if (String.IsNullOrEmpty(payload.AuditCustomerServiceTicket.TicketNumber))
                        errors.Add("TicketNumber is a required field on the CustomerServiceTicket record");
                    if (String.IsNullOrEmpty(payload.AuditCustomerServiceTicket.Title))
                        errors.Add("Title is a required field on the CustomerServiceTicket record");
                    if (String.IsNullOrEmpty(payload.AuditCustomerServiceTicket.Description))
                        errors.Add("Description is a required field on the CustomerServiceTicket record");
                    else
                    {
                        if (payload.AuditCustomerServiceTicket.Description.Length > 500)
                            errors.Add("Description Field is Greater than 500 Characters");
                    }
                    if (payload.AuditCustomerServiceTicket.TicketDate == default(DateTime))
                        errors.Add("TicketDate is a required field on the CustomerServiceTicket record");
                    if (String.IsNullOrEmpty(payload.AuditCustomerServiceTicket.Comments))
                        errors.Add("Comments is a required field on the CustomerServiceTicket record");
                    else
                    {
                        if (payload.AuditCustomerServiceTicket.Comments.Length > 500)
                            errors.Add("Comments Field is Greater than 500 Characters");
                    }



                    if (payload.AuditCustomerServiceRep == null)
                    {
                        errors.Add("CustomerServiceRep record is required for the Process Points");
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(payload.AuditCustomerServiceRep.FirstName))
                            errors.Add("FirstName is a required field on the CustomerServiceRep record");
                        if (String.IsNullOrEmpty(payload.AuditCustomerServiceRep.LastName))
                            errors.Add("LastName is a required field on the CustomerServiceRep record");
                        if (String.IsNullOrEmpty(payload.AuditCustomerServiceRep.UserId))
                            errors.Add("UserId is a required field on the CustomerServiceRep record");
                    }
                }

                if (string.IsNullOrEmpty(payload.MemberID))
                    errors.Add("MemberID is a required field");
                if (string.IsNullOrEmpty(payload.WalletID))
                    errors.Add("WalletID is a required field");
                if (payload.TotalPoints == 0)
                    errors.Add("TotalPoints is a required field");

                return errors;
            }

            return errors;
        }
    }
}
