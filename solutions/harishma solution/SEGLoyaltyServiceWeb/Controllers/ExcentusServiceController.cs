//using SEG.Excentus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Excentus;
using SEG.ApiService.Models.Excentus.request;
using Microsoft.Extensions.Caching.Distributed;
using SEG.LoyaltyService.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace SEGLoyaltyServiceWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcentusServiceController : Controller
    {

        //FuelRewardsWebService fuelRewardsWebService;
        //IConfiguration Configuration;
        //SEG.CustomerLibrary.CustomerService customerServiceAzure;
        //const string WinnDixieCRCIdPrefix = "9800";
        //const string FRNCardPrefix = "722";
        //Boolean goldStatusCheck { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="configuration"></param>
        ///// <param name="distributedCache"></param>
        //public ExcentusServiceController(IConfiguration configuration, IDistributedCache distributedCache)
        //{
        //    Configuration = configuration;
        //    fuelRewardsWebService = new FuelRewardsWebService(
        //                                   Configuration["Settings:Excentus:EndPoint"],
        //                                  Configuration["Settings:Excentus:ProgramUdk"],
        //                                  Configuration["Settings:Excentus:PartSrcId"],
        //                                  Configuration["Settings:Excentus:participantId"],
        //                                   Configuration["Settings:Excentus:clientID"],
        //                                    Configuration["Settings:Excentus:clientSecret"],
        //                                 distributedCache);

        //    customerServiceAzure = new SEG.CustomerLibrary.CustomerService(Configuration["Settings:StoreWebAPIEndPoint"], Configuration["Settings:OFferWebAPIEndPoint"], Configuration["Settings:LoyaltyEndPoint"], Configuration["Settings:AzureLoyaltyApiEndpoint"], Configuration["Settings:StorageConnectionString"], Configuration["Settings:CacheConnectionString"], Configuration["Settings:SalesForce:SalesForceAPIEndPoint"]);

        //    goldStatusCheck = Boolean.Parse(Configuration["Settings:Excentus:goldStatusCheck"]);

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/FRConnectSettings")]
        //public async Task<FRConnectSettings> FRConnectSettings()
        //{
        //    FRConnectSettings fRConnectSettings = null;
        //    try
        //    {
        //        fRConnectSettings = await fuelRewardsWebService.FRConnectSettings().ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in FRConnectSettings :", e);
        //    }

        //    return fRConnectSettings;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="memberLogin"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/FRLogin")]
        //public async Task<string> FRLogin(MemberLogin memberLogin)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.FRLogin(memberLogin.callBackUrl, memberLogin.emailAddress).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in FRLogin :", e);
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="frRegistrationRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/FRRegristration")]
        //public async Task<string> FRRegristration(FRRegistration frRegistrationRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.FRRegristration(frRegistrationRequest).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in FRRegristration :", e);
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="memberRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/MemberIDView")]
        //public async Task<MemberIDInfo> MemberIDView(MemberRequest memberRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.MemberIDView(memberRequest.accountNumber, memberRequest.securityCode).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in MemberIDView :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="preferenceUpdateRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/PreferenceUpdate")]
        //public async Task<string> PreferenceUpdate(PreferenceUpdateRequest preferenceUpdateRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.PreferenceUpdate(preferenceUpdateRequest.accountNumber, preferenceUpdateRequest.securityCode, preferenceUpdateRequest.preferenceId, preferenceUpdateRequest.optIn).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in PreferenceUpdate :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="profileUpdateRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/ProfileUpdate")]
        //public async Task<ProfileView> ProfileUpdate(ProfileUpdateRequest profileUpdateRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.ProfileUpdate(profileUpdateRequest.accountNumber, profileUpdateRequest.securityCode, profileUpdateRequest.profileUpdate).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in ProfileUpdate :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="memberRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/ProfileView")]
        //public async Task<ProfileView> ProfileView(MemberRequest memberRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.ProfileView(memberRequest.accountNumber, memberRequest.securityCode).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in ProfileView :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="transactionHistoryByCardRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/TransactionHistoryByCard")]
        //public async Task<TransactionHistoryResponse> TransactionHistoryByCard(TransactionHistoryByCardRequest transactionHistoryByCardRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.TransactionHistoryByCard(transactionHistoryByCardRequest.cardNumber, transactionHistoryByCardRequest.pageNumber, transactionHistoryByCardRequest.pageSize, transactionHistoryByCardRequest.transactionType).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in TransactionHistoryByCard :", e);
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="balanceDetailsByCard"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/BalanceDetailsByCard")]
        //public async Task<BalanceDetailsResponse> BalanceDetailsByCard(BalanceDetailsByCardRequest balanceDetailsByCard)
        //{
        //    try
        //    {
        //        BalanceDetailsResponse response = new BalanceDetailsResponse();
        //        FRNStatusValidateResponse fRNStatusResponse = null;
        //        string successCode = "success";
        //        string CPG = "cpg";
        //        decimal FRNgoldStatus = 0.05m;

        //        response = await fuelRewardsWebService.BalanceDetailsByCard(balanceDetailsByCard.cardNumber).ConfigureAwait(false);

        //        //add excentus points hack check
        //        if (goldStatusCheck)
        //        {
        //            if (response != null && response.responseCode.ToLower() == successCode)
        //            {
        //                foreach (BalanceDetail b in response.balanceDetails)
        //                {
        //                    if (b.rewardType.ToLower() == CPG)
        //                    {
        //                        fRNStatusResponse = await FRNCheck(balanceDetailsByCard.cardNumber).ConfigureAwait(false);
        //                        if (fRNStatusResponse != null)
        //                        {
        //                            if (fRNStatusResponse.Status.ToLower() == successCode && fRNStatusResponse.FRNStatus == true)
        //                            {
        //                                foreach (ParticipantReward reward in b.participantRewards)
        //                                {
        //                                    reward.rewardAmount = reward.rewardAmount + FRNgoldStatus;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in BalanceDetailsByCard :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="balancesByCardRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/BalancesByCard")]
        //public async Task<BalancesResponse> BalancesByCard(BalancesByCardRequest balancesByCardRequest)
        //{
        //    try
        //    {
        //        BalancesResponse response = new BalancesResponse();
        //        FRNStatusValidateResponse fRNStatusResponse = null;
        //        string successCode = "success";
        //        string CPG = "cpg";
        //        decimal FRNgoldStatus = 0.05m;

        //        response = await fuelRewardsWebService.BalancesByCard(balancesByCardRequest.cardNumber, balancesByCardRequest.rewardType).ConfigureAwait(false);

        //        //add excentus points hack check
        //        if (goldStatusCheck)
        //        {
        //            if (response != null && response.responseCode.ToLower() == successCode && response.rewardBalance != null)
        //            {
        //                if (response.rewardBalance.rewardType.ToLower() == CPG)
        //                {
        //                    fRNStatusResponse = await FRNCheck(balancesByCardRequest.cardNumber).ConfigureAwait(false);
        //                    if (fRNStatusResponse != null)
        //                    {
        //                        if (fRNStatusResponse.Status.ToLower() == successCode && fRNStatusResponse.FRNStatus == true)
        //                        {
        //                            response.rewardBalance.redeemableRewards = response.rewardBalance.redeemableRewards + FRNgoldStatus;
        //                            response.rewardBalance.totalRewards = response.rewardBalance.totalRewards + FRNgoldStatus;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in BalancesByCard :", e);
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rewardsInProgressByCardRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/RewardsInProgressByCard")]
        //public async Task<RewardsInProgressResponse> RewardsInProgressByCard(RewardsInProgressByCardRequest rewardsInProgressByCardRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.RewardsInProgressByCard(rewardsInProgressByCardRequest.cardNumber, rewardsInProgressByCardRequest.earnFlag).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in RewardsInProgressByCard :", e);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="transactionDetailByCardRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/TransactionDetailByCard")]
        //public async Task<TransactionDetailResponse> TransactionDetailByCard(TransactionDetailByCardRequest transactionDetailByCardRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.TransactionDetailByCard(transactionDetailByCardRequest.cardNumber, transactionDetailByCardRequest.referenceId).ConfigureAwait(false);
        //    }

        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in TransactionDetailByCard :", e);
        //    }
        //}

        ///// <summary>
        ///// get locations for fuel perks 
        ///// </summary>
        ///// <param name="locationsByCategoryRequest"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Excentus/LocationsByCategory")]
        //public async Task<LocationsByCategoryResponse> LocationsByCategory([FromBody]LocationsByCategoryRequest locationsByCategoryRequest)
        //{
        //    try
        //    {
        //        return await fuelRewardsWebService.LocationsByCategory(locationsByCategoryRequest).ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in LocationsByCategory :", e);
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <returns></returns>
        //private async Task<FRNStatusValidateResponse> FRNCheck(string cardNumber)
        //{
        //    try
        //    {
        //        FRNStatusValidateResponse response = null;

        //        if (cardNumber.StartsWith(FRNCardPrefix))
        //        {
        //            response = await customerServiceAzure.FRNStatusValidate(aliasNumber: cardNumber).ConfigureAwait(false);
        //        }
        //        else
        //        {
        //            if (cardNumber.StartsWith(WinnDixieCRCIdPrefix))
        //            {
        //                cardNumber = cardNumber.Replace(WinnDixieCRCIdPrefix, "");
        //            }

        //            response = await customerServiceAzure.FRNStatusValidate(crcid: cardNumber).ConfigureAwait(false);
        //        }

        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Exception in FRNCheck :", e);
        //    }
        //}
    }
}
