using SEG.EagleEyeLibrary.Controllers;
using SEG.EagleEyeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
//using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Text.RegularExpressions;
using SEG.Shared;
using SEG.ApiService.Models;
using SEG.EagleEyeLibrary.Models.Enum;
using SEG.AzureLoyaltyDatabase;
using SEG.ApiService.Models.MobileFirst;
using SEG.StoreLocatorLibrary;


namespace SEG.EagleEyeLibrary.Process
{
    public class EagleEyeProcess
    {

        string cacheConnectionString { get; set; }
        string cacheServer { get; set; }
        string cosmosEndpoint { get; set; }
        string cosmosPrimary { get; set; }

        string cosmosDataBase { get; set; }

        string cosmosContainer { get; set; }

        private const string Wallet_AccountsCacheKey = "Wallet_Accounts";
        private const string Wallet_RecommendationsCacheKey = "Wallet_Recommendations";
        private const string Wallet_AllOffersCacheKey = "Wallet_AllOffers";
        private const string Wallet_AllOffersCacheKeySFMC = "Wallet_AllOffers_SFMC";
        private const string Wallet_WalletAccountsCacheSFMC = "Wallet_Accounts_SFMC";
        private const string Guest_AllOffersCacheKey = "Guest_AllOffers";
        private const string Wallet_MBOCacheKey = "Wallet_MBO";
        private const string GetAllOffers_CosmosDB = "GetAllOffers_CosmosDB";
        private const string GetDefaultBoosters_CosmosDB = "GetDefaultBoosters_CosmosDB";

        //private const int Max_Cache_Limit = 1500;

        // return only 2 max  boosters for users 
        private const int MaxBoosterCount = 2;

        //return only 1 max Control boosters for user 
        private const int MaxControlBoosterCount = 1;

        private List<string> BlockedCategories = new List<string>() { "alcohol", "tobacco", "familyplanning" };


        EagleEyeDAL serviceDAL;
        EagleEyeService serviceEE;
        protected StackExchange.Redis.IDatabase _cache;
        protected StackExchange.Redis.IDatabase _cacheDatabase1;
        protected StackExchange.Redis.IDatabase _cacheDatabase2;
        CosmosClient cosmosClient;


