
using Flurl;
using Newtonsoft.Json;
using SEG.EagleEyeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SEG.Shared;
using System.Web;
using Type = SEG.EagleEyeLibrary.Models.Type;

namespace SEG.EagleEyeLibrary.Controllers
{
    public class EagleEyeDAL
    {
        string clientIDEE { get; set; }
        string secretEE { get; set; }
        string baseUrlEE { get; set; }

        string baseUrlCampaignsEE { get; set; }

        string ocpApimSubscriptionKey { get; set; }
        ////// <summary>
        /// 
        /// </summary>
        public EagleEyeDAL(string clientID, string secret, string baseUrlWallet, string baseUrlCampaign, string ocpApimSubscriptionKeySecret)
        {

            clientIDEE = clientID;
            secretEE = secret;
            baseUrlCampaignsEE = baseUrlCampaign;
            baseUrlEE = baseUrlWallet;
            ocpApimSubscriptionKey = ocpApimSubscriptionKeySecret;
        }

        public async Task<Shared.Response<GetWalletAccountsTrasactionsResponse>> GetWalletTransactions(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/account/{1}/transactions", request.WalletId, request.AccountId);
                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);

                queryParams.Add("orderBy", "dateCreated,DESC");

                queryParams.Add("event%5B%5D=EXPIRY&event%5B%5D=EARN&event%5B%5D=DEBIT&event%5B%5D=SPEND&event%5B%5D", "CREDIT");

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsyncData<GetWalletAccountsTrasactionsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetWalletAccountsTrasactionsResponse>> GetYTDPointSavings(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                int year = DateTime.Now.Year;
                string firstDay = new DateTime(year, 1, 1).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"); ;

                // DateTime format = firstDay.UtcDateTime;

                string pathParameters = string.Format("/wallet/{0}/account/{1}/transactions", request.WalletId, request.AccountId);
                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);

                queryParams.Add("orderBy", "dateCreated,DESC");

