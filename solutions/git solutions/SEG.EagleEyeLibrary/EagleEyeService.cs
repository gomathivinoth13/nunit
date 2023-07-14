using SEG.EagleEyeLibrary.Controllers;
using SEG.EagleEyeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SEG.EagleEyeLibrary
{
    public class EagleEyeService
    {


        //// <summary>   The service dal. </summary>
        EagleEyeDAL serviceDAL;



        public EagleEyeService(string clientID, string secret, string baseUrlWallet, string baseUrlCampaign, string ocpApimSubscriptionKeySecret)
        {
            serviceDAL = new Controllers.EagleEyeDAL(clientID, secret, baseUrlWallet, baseUrlCampaign, ocpApimSubscriptionKeySecret);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccounts(GetWalletAccountsRequest request)
        {
            try
            {
                return await serviceDAL.GetWalletAccounts(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<Shared.Response<List<WalletRecommendations>>> GetWalletRecommendations(GetWalletRecommendationsRequest request)
        {
            try
            {
                return await serviceDAL.GetWalletRecommendations(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }


        public async Task<Shared.Response<List<WalletRecommendations>>> GetWalletRecommendationsIdentity(GetWalletRecommendationsRequest request)
        {
            try
            {
                return await serviceDAL.GetWalletRecommendationsIdentity(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="walletId"></param>
        ///// <param name="campaignId"></param>
        ///// <param name="createCouponAccountRequest"></param>
        ///// <returns></returns>
        //public async Task<Shared.Response<ActivateWalletAccountResponse>> ActivateWalletAccount(ActivateWalletAccountRequest createCouponAccountRequest)
        //{
        //    try
        //    {
        //        var response = await serviceDAL.ActivateWalletAccount(createCouponAccountRequest).ConfigureAwait(false);

        //        return response;


        //    }
        //    catch (Exception ex)
        //    {
        //        //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Shared.Response<GetCampaignsResponse>> GetCampaigns(GetCampaignsRequest request)
        {
            try
            {
                return await serviceDAL.GetCampaigns(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public async Task<Shared.Response<Campaign>> GetCampaignsByCampaignId(string campaignId)
        {
            try
            {
                return await serviceDAL.GetCampaignsByCampaignId(campaignId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Shared.Response<GetWalletAccountsTrasactionsResponse>> GetWalletTransactions(GetWalletAccountsRequest request)
        {
            try
            {
                return await serviceDAL.GetWalletTransactions(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccountPoints(GetWalletAccountsRequest request)
        {
            try
            {
                return await serviceDAL.GetWalletAccountPoints(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<Shared.Response<WalletBackEndPointsResponse>> WalletBackEndpoints(WalletBackEndPointsRequest request)
        {
            try
            {
                return await serviceDAL.WalletBackEndpoints(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<Shared.Response<GetWalletIdentitiesResponse>> GetWalletIdentities(string walletID)
        {
            try
            {
                return await serviceDAL.GetWalletIdentities(walletID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Logging.Error(String.Format("An error occured while trying to run FRRegristration.  Error {0}", ex.Message), ex);
                throw;
            }
        }
    }
}