        public EagleEyeProcess(string clientID, string secret, string baseUrlWallet, string baseUrlCampaign, string cacheConnection, string cosmosEndpointUri, string cosmosPrimaryKey, string cosmosDataBaseId, string cosmosContainerId, string cacheServerUrl, string ocpApimSubscriptionKeySecret, string loyaltyAzureConnection)
        {
            serviceEE = new EagleEyeService(clientID, secret, baseUrlWallet, baseUrlCampaign, ocpApimSubscriptionKeySecret);
            serviceDAL = new Controllers.EagleEyeDAL(clientID, secret, baseUrlWallet, baseUrlCampaign, ocpApimSubscriptionKeySecret);
            cacheConnectionString = cacheConnection;
            cosmosEndpoint = cosmosEndpointUri;
            cosmosPrimary = cosmosPrimaryKey;
            cosmosDataBase = cosmosDataBaseId;
            cosmosContainer = cosmosContainerId;
            cacheServer = cacheServerUrl;
            RedisConnectorHelper.localHost = cacheConnectionString;
            SEG.AzureLoyaltyDatabase.DataAccess.DapperDalBase.ConnectionString = loyaltyAzureConnection;

            CosmosConnectionHelper.cosmosEndpoint = cosmosEndpointUri;
            CosmosConnectionHelper.cosmosPrimary = cosmosPrimaryKey;

        }


        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccountPoints(GetWalletAccountsRequest request)
        {
            Shared.Response<GetWalletAccountsResponse> pointsResult = null;
            List<Point> pointsInfo = new List<Point>();

            try
            {
                //wallet accounts 
                var response = await serviceDAL.GetWalletAccountPoints(request).ConfigureAwait(false);
                if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                {
                    if (!string.IsNullOrEmpty(response.Result.Results.FirstOrDefault().AccountId))
                    {
                        request.AccountId = response.Result.Results.FirstOrDefault().AccountId;
                        pointsResult = await serviceDAL.GetWalletAccountPointsExpiryDate(request).ConfigureAwait(false);

                        if (pointsResult != null && pointsResult.IsSuccessful && pointsResult.Result != null && pointsResult.Result.Results != null && pointsResult.Result.Results.Count > 0)
                        {
                            foreach (var r in pointsResult.Result.Results)
                            {
                                Point point = new Point();
                                point.Points = r.Points;
                                point.ValidTo = r.ValidTo;

                                pointsInfo.Add(point);
                            }

                            response.Result.Results.FirstOrDefault().PointsInfo = pointsInfo;
                        }
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetWalletAccountsTrasactionsResponse>> GetWalletTransactions(GetWalletAccountsRequest request)
        {
            Shared.Response<GetWalletAccountsTrasactionsResponse> result = null;
            try
            {
                //wallet accounts 
                var response = await serviceDAL.GetWalletAccountPoints(request).ConfigureAwait(false);
                if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                {
                    if (!string.IsNullOrEmpty(response.Result.Results.FirstOrDefault().AccountId))
                    {
                        request.AccountId = response.Result.Results.FirstOrDefault().AccountId;
                        result = await serviceDAL.GetWalletTransactions(request).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }


        public async Task<GetWalletAccountsTrasactionsResponse> UpdateStoreAddresses(
                                                                GetWalletAccountsTrasactionsResponse transactions,
                                                                string storeLocatorDbConnectionString,
                                                                string storeLocatorRedisConnection)
        {
            if (transactions == null || transactions.Results == null || !transactions.Results.Any())
                return transactions;


            var dataAccess = StoreLocatorLibrary.Repository.StoreLocatorDataAccess.Connect(storeLocatorDbConnectionString);
            var repo = StoreLocatorLibrary.Repository.StoreLocatorRepository.Connect(
                        dataAccess,
                        cfg => cfg.RedisCacheConnection = storeLocatorRedisConnection);

            transactions.Results.ForEach(r => r.transactionDetails = UpdateAddress(r.transactionDetails, repo));

            return transactions;
        }


        private static TransactionDetails UpdateAddress(
                        TransactionDetails details,
                        StoreLocatorLibrary.Shared.Interfaces.IStoreLocatorRepository repo)
        {
            if (details == null ||
                    string.IsNullOrEmpty(details.merchant_store_id) ||
                    !int.TryParse(details.merchant_store_id, out int storeId))
                return details;
            
            var storeAddress = repo.GetStoreAddress(
                new StoreLocatorLibrary.Shared.RequestModels.GetStoreAddressRequest
                    {
                        StoreId = storeId
                    });

            if (!storeAddress.HasValue)
                return details;

            details.ShoppingCenterName = storeAddress.Value.StoreName;
            details.StoreAddress1 = storeAddress.Value.Address.AddressLine1;
            details.StoreAddress1 = storeAddress.Value.Address.AddressLine2;
            details.StoreCity = storeAddress.Value.Address.City;
            details.StoreZip = storeAddress.Value.Address.Zipcode;

            return details;
        }



        public async Task<WalletCouponsCacheResponse> GetAllMBOCache(GetAllCampaignsCacheRequest request, string BannerPartnerCode, string ChainId)
        {
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();

            try
            {
                if (!string.IsNullOrEmpty(request.MemberId))
                {
                    string cacheKey = string.Format("{0}:{1}:{2}:{3}", Wallet_MBOCacheKey, request.WalletId, request.ChainId, request.StoreId);

                    ///get list of ALL MBO 
                    RedisValue walletAllOffersByWalletId = getSingleKeyFromCacheDatabase1(cacheKey);

                    if (!walletAllOffersByWalletId.IsNullOrEmpty)
                    {
                        result = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAllOffersByWalletId);
                        if (request.AlcoholOffersAccepted != result.AlcoholOffersAccepted)
                        {
                            result = await getAllWalletMBO(request, BannerPartnerCode, ChainId, cacheKey);
                            addToCacheDatabase1(request.WalletId, result, cacheKey);
                        }

                        return result;
                    }
                    else
                    {
                        result = await getAllWalletMBO(request, BannerPartnerCode, ChainId, cacheKey);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<WalletCouponsCacheResponse> GetAllOffersCacheSFMC(GetAllCampaignsCacheRequest request, string BannerPartnerCode)
        {
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();

            try
            {

                //set default value 
                if (request.Limit == 0 || request.Limit > 20)
                {
                    request.Limit = 20;
                }
                request.Offset = 0;


                if (!string.IsNullOrEmpty(request.MemberId))
                {
                    ///get list of ALL offers  from cache 
                    string cacheKey = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKeySFMC, request.MemberId, request.ChainId, request.StoreId);
                    RedisValue walletAllOffersByWalletId = getSingleKeyFromCacheDatabase1(cacheKey);

                    if (!walletAllOffersByWalletId.IsNullOrEmpty)
                    {
                        result = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAllOffersByWalletId);
                        return result;
                    }
                    else
                    {
                        result = await getAllWalletRecommendationOffersSFMC(request, BannerPartnerCode, cacheKey);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<WalletCouponsCacheResponse> GetWalletAccountsCacheSFMC(GetAllCampaignsCacheRequest request, string BannerPartnerCode)
        {
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();

            try
            {

                //set default value 
                if (request.Limit == 0 || request.Limit > 20)
                {
                    request.Limit = 20;
                }
                request.Offset = 0;


                if (!string.IsNullOrEmpty(request.MemberId))
                {
                    ///get list of ALL offers  from cache 
                    string cacheKey = string.Format("{0}:{1}:{2}:{3}", Wallet_WalletAccountsCacheSFMC, request.MemberId, request.ChainId, request.StoreId);
                    RedisValue walletAllOffersByWalletId = getSingleKeyFromCacheDatabase1(cacheKey);

                    if (!walletAllOffersByWalletId.IsNullOrEmpty)
                    {
                        result = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAllOffersByWalletId);
                        return result;
                    }
                    else
                    {
                        result = await getAllWalletAccountOffersSFMC(request, BannerPartnerCode, cacheKey);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<WalletCouponsCacheResponse> GetAllOffersCache(GetAllCampaignsCacheRequest request, string BannerPartnerCode)
        {
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();

            try
            {
                if (request != null)
                {
                    if (request.Offset != 0)
                    {
                        result.StatusCode = System.Net.HttpStatusCode.OK;
                        return result;
                    }

                    if (!string.IsNullOrEmpty(request.WalletId) && !string.IsNullOrEmpty(request.MemberId))
                    {
                        ///get list of ALL offers  from cache 
                        string cacheKey = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKey, request.WalletId, request.ChainId, request.StoreId);
                        RedisValue walletAllOffersByWalletId = getSingleKeyFromCacheDatabase1(cacheKey);

                        if (!walletAllOffersByWalletId.IsNullOrEmpty)
                        {
                            result = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAllOffersByWalletId);

                            if (request.AlcoholOffersAccepted != result.AlcoholOffersAccepted)
                            {
                                result = await getAllWalletRecommendationOffers(request, BannerPartnerCode, cacheKey);
                                addToCacheDatabase1(request.WalletId, result, cacheKey);
                            }

                            if (result != null)
                            {
                                //if (request.nextCursor == result.currentCursor)
                                //{
                                //cached data is equal to user requested data 
                                //return cache data 
                                return result;
                                //}
                                //else
                                //{
                                //    result = await getAllWalletRecommendationOffers(request, BannerPartnerCode, cacheKey);
                                //}
                            }
                        }
                        else
                        {
                            result = await getAllWalletRecommendationOffers(request, BannerPartnerCode, cacheKey);
                        }
                    }
                    else
                    //guest user 
                    {
                        ///get all offers  from cache 
                        string cacheKey = string.Format("{0}:{1}:{2}:{3}:{4}", Guest_AllOffersCacheKey, request.ChainId, request.StoreId, request.Limit, request.Offset);
                        RedisValue walletAllOffersGuest = getSingleKeyFromCacheDatabase1(cacheKey);

                        if (walletAllOffersGuest.IsNullOrEmpty)
                        {

                            result = await getAllOffers(null, null, null, request.Offset, request.Limit, BannerPartnerCode, (SEG.ApiService.Models.Banner)request.ChainId, request.StoreId, null, request.AlcoholOffersAccepted = false).ConfigureAwait(false);

                            result.Offset = request.Offset;
                            result.Limit = request.Limit;
                            result.Total = result.Coupons.Count();
                            result.StatusCode = System.Net.HttpStatusCode.OK;

                            //add to cache 
                            addToCacheDatabase1("Guest", result, cacheKey);
                        }
                        else
                        {
                            result = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAllOffersGuest);
                        }
                    }
                }


                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> LoadWalletAccount(LoadWalletAccountRequest createCouponAccountRequest)
        {
            Shared.Response<LoadUnloadWalletAccountResponse> result = null;
            try
            {

                if (createCouponAccountRequest != null && !string.IsNullOrEmpty(createCouponAccountRequest.WalletId))
                {
                    //clear account , recommendations and get all offers  cache values 
                    string cacheKeyAccounts = string.Format("{0}:{1}:{2}:{3}", Wallet_AccountsCacheKey, createCouponAccountRequest.WalletId, createCouponAccountRequest.ChainId, createCouponAccountRequest.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAccounts);

                    //string cacheKeyRecommendations = string.Format("{0}:{1}:{2}:{3}", Wallet_RecommendationsCacheKey, createCouponAccountRequest.MemberId, createCouponAccountRequest.ChainId, createCouponAccountRequest.StoreId);
                    //deleteKeyInCacheDatabase1(cacheKeyRecommendations);

                    string cacheKeyAllOffers = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKey, createCouponAccountRequest.WalletId, createCouponAccountRequest.ChainId, createCouponAccountRequest.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAllOffers);

                    //clear MBO offers - MBO are recommendations , they do not exits in regular get all offers  
                    string cacheKeyMBO = string.Format("{0}:{1}:{2}:{3}", Wallet_MBOCacheKey, createCouponAccountRequest.WalletId, createCouponAccountRequest.ChainId, createCouponAccountRequest.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyMBO);


                    result = await serviceDAL.LoadWalletAccount(createCouponAccountRequest);
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> AcceptRecommendation(LoadWalletAccountRequest request)
        {
            Shared.Response<LoadUnloadWalletAccountResponse> result = null;
            try
            {

                if (request != null && !string.IsNullOrEmpty(request.WalletId))
                {
                    //clear account , recommendations and get all offers  cache values 
                    string cacheKeyAccounts = string.Format("{0}:{1}:{2}:{3}", Wallet_AccountsCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAccounts);

                    string cacheKeyRecommendations = string.Format("{0}:{1}:{2}:{3}", Wallet_RecommendationsCacheKey, request.MemberId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyRecommendations);

                    string cacheKeyAllOffers = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAllOffers);

                    string cacheKeyMBO = string.Format("{0}:{1}:{2}:{3}", Wallet_MBOCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyMBO);

                    result = await serviceDAL.AcceptRecommendation(request);

                    if (result.IsSuccessful && result.Result != null && !string.IsNullOrEmpty(result.Result.State))
                    {
                        if (result.Result.State == StateType.UNLOADED.ToString() && !string.IsNullOrEmpty(result.Result.WalletId) && !string.IsNullOrEmpty(result.Result.AccountId))
                        {
                            SetWalletAccountStateRequest setStateRequest = new SetWalletAccountStateRequest();
                            setStateRequest.WalletId = result.Result.WalletId;
                            setStateRequest.AccountId = result.Result.AccountId;
                            //load wallet Account 
                            setStateRequest.State = StateType.LOADED.ToString();

                            result = await serviceDAL.SetWalletAccountState(setStateRequest);
                        }
                    }

                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> UnLoadedWalletAccount(SetWalletAccountStateRequest request)
        {
            Shared.Response<LoadUnloadWalletAccountResponse> result = null;
            try
            {
                if (request != null && !string.IsNullOrEmpty(request.WalletId))
                {
                    //clear account , recommendations and get all offers  cache values 
                    string cacheKeyAccounts = string.Format("{0}:{1}:{2}:{3}", Wallet_AccountsCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAccounts);

                    //string cacheKeyRecommendations = string.Format("{0}:{1}:{2}:{3}", Wallet_RecommendationsCacheKey, request.MemberId, request.ChainId, request.StoreId);
                    //deleteKeyInCacheDatabase1(cacheKeyRecommendations);

                    string cacheKeyAllOffers = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAllOffers);

                    string cacheKeyMBO = string.Format("{0}:{1}:{2}:{3}", Wallet_MBOCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyMBO);

                    //Unload wallet Account 
                    request.State = StateType.UNLOADED.ToString();

                    result = await serviceDAL.SetWalletAccountState(request);
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> SetWalletAccountState(SetWalletAccountStateRequest request)
        {
            Shared.Response<LoadUnloadWalletAccountResponse> result = null;
            try
            {

                if (request != null && !string.IsNullOrEmpty(request.WalletId))
                {
                    //clear account , recommendations and get all offers  cache values 
                    string cacheKeyAccounts = string.Format("{0}:{1}:{2}:{3}", Wallet_AccountsCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAccounts);

                    //string cacheKeyRecommendations = string.Format("{0}:{1}:{2}:{3}", Wallet_RecommendationsCacheKey, request.MemberId, request.ChainId, request.StoreId);
                    //deleteKeyInCacheDatabase1(cacheKeyRecommendations);

                    string cacheKeyAllOffers = string.Format("{0}:{1}:{2}:{3}", Wallet_AllOffersCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyAllOffers);

                    string cacheKeyMBO = string.Format("{0}:{1}:{2}:{3}", Wallet_MBOCacheKey, request.WalletId, request.ChainId, request.StoreId);
                    deleteKeyInCacheDatabase1(cacheKeyMBO);

                    //Load wallet Account 
                    request.State = StateType.LOADED.ToString();

                    result = await serviceDAL.SetWalletAccountState(request);
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<WalletCouponsCacheResponse> GetWalletAccountsCache(GetWalletAccountsRequest request, string bannerPartnerCode)
        {
            WalletCouponsCacheResponse walletCouponsCacheResponse = new WalletCouponsCacheResponse();
            request.Offset = request.Offset;

            try
            {
                //get list of wallet accounts from cache 
                string cacheKey = string.Format("{0}:{1}:{2}:{3}", Wallet_AccountsCacheKey, request.WalletId, request.ChainId, request.StoreId);
                RedisValue walletAccountsByWalletId = getSingleKeyFromCacheDatabase1(cacheKey);

                if (!walletAccountsByWalletId.IsNullOrEmpty)
                {
                    walletCouponsCacheResponse = Serializer.JsonDeserialize<WalletCouponsCacheResponse>(walletAccountsByWalletId);
                    if (walletCouponsCacheResponse != null)
                    {
                        if (request.AlcoholOffersAccepted != walletCouponsCacheResponse.AlcoholOffersAccepted)
                        {
                            walletCouponsCacheResponse = await GetWalletAccounts(request, bannerPartnerCode, cacheKey);
                            addToCacheDatabase1(request.WalletId, walletCouponsCacheResponse, cacheKey);
                        }

                        if (request.Offset == walletCouponsCacheResponse.Offset && request.Limit == walletCouponsCacheResponse.Limit)
                        {
                            //cached data is equal to user requested data 
                            //return cache data 
                            return walletCouponsCacheResponse;
                        }
                        else
                        {
                            walletCouponsCacheResponse = await GetWalletAccounts(request, bannerPartnerCode, cacheKey).ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    walletCouponsCacheResponse = await GetWalletAccounts(request, bannerPartnerCode, cacheKey).ConfigureAwait(false);
                }


                return walletCouponsCacheResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<WalletCouponsCacheResponse> GetWalletAccounts(GetWalletAccountsRequest request, string bannerPartnerCode, string cacheKey)
        {
            WalletCouponsCacheResponse walletCouponsCacheResponse = new WalletCouponsCacheResponse();
            Shared.Response<GetWalletAccountsResponse> response = null;

            try
            {
                //get only active and loaded offers 
                request.State = StateType.LOADED.ToString();
                response = await serviceDAL.GetWalletAccounts(request).ConfigureAwait(false);

                if (response != null && response.IsSuccessful && response.Result != null)
                {
                    if (response.Result.Results != null && response.Result.Results.Count > 0)
                    {

                        List<Coupon> accountsResult = getCampaignsFromCacheAccounts(response.Result.Results, bannerPartnerCode, request.StoreId, (ApiService.Models.Banner)request.ChainId, request.AlcoholOffersAccepted);
                        walletCouponsCacheResponse.Limit = response.Result.Limit;
                        walletCouponsCacheResponse.Offset = response.Result.Offset;
                        walletCouponsCacheResponse.Total = accountsResult.Count();
                        walletCouponsCacheResponse.OrderBy = response.Result.OrderBy;
                        walletCouponsCacheResponse.StatusCode = response.StatusCode;

                        walletCouponsCacheResponse.Coupons = accountsResult;

                        //add to cache 
                        addToCacheDatabase1(request.WalletId, walletCouponsCacheResponse, cacheKey);

                    }
                    else
                    {
                        walletCouponsCacheResponse.ErrorCode = "NP";
                        walletCouponsCacheResponse.ErrorDescription = "Empty List:";
                        walletCouponsCacheResponse.Details = null;
                        walletCouponsCacheResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }
                else
                {
                    if (!response.IsSuccessful)
                    {
                        walletCouponsCacheResponse.ErrorCode = response.Result.ErrorCode;
                        walletCouponsCacheResponse.ErrorDescription = response.Result.ErrorDescription;
                        walletCouponsCacheResponse.Details = response.Result.Details;
                        walletCouponsCacheResponse.StatusCode = response.StatusCode;
                    }
                    else
                    {
                        walletCouponsCacheResponse.ErrorCode = "NP";
                        walletCouponsCacheResponse.ErrorDescription = "Empty List:";
                        walletCouponsCacheResponse.Details = null;
                        walletCouponsCacheResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }

                return walletCouponsCacheResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Coupon GetCampaignIdFromCache(string campaignId)
        {
            try
            {
                Coupon response = null;
                //get cache 
                RedisValue campaign = getSingleCouponFromCache(campaignId);
                if (campaign.HasValue)
                    response = Serializer.JsonDeserialize<Coupon>(campaign);

                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.CouponBeginDate))
                    {
                        DateTime beginDate = TimeConvertionEST(response.CouponBeginDate);
                        response.CouponBeginDate = beginDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    //expiry Date logic 
                    DateTime date = RollingAndExpiryDateLogic(response);
                }

                return response;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<WalletCouponsCacheResponse> getAllWalletRecommendationOffers(GetAllCampaignsCacheRequest request, string BannerPartnerCode, string cacheKey)
        {

            GetWalletAccountsRequest walletAccountRequest = new GetWalletAccountsRequest();
            GetWalletAccountsRequest walletAccountRequestoffSet = new GetWalletAccountsRequest();
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();
            Shared.Response<GetWalletAccountsResponse> response = null;
            Shared.Response<GetWalletAccountsResponse> responseOffSet = null;
            Shared.Response<List<WalletRecommendations>> recommendationsResponse = null;
            List<Account> unLoadedAccounts = null;
            List<WalletRecommendations> recommendationsAccounts = null;
            List<string> campaignIds = new List<string>();
            List<string> campaignIds2 = new List<string>();
            List<string> walletCampaignIds = new List<string>();
            //List<Coupon> coupons = null;
            GetWalletRecommendationsRequest recommendationsRequest = new GetWalletRecommendationsRequest();

            try
            {
                //getwalletAccounts
                walletAccountRequest.WalletId = request.WalletId;
                walletAccountRequest.Limit = 100;
                walletAccountRequest.Offset = 0;
                //get active , loaded or unloaded offers 
                walletAccountRequest.State = StateType.UNLOADED.ToString();

                //wallet accounts 
                response = await serviceDAL.GetWalletAccounts(walletAccountRequest);
                if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                {

                    campaignIds = response.Result.Results.Select(r => r.CampaignId).Distinct().ToList();

                    if (response.Result.Total >= 100)
                    {
                        walletAccountRequestoffSet.WalletId = request.WalletId;
                        walletAccountRequestoffSet.Limit = 100;
                        walletAccountRequestoffSet.Offset = 100;
                        //get active , loaded or unloaded offers 
                        walletAccountRequestoffSet.State = StateType.UNLOADED.ToString();

                        //wallet accounts 
                        responseOffSet = await serviceDAL.GetWalletAccounts(walletAccountRequestoffSet);
                        if (responseOffSet != null && responseOffSet.IsSuccessful && responseOffSet.Result != null && responseOffSet.Result.Results != null && responseOffSet.Result.Results.Count > 0)
                        {
                            campaignIds2 = responseOffSet.Result.Results.Select(r => r.CampaignId).Distinct().ToList();

                            if (campaignIds2.Count() > 0)
                                campaignIds.AddRange(campaignIds2);
                        }
                    }
                    //list of unloaded coupons 
                    if (request.Offset == 0)
                        unLoadedAccounts = response.Result.Results.Where(r => r.State == StateType.UNLOADED.ToString()).ToList();
                }


                //set campaignIDs in wallet . 
                walletCampaignIds.AddRange(campaignIds);

                //Customer Recommendations 
                recommendationsRequest.MemberId = request.MemberId;
                recommendationsResponse = await serviceDAL.GetWalletRecommendationsIdentity(recommendationsRequest);
                if (recommendationsResponse != null && recommendationsResponse.IsSuccessful && recommendationsResponse.Result != null)
                {
                    if (recommendationsResponse.Result != null && recommendationsResponse.Result.Count > 0)
                    {
                        foreach (WalletRecommendations r in recommendationsResponse.Result)
                        {
                            if (r != null && r.Weight != null && !string.IsNullOrEmpty(r.Target.FirstOrDefault()))
                            {
                                string Id = Regex.Replace(r.Target.FirstOrDefault(), "[^0-9]", "");
                                if (!string.IsNullOrEmpty(Id))
                                {
                                    campaignIds.Add(Id);
                                }
                            }
                        }

                        //list of recommendations coupons 
                        if (request.Offset == 0)
                            recommendationsAccounts = recommendationsResponse.Result;
                    }

                }

                result = await getAllOffers(campaignIds, unLoadedAccounts, recommendationsAccounts, request.Offset, request.Limit, BannerPartnerCode, (SEG.ApiService.Models.Banner)request.ChainId, request.StoreId, walletCampaignIds, request.AlcoholOffersAccepted).ConfigureAwait(false);

                result.Offset = request.Offset;
                result.Limit = request.Limit;
                result.Total = result.Coupons.Count();
                result.StatusCode = System.Net.HttpStatusCode.OK;

                //add get all offers cache 
                addToCacheDatabase1(request.WalletId, result, cacheKey);
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }


        private async Task<WalletCouponsCacheResponse> getAllWalletMBO(GetAllCampaignsCacheRequest request, string BannerPartnerCode, string ChainId, string cacheKey)
        {

            GetWalletAccountsRequest walletAccountRequest = new GetWalletAccountsRequest();
            GetWalletAccountsRequest walletAccountRequestoffSet = new GetWalletAccountsRequest();
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();
            Shared.Response<List<WalletRecommendations>> recommendationsResponse = null;
            List<Account> accounts = null;
            List<Account> accountsUsed = null;
            List<WalletRecommendations> recommendationsAccounts = null;
            List<string> walletCampaignIds = new List<string>();
            List<string> walletCampaignIds2 = new List<string>();
            List<Coupon> coupons = null;
            Coupon dealOfWeek = null;
            int totalCount = 0;
            List<Account> accountsList = new List<Account>();
            GetWalletRecommendationsRequest recommendationsRequest = new GetWalletRecommendationsRequest();

            try
            {
                //getwalletAccountsusing identy value
                walletAccountRequest.WalletId = request.WalletId;
                walletAccountRequest.Limit = 100;
                walletAccountRequest.Offset = 0;
                //get active , loaded or unloaded offers 
                walletAccountRequest.State = StateType.UNLOADED.ToString();

                do
                {
                    //wallet accounts 
                    var response = await serviceDAL.GetWalletAccounts(walletAccountRequest).ConfigureAwait(false);
                    if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                    {

                        accountsList.AddRange(response.Result.Results);
                        totalCount = response.Result.Total;
                        if (accountsList.Count != response.Result.Total)
                        {
                            walletAccountRequest.Limit = 100;
                            walletAccountRequest.Offset = walletAccountRequest.Offset + 100;
                        }
                    }
                }
                while (accountsList.Count != totalCount);

                if (accountsList != null && accountsList.Count > 0)
                {
                    walletCampaignIds = accountsList.Select(r => r.CampaignId).Distinct().ToList();

                    //list of loaded and unloaded  coupons 
                    if (request.Offset == 0)
                    {
                        accounts = accountsList.Where(r => r.Status.Trim() != StatusType.USED.ToString()).ToList();

                        accountsUsed = accountsList.Where(r => r.Status.Trim() == StatusType.USED.ToString() && (r.LastUpdated.Value != null && r.LastUpdated.Value.ToUniversalTime().AddDays(14) >= DateTime.UtcNow)).ToList();
                    }
                }

                //Customer Recommendations 
                recommendationsRequest.MemberId = request.MemberId;
                recommendationsResponse = await serviceDAL.GetWalletRecommendationsIdentity(recommendationsRequest);
                if (recommendationsResponse != null && recommendationsResponse.IsSuccessful && recommendationsResponse.Result != null)
                {
                    if (recommendationsResponse.Result != null && recommendationsResponse.Result.Count > 0)
                    {
                        //list of recommendations coupons 
                        if (request.Offset == 0)
                            recommendationsAccounts = recommendationsResponse.Result;
                    }

                }


                ////Deal of the week 
                CustomerRewardPromo customerRewardPromo = (await AzureLoyaltyDatabaseManager.GetCustomerRewardPromoDealOfWeek(DateTime.Now, ChainId).ConfigureAwait(false)).FirstOrDefault();
                //check to see if DOTW is already in user wallet
                if (customerRewardPromo != null && !string.IsNullOrEmpty(customerRewardPromo.ActionValue))
                {
                    if (walletCampaignIds == null || walletCampaignIds.Count() <= 0 || !walletCampaignIds.Contains(customerRewardPromo.ActionValue))
                    {
                        if (!string.IsNullOrEmpty(customerRewardPromo.EECouponObject))
                        {
                            dealOfWeek = Shared.Serializer.JsonDeserialize<Coupon>(customerRewardPromo.EECouponObject);
                            dealOfWeek.DealOfTheWeek = true;
                        }
                    }
                }

                coupons = await combineCouponUnloadedDataRecommendationsToMBO(BannerPartnerCode, (SEG.ApiService.Models.Banner)request.ChainId, request.StoreId, accounts, recommendationsAccounts, accountsUsed, dealOfWeek, walletCampaignIds, request.AlcoholOffersAccepted).ConfigureAwait(false);

                result.Offset = request.Offset;
                result.Limit = request.Limit;
                result.Coupons = coupons;
                result.Total = coupons.Count();
                result.StatusCode = System.Net.HttpStatusCode.OK;

                //add get all offers cache 
                addToCacheDatabase1(request.MemberId, result, cacheKey);

            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }


        private async Task<WalletCouponsCacheResponse> getAllWalletRecommendationOffersSFMC(GetAllCampaignsCacheRequest request, string BannerPartnerCode, string cacheKey)
        {

            GetWalletAccountsRequest walletAccountRequest = new GetWalletAccountsRequest();
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();
            Shared.Response<GetWalletAccountsResponse> response = null;
            Shared.Response<List<WalletRecommendations>> recommendationsResponse = null;
            List<Account> unLoadedAccounts = null;
            List<WalletRecommendations> recommendationsAccounts = null;
            List<string> campaignIds = new List<string>();
            List<string> walletCampaignIds = new List<string>();
            List<Coupon> coupons = null;
            GetWalletRecommendationsRequest recommendationsRequest = new GetWalletRecommendationsRequest();

            try
            {

                //getwalletAccountsusing identy value
                walletAccountRequest.MemberId = request.MemberId;
                walletAccountRequest.Limit = 100;
                walletAccountRequest.Offset = 0;
                //get active , loaded or unloaded offers 
                walletAccountRequest.State = StateType.UNLOADED.ToString();

                //wallet accounts 
                response = await serviceDAL.GetWalletAccountsByIdentityValue(walletAccountRequest);
                if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                {
                    campaignIds = response.Result.Results.Select(r => r.CampaignId).Distinct().ToList();
                }

                //set campaignIDs in wallet . 
                walletCampaignIds.AddRange(campaignIds);

                //Customer Recommendations 
                recommendationsRequest.MemberId = request.MemberId;
                recommendationsResponse = await serviceDAL.GetWalletRecommendationsIdentity(recommendationsRequest);
                if (recommendationsResponse != null && recommendationsResponse.IsSuccessful && recommendationsResponse.Result != null)
                {
                    if (recommendationsResponse.Result != null && recommendationsResponse.Result.Count > 0)
                    {
                        foreach (WalletRecommendations r in recommendationsResponse.Result)
                        {
                            if (r != null && r.Weight != null && !string.IsNullOrEmpty(r.Target.FirstOrDefault()))
                            {
                                string Id = Regex.Replace(r.Target.FirstOrDefault(), "[^0-9]", "");
                                if (!string.IsNullOrEmpty(Id))
                                {
                                    campaignIds.Add(Id);
                                }
                            }
                        }

                        //list of recommendations coupons 
                        if (request.Offset == 0)
                            recommendationsAccounts = recommendationsResponse.Result;
                    }

                }

                coupons = await getAllOffersSFMC(campaignIds, unLoadedAccounts, recommendationsAccounts, request.Offset, request.Limit, BannerPartnerCode, (SEG.ApiService.Models.Banner)request.ChainId, request.StoreId, walletCampaignIds, request.AlcoholOffersAccepted = false).ConfigureAwait(false);

                if (coupons != null)
                    coupons = coupons.Take(request.Limit).ToList();

                //get issuance count
                foreach (var i in coupons)
                {
                    RedisValue redisValue = getSingleKeyFromStoreDatabase(i.CouponID);
                    if (!redisValue.IsNullOrEmpty)
                    {
                        i.TotalIssuanceConsumed = Serializer.JsonDeserialize<string>(redisValue);

                    }
                    else
                    {
                        i.TotalIssuanceConsumed = await TotalIssuanceConsumed(i.CouponID).ConfigureAwait(false);
                    }
                }

                result.Offset = request.Offset;
                result.Limit = request.Limit;
                result.Coupons = coupons;
                result.Total = coupons.Count();
                result.StatusCode = System.Net.HttpStatusCode.OK;

                //add get all offers cache 
                addToCacheDatabase1(request.MemberId, result, cacheKey);

            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }


        private async Task<WalletCouponsCacheResponse> getAllWalletAccountOffersSFMC(GetAllCampaignsCacheRequest request, string BannerPartnerCode, string cacheKey)
        {

            GetWalletAccountsRequest walletAccountRequest = new GetWalletAccountsRequest();
            WalletCouponsCacheResponse result = new WalletCouponsCacheResponse();
            Shared.Response<GetWalletAccountsResponse> response = null;
            List<Coupon> coupons = null;
            int totalCount = 0;
            List<Account> accountsList = new List<Account>();


            try
            {

                //getwalletAccountsusing identy value
                walletAccountRequest.MemberId = request.MemberId;
                walletAccountRequest.Limit = 100;
                walletAccountRequest.Offset = 0;
                //get  loaded or unloaded offers 
                walletAccountRequest.State = StateType.ZEMBULA.ToString();

                do
                {
                    //wallet accounts 
                    response = await serviceDAL.GetWalletAccountsByIdentityValue(walletAccountRequest).ConfigureAwait(false);
                    if (response != null && response.IsSuccessful && response.Result != null && response.Result.Results != null && response.Result.Results.Count > 0)
                    {

                        accountsList.AddRange(response.Result.Results);
                        totalCount = response.Result.Total;
                        if (accountsList.Count != response.Result.Total)
                        {
                            walletAccountRequest.Limit = 100;
                            walletAccountRequest.Offset = walletAccountRequest.Offset + 100;
                        }
                    }
                }
                while (accountsList.Count != totalCount);

                if (accountsList != null && accountsList.Count > 0)
                {
                    coupons = getCampaignsFromCacheAccounts(accountsList, BannerPartnerCode, request.StoreId, (ApiService.Models.Banner)request.ChainId);
                    if (coupons != null && coupons.Count > 0)
                    {
                        coupons.Select(a => a.PromoTags.Contains(PromoTagType.Zembula.ToString()));

                        if (coupons != null && coupons.Count > request.Limit)
                            coupons = coupons.Take(request.Limit).ToList();

                        //get issuance count
                        foreach (var i in coupons)
                        {
                            RedisValue redisValue = getSingleKeyFromStoreDatabase(i.CouponID);
                            if (!redisValue.IsNullOrEmpty)
                            {
                                i.TotalIssuanceConsumed = Serializer.JsonDeserialize<string>(redisValue);

                            }
                            else
                            {
                                i.TotalIssuanceConsumed = await TotalIssuanceConsumed(i.CouponID).ConfigureAwait(false);
                            }
                        }
                    }
                }

                result.Offset = request.Offset;
                result.Limit = request.Limit;
                result.Coupons = coupons;
                result.Total = coupons.Count();
                result.StatusCode = System.Net.HttpStatusCode.OK;

                //add get all offers cache 
                addToCacheDatabase1(request.MemberId, result, cacheKey);

            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        private List<Coupon> getCampaignsFromCacheAccounts(List<Account> accounts, string bannerPartnerCode, string storeID, ApiService.Models.Banner banner, bool alcoholOffersAccepted = false)
        {
            List<Coupon> finalAccountsList = new List<Coupon>();

            if (accounts != null && accounts.Count() > 0)
            {
                foreach (Account account in accounts)
                {
                    if (account != null && !string.IsNullOrEmpty(account.CampaignId))
                    {
                        RedisValue redisValue = getSingleCouponFromCache(account.CampaignId);
                        if (!redisValue.IsNullOrEmpty)
                        {

                            Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                            coupon.AccountID = account.AccountId;
                            coupon.CouponEndDate = account.Dates.end.ToString();
                            coupon.CouponBeginDate = account.Dates.start.ToString();
                            coupon.CouponFixedExpiryDate = null;
                            coupon.Status = account.Status;
                            coupon.State = account.State;

                            filterCouponsDataAccounts(coupon, bannerPartnerCode, banner, storeID, finalAccountsList, false, alcoholOffersAccepted);

                        }
                    }
                }
            }
            return finalAccountsList;
        }

        private async Task<WalletCouponsCacheResponse> getAllOffers(List<string> campaignIds, List<Account> unloadedCampaigns, List<WalletRecommendations> recommendationsCampaigns, int offSet, int limit, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<string> walletCampaignIDs, bool alcoholOffersAccepted)
        {
            WalletCouponsCacheResponse walletCouponsCacheResponse = new WalletCouponsCacheResponse();
            List<Coupon> allCoupons = null;
            RedisKey[] redisKeys = null;

            try
            {
                //get keys in cache : 
                _cache = RedisConnectorHelper.Connection.GetDatabase();



                //get Data from cosmos DB . 

                redisKeys = await GetAllOffersDB().ConfigureAwait(false);

                if (redisKeys != null && redisKeys.Count() > 0)
                {
                    if (campaignIds != null && campaignIds.Count() > 0)
                    {
                        //remove accounts from final key list 
                        RedisKey[] finalKeys = redisKeys.Where(x => !campaignIds.Contains(x)).ToArray();

                        //get values from cache 
                        RedisValue[] result = await _cache.StringGetAsync(finalKeys).ConfigureAwait(false);
                        allCoupons = result.Where(a => a.HasValue).Select(a => Serializer.JsonDeserialize<Coupon>(a)).ToList();

                    }
                    else
                    {
                        //get values from cache 
                        RedisValue[] result = await _cache.StringGetAsync(redisKeys).ConfigureAwait(false);
                        allCoupons = result.Where(a => a.HasValue).Select(a => Serializer.JsonDeserialize<Coupon>(a)).ToList();
                    }
                }

                walletCouponsCacheResponse.Coupons = combineCouponUnloadedDataRecommendations(allCoupons, bannerPartnerCode, banner, storeID, unloadedCampaigns, recommendationsCampaigns, walletCampaignIDs, alcoholOffersAccepted);

                return walletCouponsCacheResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<List<Coupon>> getAllOffersSFMC(List<string> campaignIds, List<Account> unloadedCampaigns, List<WalletRecommendations> recommendationsCampaigns, int offSet, int limit, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<string> walletCampaignIDs, bool alcoholOffersAccepted)
        {
            List<Coupon> allCoupons = null;
            RedisKey[] redisKeysGet = null;
            int cacheLimit = 500;
            int totalLimit = offSet + limit + cacheLimit;

            try
            {
                //get keys in cache : 
                _cache = RedisConnectorHelper.Connection.GetDatabase();



                //get Data from cosmos DB . 

                redisKeysGet = await GetAllOffersDB().ConfigureAwait(false);

                if (redisKeysGet != null && redisKeysGet.Count() > 0)
                {
                    if (campaignIds != null && campaignIds.Count() > 0)
                    {
                        //remove accounts from final key list 
                        RedisKey[] finalKeys = redisKeysGet.Where(x => !campaignIds.Contains(x)).ToArray();

                        //get values from cache 
                        RedisValue[] result = await _cache.StringGetAsync(finalKeys).ConfigureAwait(false);
                        allCoupons = result.Where(a => a.HasValue).Select(a => Serializer.JsonDeserialize<Coupon>(a)).ToList();

                    }
                    else
                    {
                        //get values from cache 
                        RedisValue[] result = await _cache.StringGetAsync(redisKeysGet).ConfigureAwait(false);
                        allCoupons = result.Where(a => a.HasValue).Select(a => Serializer.JsonDeserialize<Coupon>(a)).ToList();
                    }
                }

                return combineCouponUnloadedDataRecommendations(allCoupons, bannerPartnerCode, banner, storeID, unloadedCampaigns, recommendationsCampaigns, walletCampaignIDs, alcoholOffersAccepted);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void deleteKeyInCacheDatabase1(string cacheKey)
        {
            try
            {
                if (_cacheDatabase1 == null)
                {
                    _cacheDatabase1 = RedisConnectorHelper.Connection.GetDatabase(1);
                }
                _cacheDatabase1.KeyDelete(cacheKey);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private RedisValue getSingleCouponFromCache(string key)
        {
            if (_cache == null)
            {
                _cache = RedisConnectorHelper.Connection.GetDatabase();
            }
            return _cache.StringGet(key);
        }



        /// <summary>
        /// Get single event from Azure Cache by Event ID
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private RedisValue getSingleKeyFromCacheDatabase1(string key)
        {
            if (_cacheDatabase1 == null)
            {
                _cacheDatabase1 = RedisConnectorHelper.Connection.GetDatabase(1);
            }
            return _cacheDatabase1.StringGet(key);
        }

        private RedisValue getSingleKeyFromStoreDatabase(string key)
        {
            if (_cacheDatabase2 == null)
            {
                _cacheDatabase2 = RedisConnectorHelper.Connection.GetDatabase(2);
            }
            return _cacheDatabase2.StringGet(key);
        }


        private void addToCacheDatabase1(string walletID, WalletCouponsCacheResponse walletCouponsCacheResponse, string cacheKey)
        {
            try
            {
                if (!string.IsNullOrEmpty(walletID))
                {

                    TimeSpan ttc = new TimeSpan(0, 10, 0);
                    if (ttc.Ticks > 0)
                    {
                        if (_cacheDatabase1 == null)
                        {
                            _cacheDatabase1 = RedisConnectorHelper.Connection.GetDatabase(1);
                        }
                        _cacheDatabase1.StringSet(cacheKey, Serializer.JsonSerialize<WalletCouponsCacheResponse>(walletCouponsCacheResponse), ttc); //pass TimeSpan value here to add
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void addToCacheDatabase1(List<RedisKey> redisKeys, string cacheKey)
        {
            try
            {
                var redisKeysString = redisKeys.Select(x => (string)x).ToList();

                TimeSpan ttc = new TimeSpan(0, 60, 0);
                if (ttc.Ticks > 0)
                {
                    if (_cacheDatabase1 == null)
                    {
                        _cacheDatabase1 = RedisConnectorHelper.Connection.GetDatabase(1);
                    }

                    _cacheDatabase1.StringSet(cacheKey, Serializer.JsonSerialize<List<string>>(redisKeysString), ttc); //pass TimeSpan value here to add
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// filetering  of coupons list per coupon begin date , end date , status , inclusions and exclusions 
        /// </summary>
        /// <param name="coupons"></param>
        /// <param name="bannerPartnerCode"></param>
        /// <param name="banner"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        private List<Coupon> combineCouponUnloadedDataRecommendations(List<Coupon> coupons, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Account> unloadedCampaigns, List<WalletRecommendations> recommendationsCampaigns, List<string> walletCampaignIDs, bool alcoholOffersAccepted = false)
        {
            try
            {
                List<Coupon> finalList = new List<Coupon>();

                //recommendation campaigns
                if (recommendationsCampaigns != null && recommendationsCampaigns.Count() > 0)
                {
                    foreach (WalletRecommendations r in recommendationsCampaigns)
                    {
                        if (r != null && r.Weight != null && !string.IsNullOrEmpty(r.Target.FirstOrDefault()))
                        {

                            string Id = Regex.Replace(r.Target.FirstOrDefault(), "[^0-9]", "");
                            //check to see if recommendation is already in user wallet
                            if (walletCampaignIDs == null || walletCampaignIDs.Count() <= 0 || !walletCampaignIDs.Contains(Id))
                            {
                                RedisValue redisValue = getSingleCouponFromCache(Id);
                                if (!redisValue.IsNullOrEmpty)
                                {
                                    Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                                    if (!string.IsNullOrEmpty(r.Weight))
                                        coupon.CouponWeight = r.Weight;
                                    if (!string.IsNullOrEmpty(r.Guid))
                                        coupon.RecommendationGuid = r.Guid;
                                    if (!string.IsNullOrEmpty(r.Catalogue))
                                        coupon.CatalogueGuid = r.Catalogue;

                                    //add check to exclude any welcome offers 
                                    if (coupon.MarketingRecommendedWelcomeOffer == null || coupon.MarketingRecommendedWelcomeOffer.Value == false)
                                        filterCouponsDataAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, true, alcoholOffersAccepted);

                                }
                            }
                        }
                    }
                }

                //regular campaigns 
                if (coupons != null && coupons.Count > 0)
                {
                    foreach (Coupon i in coupons)
                    {
                        filterCouponsData(i, bannerPartnerCode, banner, storeID, finalList, alcoholOffersAccepted);
                    }
                }

                //unloaded campaigns 
                if (unloadedCampaigns != null && unloadedCampaigns.Count > 0)
                {
                    foreach (var i in unloadedCampaigns)
                    {
                        //get from cache 
                        RedisValue redisValue = getSingleCouponFromCache(i.CampaignId);
                        if (!redisValue.IsNullOrEmpty)
                        {
                            Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                            if (!string.IsNullOrEmpty(i.AccountId))
                            {
                                coupon.AccountID = i.AccountId;
                                coupon.CouponEndDate = i.Dates.end.ToString();
                                coupon.CouponBeginDate = i.Dates.start.ToString();
                                coupon.Status = i.Status;
                                coupon.CouponFixedExpiryDate = null;

                                filterCouponsDataAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, false, alcoholOffersAccepted);
                            }
                        }

                    }

                }

                return finalList;
            }
            catch (Exception e)
            {
                throw;
            }

        }


        private async Task<List<Coupon>> combineCouponUnloadedDataRecommendationsToMBO(string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Account> accountCampaigns, List<WalletRecommendations> recommendationsCampaigns, List<Account> accountsUsed, Coupon dealOfWeek, List<string> walletCampaignIDs, bool alcoholOffersAccepted = false)
        {
            try
            {
                List<Coupon> finalList = new List<Coupon>();

                int boosterCount = 0;

                int controlBoosterCount = 0;

                //loaded and  unloaded  campaigns 
                if (accountCampaigns != null && accountCampaigns.Count > 0)
                {
                    foreach (var i in accountCampaigns)
                    {
                        //get from cache 
                        RedisValue redisValue = getSingleCouponFromCache(i.CampaignId);
                        if (!redisValue.IsNullOrEmpty)
                        {
                            Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                            if (!string.IsNullOrEmpty(i.AccountId))
                            {
                                coupon.AccountID = i.AccountId;
                                coupon.CouponEndDate = i.Dates.end.ToString();
                                coupon.CouponBeginDate = i.Dates.start.ToString();
                                coupon.Status = i.Status;
                                coupon.State = i.State;
                                coupon.CouponFixedExpiryDate = null;
                                if (i.Balances != null)
                                {
                                    if (!string.IsNullOrEmpty(coupon.TotalTransactionCount))
                                        coupon.TransactionCount = i.Balances.TransactionCount.ToString();
                                    if (!string.IsNullOrEmpty(coupon.TotalTransactionSpend))
                                        coupon.TotalSpend = i.Balances.TotalSpend.ToString();
                                    if (!string.IsNullOrEmpty(coupon.TotalTransactionUnits))
                                        coupon.TotalUnits = i.Balances.TotalUnits.ToString();
                                }

                                boosterCount = filterCouponsDataAccountsMBO(coupon, bannerPartnerCode, banner, storeID, finalList, false, boosterCount, alcoholOffersAccepted);

                                //control booster
                                controlBoosterCount = setControlBooster(coupon, bannerPartnerCode, banner, storeID, finalList, false, boosterCount, alcoholOffersAccepted);
                            }
                        }
                    }
                }

                //recommendation campaigns
                if (recommendationsCampaigns != null && recommendationsCampaigns.Count() > 0)
                {
                    foreach (WalletRecommendations r in recommendationsCampaigns)
                    {
                        if (r != null && r.Weight != null && !string.IsNullOrEmpty(r.Target.FirstOrDefault()))
                        {
                            string Id = Regex.Replace(r.Target.FirstOrDefault(), "[^0-9]", "");

                            RedisValue redisValue = getSingleCouponFromCache(Id);
                            if (!redisValue.IsNullOrEmpty)
                            {
                                Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                                if (!string.IsNullOrEmpty(r.Weight))
                                    coupon.CouponWeight = r.Weight;
                                if (!string.IsNullOrEmpty(r.Guid))
                                    coupon.RecommendationGuid = r.Guid;
                                if (!string.IsNullOrEmpty(r.Catalogue))
                                    coupon.CatalogueGuid = r.Catalogue;

                                // get only boosters 
                                if ((!string.IsNullOrEmpty(coupon.CouponOfferType) && coupon.CouponOfferType == CouponType.CONTINUITY.ToString()) || (!string.IsNullOrEmpty(coupon.PowerUp) && Convert.ToBoolean(Convert.ToInt32(coupon.PowerUp.Trim())) == true))
                                {
                                    //check to see if boosters is already in user wallet
                                    if (walletCampaignIDs == null || walletCampaignIDs.Count() <= 0 || !walletCampaignIDs.Contains(Id))
                                    {
                                        boosterCount = filterCouponsDataAccountsMBO(coupon, bannerPartnerCode, banner, storeID, finalList, true, boosterCount, alcoholOffersAccepted);
                                    }
                                }
                                else
                                {

                                    boosterCount = filterCouponsDataAccountsMBO(coupon, bannerPartnerCode, banner, storeID, finalList, true, boosterCount, alcoholOffersAccepted);
                                }

                                //control booster
                                if (controlBoosterCount < MaxControlBoosterCount)
                                {
                                    //check to see if controlled  boosters is already in user wallet
                                    if (walletCampaignIDs == null || walletCampaignIDs.Count() <= 0 || !walletCampaignIDs.Contains(Id))
                                    {
                                        controlBoosterCount = setControlBooster(coupon, bannerPartnerCode, banner, storeID, finalList, false, boosterCount, alcoholOffersAccepted);
                                    }
                                }
                            }
                        }
                    }
                }

                ///add default boosters  - if no boosters are assigned 
                if (boosterCount < 2)
                {
                    boosterCount = await GetDefaultBooserts(bannerPartnerCode, banner, storeID, finalList, walletCampaignIDs, boosterCount, alcoholOffersAccepted).ConfigureAwait(false);
                }

                //add deal of the week to final List 
                if (dealOfWeek != null)
                {
                    finalList.Add(dealOfWeek);
                }


                //used campaigsn 
                if (accountsUsed != null && accountsUsed.Count > 0)
                {
                    foreach (var i in accountsUsed)
                    {
                        //consider only used coupons for last 2 weeks . 
                        if (i.LastUpdated.Value != null && i.LastUpdated.Value.ToUniversalTime().AddDays(14) >= DateTime.UtcNow)
                        {
                            //get from cache 
                            RedisValue redisValue = getSingleCouponFromCache(i.CampaignId);
                            if (!redisValue.IsNullOrEmpty)
                            {
                                Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                                if ((!string.IsNullOrEmpty(coupon.CouponOfferType) && coupon.CouponOfferType == CouponType.CONTINUITY.ToString()) || (!string.IsNullOrEmpty(coupon.PowerUp) && Convert.ToBoolean(Convert.ToInt32(coupon.PowerUp.Trim())) == true))
                                {
                                    if (!string.IsNullOrEmpty(i.AccountId))
                                    {
                                        coupon.AccountID = i.AccountId;
                                        coupon.CouponEndDate = TimeConvertionEST(i.Dates.end.ToUniversalTime()).ToString("yyyy-MM-ddTHH:mm:ss");
                                        coupon.CouponBeginDate = TimeConvertionEST(i.Dates.start.ToUniversalTime()).ToString("yyyy-MM-ddTHH:mm:ss");
                                        coupon.Status = i.Status;
                                        coupon.State = i.State;
                                        coupon.CouponFixedExpiryDate = null;
                                        if (i.LastUpdated.Value != null)
                                        {
                                            coupon.LastUpdatedDate = TimeConvertionEST(i.LastUpdated.Value.ToUniversalTime()).ToString("yyyy-MM-ddTHH:mm:ss");
                                        }
                                        if (i.Balances != null)
                                        {
                                            coupon.TransactionCount = i.Balances.TransactionCount.ToString();
                                        }

                                        finalList.Add(coupon);

                                    }
                                }
                            }
                        }
                    }
                }
                return finalList;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private void filterCouponsData(Coupon coupon, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, bool alcoholOffersAccepted)
        {
            //filter MBO , power up and continutity offers from wallet accounts 
            if (!string.IsNullOrEmpty(coupon.CouponType) && coupon.CouponType != CouponType.MBO.ToString() && !string.IsNullOrEmpty(coupon.CouponOfferType) && coupon.CouponOfferType != CouponType.CONTINUITY.ToString())
            {
                if (string.IsNullOrEmpty(coupon.PowerUp) || Convert.ToBoolean(Convert.ToInt32(coupon.PowerUp.Trim())) == false)
                {
                    bool includeCampaignFlag = true;

                    // expirydate logic for rolling expiry date  : 
                    DateTime expiredDate = RollingAndExpiryDateLogic(coupon);

                    if (!string.IsNullOrEmpty(coupon.CouponBeginDate) && !string.IsNullOrEmpty(coupon.Status))
                    {
                        //check for not a Hidden Offers 
                        if (string.IsNullOrEmpty(coupon.HiddenOffer) || Convert.ToBoolean(Convert.ToInt32(coupon.HiddenOffer.Trim())) == false)
                        {
                            // convert Coupon begin date TO EST 
                            DateTime beginDate = TimeConvertionEST(coupon.CouponBeginDate);
                            coupon.CouponBeginDate = beginDate.ToString("yyyy-MM-ddTHH:mm:ss");
                            DateTime dateTimeNowEST = TimeConvertionEST(DateTime.UtcNow);


                            //check for expiry and start dates and Active status 
                            if ((beginDate <= dateTimeNowEST) && (expiredDate >= dateTimeNowEST) && coupon.Status == StatusType.ACTIVE.ToString())
                            {
                                string category = coupon.SEGDigitalGroup.Replace(" ", "").ToLower();
                                if (alcoholOffersAccepted)
                                {
                                    BlockedCategories.Remove("alcohol");
                                }

                                if (!BlockedCategories.Exists(a => a == category))
                                {

                                    //check for banner partners 
                                    if (coupon.BannerPartnerInfo != null && coupon.BannerPartnerInfo.Count() > 0 && coupon.BannerPartnerInfo.Contains(bannerPartnerCode))
                                    {
                                        if (!string.IsNullOrEmpty(storeID))
                                            includeCampaignFlag = storeInclusionsExclusionsCheck(coupon, storeID, banner);

                                        if (includeCampaignFlag)
                                        {
                                            finalList.Add(coupon);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private int filterCouponsDataAccountsMBO(Coupon coupon, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, bool recommendationsFlag, int boosterCount, bool alcoholOffersAccepted)
        {
            //only  MBO , power up and continutity offers from recommendations and accounts 
            if (!string.IsNullOrEmpty(coupon.CouponType) && coupon.CouponType == CouponType.MBO.ToString())
            {
                dateFilterLogicAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, recommendationsFlag, alcoholOffersAccepted);
            }
            else
            if ((!string.IsNullOrEmpty(coupon.CouponOfferType) && coupon.CouponOfferType == CouponType.CONTINUITY.ToString()) || (!string.IsNullOrEmpty(coupon.PowerUp) && Convert.ToBoolean(Convert.ToInt32(coupon.PowerUp.Trim())) == true))
            {
                //check for  controlled boosters 
                if (coupon.PromoTags == null || !coupon.PromoTags.Contains(PromoTagType.ControlBooster.ToString()))
                {
                    if (boosterCount < MaxBoosterCount)
                    {
                        // only consider valid continuity offers  
                        int validBoosterCount = finalList.Count() + 1;
                        dateFilterLogicAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, recommendationsFlag, alcoholOffersAccepted);
                        if (validBoosterCount == finalList.Count())
                            boosterCount = boosterCount + 1;
                    }
                }
            }

            return boosterCount;
        }

        private void filterCouponsDataAccounts(Coupon coupon, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, bool recommendationsFlag, bool alcoholOffersAccepted)
        {
            //filter MBO , power up and continutity offers from wallet accounts and recommendations 
            if (!string.IsNullOrEmpty(coupon.CouponType) && coupon.CouponType != CouponType.MBO.ToString() && !string.IsNullOrEmpty(coupon.CouponOfferType) && coupon.CouponOfferType != CouponType.CONTINUITY.ToString())
            {
                if (string.IsNullOrEmpty(coupon.PowerUp) || Convert.ToBoolean(Convert.ToInt32(coupon.PowerUp.Trim())) == false)
                {
                    dateFilterLogicAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, recommendationsFlag, alcoholOffersAccepted);
                }
            }
        }


        private void dateFilterLogicAccounts(Coupon coupon, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, bool recommendationsFlag, bool alcoholOffersAccepted)
        {

            DateTime expiredDate = DateTime.MinValue;
            bool includeCampaignFlag = true;

            //recommendations use expiry dates logic , while coupons loaded in account dont require expiry logic .
            if (recommendationsFlag)
            {
                expiredDate = RollingAndExpiryDateLogic(coupon);
            }
            else
            {
                expiredDate = TimeConvertionEST(coupon.CouponEndDate);
                coupon.CouponEndDate = expiredDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            if (!string.IsNullOrEmpty(coupon.CouponBeginDate) && !string.IsNullOrEmpty(coupon.Status))
            {
                // convert Coupon begin date TO EST 
                DateTime startDate = TimeConvertionEST(coupon.CouponBeginDate);
                coupon.CouponBeginDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss");

                DateTime dateTimeNowEST = TimeConvertionEST(DateTime.UtcNow);
                //check for expiry and start dates and Active status 
                if ((startDate <= dateTimeNowEST) && (expiredDate >= dateTimeNowEST) && coupon.Status == StatusType.ACTIVE.ToString())
                {
                    //filterOut coupons with Alcohol, Tobacco or Family Planning
                    string category = coupon.SEGDigitalGroup.Replace(" ", "").ToLower();
                    if (alcoholOffersAccepted)
                    {
                        BlockedCategories.Remove("alcohol");
                    }
                    if (!BlockedCategories.Exists(a => a == category))
                    {
                        //check for banner partners 
                        if (coupon.BannerPartnerInfo != null && coupon.BannerPartnerInfo.Count() > 0 && coupon.BannerPartnerInfo.Contains(bannerPartnerCode))
                        {
                            if (!string.IsNullOrEmpty(storeID))
                                includeCampaignFlag = storeInclusionsExclusionsCheck(coupon, storeID, banner);

                            if (includeCampaignFlag)
                            {
                                finalList.Add(coupon);
                            }

                        }
                    }
                }
            }
        }

        private bool storeInclusionsExclusionsCheck(Coupon i, string storeID, SEG.ApiService.Models.Banner banner)
        {
            bool IncludeCampaignFlag = true;
            List<string> storeTags = null;

            if (!string.IsNullOrEmpty(storeID))
            {
                if (!string.IsNullOrEmpty(storeID))
                {
                    RedisValue WDStoreTags = getSingleKeyFromStoreDatabase(storeID);
                    if (!WDStoreTags.IsNullOrEmpty)
                    {
                        storeTags = Serializer.JsonDeserialize<List<string>>(WDStoreTags);
                    }
                }

                if (banner == SEG.ApiService.Models.Banner.WD)
                {
                    //stores
                    if (i.WdIncludedStores != null && i.WdIncludedStores.Count() > 0)
                    {
                        if (!i.WdIncludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }
                    if (i.WdExcludedStores != null && i.WdExcludedStores.Count() > 0)
                    {
                        if (i.WdExcludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }

                    //store tags 
                    if (i.WdIncludedStoreTags != null && i.WdIncludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (!storeTags.Intersect(i.WdIncludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }
                    }

                    //store tags 
                    if (i.WdExcludedStoreTags != null && i.WdExcludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (storeTags.Intersect(i.WdExcludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }
                    }

                }

                if (banner == SEG.ApiService.Models.Banner.Bilo)
                {
                    if (i.BlIncludedStores != null && i.BlIncludedStores.Count() > 0)
                    {
                        if (!i.BlIncludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }
                    if (i.BlExcludedStores != null && i.BlExcludedStores.Count() > 0)
                    {
                        if (i.BlExcludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }

                    //store tags 
                    if (i.BlIncludedStoreTags != null && i.BlIncludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (!storeTags.Intersect(i.BlIncludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }

                    }

                    //store tags 
                    if (i.BlExcludedStoreTags != null && i.BlExcludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (storeTags.Intersect(i.BlExcludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }

                    }
                }

                if (banner == SEG.ApiService.Models.Banner.Harveys)
                {
                    if (i.HvIncludedStores != null && i.HvIncludedStores.Count() > 0)
                    {
                        if (!i.HvIncludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }
                    if (i.HvExcludedStores != null && i.HvExcludedStores.Count() > 0)
                    {
                        if (i.HvExcludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }

                    //store tags 
                    if (i.HvIncludedStoreTags != null && i.HvIncludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (!storeTags.Intersect(i.HvIncludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }
                    }

                    //store tags 
                    if (i.HvExcludedStoreTags != null && i.HvExcludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (storeTags.Intersect(i.HvExcludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }
                    }
                }

                if (banner == SEG.ApiService.Models.Banner.Fresco)
                {
                    if (i.FrIncludedStores != null && i.FrIncludedStores.Count() > 0)
                    {
                        if (!i.FrIncludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }
                    if (i.FrExcludedStores != null && i.FrExcludedStores.Count() > 0)
                    {
                        if (i.FrExcludedStores.Contains(storeID))
                        {
                            IncludeCampaignFlag = false;
                        }
                    }

                    //store tags 
                    if (i.FrIncludedStoreTags != null && i.FrIncludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (!storeTags.Intersect(i.FrIncludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }

                    }

                    //store tags 
                    if (i.FrExcludedStoreTags != null && i.FrExcludedStoreTags.Count() > 0)
                    {
                        if (storeTags != null && storeTags.Count > 0)
                        {
                            if (storeTags.Intersect(i.FrExcludedStoreTags).Any())
                            {
                                IncludeCampaignFlag = false;
                            }
                        }

                    }
                }
            }
            return IncludeCampaignFlag;
        }


        private DateTime RollingAndExpiryDateLogic(Coupon coupon)
        {
            DateTime expiredDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(coupon.RollingExpiryValueType) && !string.IsNullOrEmpty(coupon.RollingExpiryValue))
            {
                if (coupon.RollingExpiryValueType == RollingExpiryValueType.DAY.ToString())
                {
                    DateTime calculatedDate = TimeConvertionEST(DateTime.UtcNow).Date.AddDays(Convert.ToInt32(coupon.RollingExpiryValue)).AddHours(23).AddMinutes(59).AddSeconds(59);

                    expiredDate = calculatedDate;
                    coupon.CouponEndDate = calculatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    coupon.CouponFixedExpiryDate = null;

                }
                if (coupon.RollingExpiryValueType == RollingExpiryValueType.HOUR.ToString())
                {
                    DateTime calculatedDate = TimeConvertionEST(DateTime.UtcNow).AddHours(Convert.ToInt32(coupon.RollingExpiryValue));
                    expiredDate = calculatedDate;
                    coupon.CouponEndDate = calculatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    coupon.CouponFixedExpiryDate = null;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(coupon.CouponFixedExpiryDate))
                {
                    if (!string.IsNullOrEmpty(coupon.CouponEndDate))
                    {
                        expiredDate = TimeConvertionEST(coupon.CouponEndDate);
                        coupon.CouponEndDate = expiredDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }
                else
                {
                    coupon.CouponEndDate = TimeConvertionEST(coupon.CouponEndDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    expiredDate = TimeConvertionEST(coupon.CouponFixedExpiryDate);//TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(coupon.CouponFixedExpiryDate), easternZone).ToString("yyyy-MM-ddTHH:mm:ss");
                    coupon.CouponFixedExpiryDate = expiredDate.ToString("yyyy-MM-ddTHH:mm:ss");

                }
            }

            return expiredDate;
        }

        private DateTime TimeConvertionEST(string dateTime)
        {
            DateTime format = DateTimeOffset.Parse(dateTime).UtcDateTime;

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(format, easternZone);
        }

        private DateTime TimeConvertionEST(DateTime dateTime)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, easternZone);
        }

        private async Task<int> GetDefaultBooserts(string bannerPartnerCode, SEG.ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, List<string> walletCampaignIDs, int boosterCount, bool alcoholOffersAccepted)
        {

            List<RedisKey> redisKeys = new List<RedisKey>();

            try
            {
                //Check cache . 
                RedisValue defaultBoosters = getSingleKeyFromCacheDatabase1(GetDefaultBoosters_CosmosDB);

                if (!defaultBoosters.IsNullOrEmpty)
                {
                    redisKeys = Serializer.JsonDeserialize<List<RedisKey>>(defaultBoosters);
                }
                else
                {
                    using (CosmosClient cosmosClient = CosmosConnectionHelper.getCosmosClient())
                    {
                        string sqlQueryText = string.Format("SELECT  distinct (c.CouponID)  from  c where  ARRAY_CONTAINS(c.PromoTags , '{0}') ORDER BY c._ts DESC ", "DefaultBooster");

                        Container mContainer = cosmosClient.GetContainer(cosmosDataBase, cosmosContainer);

                        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                        var queryResultSetIterator = mContainer.GetItemQueryIterator<Object>(queryDefinition);

                        while (queryResultSetIterator.HasMoreResults)
                        {

                            var currentResultSet = await queryResultSetIterator.ReadNextAsync().ConfigureAwait(false);
                            foreach (var a in currentResultSet)
                            {
                                Coupon o = Serializer.JsonDeserialize<Coupon>(a.ToString());
                                if (o != null && !string.IsNullOrEmpty(o.CouponID))
                                {
                                    redisKeys.Add(o.CouponID);
                                }

                            }
                        }
                    }

                    //add defult boosters to  cache 
                    if (redisKeys != null && redisKeys.Count() > 0)
                        addToCacheDatabase1(redisKeys, GetDefaultBoosters_CosmosDB);

                }


                //add default boosters 
                if (redisKeys != null && redisKeys.Count() > 0)
                {
                    foreach (var i in redisKeys)
                    {
                        if (walletCampaignIDs == null || walletCampaignIDs.Count() <= 0 || !walletCampaignIDs.Contains(i.ToString()))
                        {
                            if (boosterCount < 2)
                            {
                                if (!string.IsNullOrEmpty(i.ToString()))
                                {
                                    RedisValue redisValue = getSingleCouponFromCache(i);
                                    if (!redisValue.IsNullOrEmpty)
                                    {
                                        Coupon coupon = Serializer.JsonDeserialize<Coupon>(redisValue);
                                        boosterCount = filterCouponsDataAccountsMBO(coupon, bannerPartnerCode, banner, storeID, finalList, true, boosterCount, alcoholOffersAccepted);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }


                return boosterCount;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        private int setControlBooster(Coupon coupon, string bannerPartnerCode, ApiService.Models.Banner banner, string storeID, List<Coupon> finalList, bool recommendationsFlag, int controlBoosterCount, bool alcoholOffersAccepted)
        {
            if (coupon.PromoTags != null && coupon.PromoTags.Count > 0 && coupon.PromoTags.Contains(PromoTagType.ControlBooster.ToString()))
            {
                if (controlBoosterCount < MaxControlBoosterCount)
                {
                    // only consider valid continuity offers  
                    int validBoosterCount = finalList.Count() + 1;
                    dateFilterLogicAccounts(coupon, bannerPartnerCode, banner, storeID, finalList, recommendationsFlag, alcoholOffersAccepted);
                    if (validBoosterCount == finalList.Count())
                        controlBoosterCount = controlBoosterCount + 1;
                }
            }

            return controlBoosterCount;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponID"></param>
        /// <returns></returns>
        private async Task<string> TotalIssuanceConsumed(string couponID)
        {
            try
            {
                Int64 TotalIssuanceConsumed = await AzureLoyaltyDatabaseManager.GetCampaignIssuanceCount(couponID).ConfigureAwait(false);
                return TotalIssuanceConsumed.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<RedisKey[]> GetAllOffersDB()
        {
            List<RedisKey> redisKeys = new List<RedisKey>();

            try
            {
                //Check cache . 
                RedisValue getAllOffersCosmosDB = getSingleKeyFromCacheDatabase1(GetAllOffers_CosmosDB);

                if (!getAllOffersCosmosDB.IsNullOrEmpty)
                {
                    redisKeys = Serializer.JsonDeserialize<List<RedisKey>>(getAllOffersCosmosDB);
                }
                else
                {
                    using (CosmosClient cosmosClient = CosmosConnectionHelper.getCosmosClient())
                    {
                        string sqlQueryText = string.Format("SELECT distinct (c.CouponID)   from  c where c.HiddenOffer = 0 or c.hiddenOffer = '{0}'", "0");


                        Container mContainer = cosmosClient.GetContainer(cosmosDataBase, cosmosContainer);

                        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                        var queryResultSetIterator = mContainer.GetItemQueryIterator<Object>(queryDefinition);

                        while (queryResultSetIterator.HasMoreResults)
                        {

                            var currentResultSet = await queryResultSetIterator.ReadNextAsync().ConfigureAwait(false);
                            foreach (var a in currentResultSet)
                            {
                                Coupon o = Serializer.JsonDeserialize<Coupon>(a.ToString());
                                if (o != null && !string.IsNullOrEmpty(o.CouponID))
                                {
                                    redisKeys.Add(o.CouponID);
                                }
                            }
                        }
                    }

                    //add get all offers cache 
                    if (redisKeys != null && redisKeys.Count() > 0)
                        addToCacheDatabase1(redisKeys, GetAllOffers_CosmosDB);
                }

                return redisKeys.ToArray();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