                queryParams.Add("event%5B%5D=DEBIT&event%5B%5D=SPEND&lastUpdated%5Bfrom%5D", firstDay);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsyncData<GetWalletAccountsTrasactionsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccountPoints(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/accounts", request.WalletId);
                //if (request.Limit != 0)
                //    queryParams.Add("limit", request.Limit);
                //if (request.Offset != 0)
                //    queryParams.Add("offset", request.Offset);


                queryParams.Add("type", Type.POINTS.ToString());

                queryParams.Add("status", StatusType.ACTIVE.ToString());


                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetWalletAccountsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccountPointsExpiryDate(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/account/{1}/points/available", request.WalletId, request.AccountId);
                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetWalletAccountsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccounts(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/accounts", request.WalletId);
                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.UNLOADED.ToString())
                    queryParams.Add("state%5B%5D=LOADED&state%5B%5D", StateType.UNLOADED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.LOADED.ToString())
                    queryParams.Add("state%5B%5D", StateType.LOADED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.LOADED.ToString())
                    queryParams.Add("status%5B%5D", StatusType.ACTIVE.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.UNLOADED.ToString())
                    queryParams.Add("status%5B%5D=ACTIVE&status%5B%5D", StatusType.USED.ToString());

                if (!string.IsNullOrEmpty(request.CampaignId))
                    queryParams.Add("campaignId", request.CampaignId);

                queryParams.Add("orderBy", "dateCreated,DESC");

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetWalletAccountsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<Shared.Response<WalletBackEndPointsResponse>> WalletBackEndpoints(WalletBackEndPointsRequest request)
        {
            string data = null;

            Dictionary<string, object> headers = null;

            try
            {
                string pathParameters = string.Format("/services/wallet/backendpoints");
                data = String.Format("{0}{1}{2}", pathParameters, Serializer.JsonSerialize<WalletBackEndPointsRequest>(request), secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);


                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<WalletBackEndPointsResponse>(HttpMethod.Post, request, pathParameters, baseUrlEE, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<Shared.Response<GetWalletAccountsResponse>> GetWalletAccountsByIdentityValue(GetWalletAccountsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/accounts");


                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.UNLOADED.ToString())
                    queryParams.Add("state%5B%5D=LOADED&state%5B%5D", StateType.UNLOADED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.LOADED.ToString())
                    queryParams.Add("state%5B%5D", StateType.LOADED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.LOADED.ToString())
                    queryParams.Add("status%5B%5D", StatusType.ACTIVE.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.UNLOADED.ToString())
                    queryParams.Add("status%5B%5D=ACTIVE&status%5B%5D", StatusType.USED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.ZEMBULA.ToString())
                    queryParams.Add("state%5B%5D=LOADED&state%5B%5D", StateType.UNLOADED.ToString());
                if (!string.IsNullOrEmpty(request.State) && request.State == StateType.ZEMBULA.ToString())
                    queryParams.Add("status%5B%5D", StatusType.ACTIVE.ToString());


                if (!string.IsNullOrEmpty(request.CampaignId))
                    queryParams.Add("campaignId", request.CampaignId);

                queryParams.Add("identity-value", request.MemberId);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetWalletAccountsResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Wallet> GetWalletByIdentityValue(string memberId)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet");


                queryParams.Add("identity-value", memberId);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<Wallet>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response.Result;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetWalletIdentitiesResponse>> GetWalletIdentities(string WalletID)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/identities", WalletID);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetWalletIdentitiesResponse>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Shared.Response<List<WalletRecommendations>>> GetWalletRecommendations(GetWalletRecommendationsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/recommendations", request.WalletId);

                //set query params 
                queryParams.Add("status", StatusType.ACTIVE.ToString());

                queryParams.Add("channel", "APP");

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<List<WalletRecommendations>>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<List<WalletRecommendations>>> GetWalletRecommendationsIdentity(GetWalletRecommendationsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/wallet/recommendations");

                //set query params 
                queryParams.Add("identity-value", request.MemberId);
                queryParams.Add("status", StatusType.ACTIVE.ToString());

                queryParams.Add("channel", "APP");

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<List<WalletRecommendations>>(HttpMethod.Get, null, pathParameters, baseUrlEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> AcceptRecommendation(LoadWalletAccountRequest request)
        {
            string data = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/services/wallet/{0}/catalogue/{1}/recommendation/{2}/accept", request.WalletId, request.CatalogueGuid, request.RecommendationGuid);

                data = String.Format("{0}{1}{2}", pathParameters, JsonConvert.SerializeObject(new { }), secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<LoadUnloadWalletAccountResponse>(HttpMethod.Post, null, pathParameters, baseUrlEE, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> LoadWalletAccount(LoadWalletAccountRequest createCouponAccountRequest)
        {
            string data = null;
            Dictionary<string, object> headers = null;

            try
            {
                ///always set coupon to active and loaded  - when adding to wallet 
                CouponAccount couponAccount = new CouponAccount();
                couponAccount.State = StateType.LOADED.ToString();
                couponAccount.Status = StatusType.ACTIVE.ToString();

                if (!string.IsNullOrEmpty(createCouponAccountRequest.ActionType))
                {
                    Details details = new Details();
                    details.ActionType = createCouponAccountRequest.ActionType;

                    couponAccount.Details = details;
                }

                string pathParameters = string.Format("/wallet/{0}/campaign/{1}/account", createCouponAccountRequest.WalletId, createCouponAccountRequest.CampaignId);

                data = String.Format("{0}{1}{2}", pathParameters, Serializer.JsonSerialize<CouponAccount>(couponAccount), secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<LoadUnloadWalletAccountResponse>(HttpMethod.Post, couponAccount, pathParameters, baseUrlEE, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch
            {
                throw;
            }
        }


        public async Task<Shared.Response<LoadUnloadWalletAccountResponse>> SetWalletAccountState(SetWalletAccountStateRequest request)
        {
            string data = null;
            Dictionary<string, object> headers = null;
            SetWalletAccountState setState = new SetWalletAccountState();

            try
            {
                string pathParameters = string.Format("/wallet/{0}/account/{1}/state", request.WalletId, request.AccountId);

                setState.State = request.State;

                data = String.Format("{0}{1}{2}", pathParameters, Serializer.JsonSerialize<SetWalletAccountState>(setState), secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsyncPatch<LoadUnloadWalletAccountResponse>(setState, pathParameters, baseUrlEE, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch
            {
                throw;
            }
        }


        public async Task<Shared.Response<GetCampaignsResponse>> GetCampaigns(GetCampaignsRequest request)
        {
            string data = null;
            string url = null;
            Dictionary<string, object> headers = null;
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            try
            {
                string pathParameters = string.Format("/campaigns");
                if (request.Limit != 0)
                    queryParams.Add("limit", request.Limit);
                if (request.Offset != 0)
                    queryParams.Add("offset", request.Offset);
                queryParams.Add("orderBy", request.OrderBy);
                queryParams.Add("status", request.Status);

                url = pathParameters.SetQueryParams(queryParams, Flurl.NullValueHandling.Remove);

                data = String.Format("{0}{1}", url, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<GetCampaignsResponse>(HttpMethod.Get, null, pathParameters, baseUrlCampaignsEE, QueryParams: queryParams, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch
            {
                throw;
            }
        }


        public async Task<Shared.Response<Campaign>> GetCampaignsByCampaignId(string campaignId)
        {
            string data = null;
            Dictionary<string, object> headers = null;


            try
            {
                string pathParameters = string.Format("/campaigns/{0}", campaignId);

                data = String.Format("{0}{1}", pathParameters, secretEE);

                headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE, ocpApimSubscriptionKey);

                var response = (await SEG.Shared.ApiUtility.RestfulCallAsync<Campaign>(HttpMethod.Get, null, pathParameters, baseUrlCampaignsEE, Headers: headers).ConfigureAwait(false));

                return response;
            }
            catch
            {
                throw;
            }
        }


        //public async Task<Shared.Response<InActivateWalletAccountResponse>> InActivateWalletAccount(CancelWalletAccountRequest cancelWalletAccountRequest)
        //{
        //    string data = null;
        //    Dictionary<string, object> headers = null;


        //    try
        //    {
        //        string pathParameters = string.Format("/wallet/{0}/account/{1}/inactivate", cancelWalletAccountRequest.WalletId, cancelWalletAccountRequest.AccountId);

        //        data = String.Format("{0}{1}{2}", pathParameters, JsonConvert.SerializeObject(new { }), secretEE);

        //        headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE);

        //        var response = (await SEG.Shared.ApiUtility.RestfulCallAsyncPatch<InActivateWalletAccountResponse>(null, pathParameters, baseUrlCampaignsEE, Headers: headers).ConfigureAwait(false));

        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}


        //public async Task<Shared.Response<CancelWalletAccountResponse>> CancelWalletAccount(CancelWalletAccountRequest cancelWalletAccountRequest)
        //{
        //    string data = null;
        //    Dictionary<string, object> headers = null;


        //    try
        //    {
        //        string pathParameters = string.Format("/wallet/{0}/account/{1}/cancel", cancelWalletAccountRequest.WalletId, cancelWalletAccountRequest.AccountId);

        //        data = String.Format("{0}{1}{2}", pathParameters, JsonConvert.SerializeObject(new { }), secretEE);

        //        headers = Utility.AddRequestHeaders(data, clientIDEE, secretEE);

        //        var response = (await SEG.Shared.ApiUtility.RestfulCallAsyncPatch<CancelWalletAccountResponse>(null, pathParameters, baseUrlCampaignsEE, Headers: headers).ConfigureAwait(false));

        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        private string jsonSerialize(CouponAccount couponAccount)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
            serializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            return JsonConvert.SerializeObject(couponAccount, serializerSettings);
        }

    }
}
