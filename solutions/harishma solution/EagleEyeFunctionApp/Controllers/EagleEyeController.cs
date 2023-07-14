using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using SEG.EagleEyeLibrary;
using SEG.EagleEyeLibrary.Models;
using SEG.EagleEyeLibrary.Process;
using System;
using System.IO;
using System.Threading.Tasks;
using SEG.Shared;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

namespace EagleEyeFunctionApp.Controllers
{
    public class EagleEyeController
    {
        private readonly EagleEyeService _serviceEE;
        private readonly EagleEyeProcess _processEE;

        string WDPartnerCode { get; init; } = Environment.GetEnvironmentVariable("WDPartnerCode");
        string BiloPartnerCode { get; init; } = Environment.GetEnvironmentVariable("BiloPartnerCode");
        string HarveysPartnerCode { get; init; } = Environment.GetEnvironmentVariable("HarveysPartnerCode");
        string FrescoPartnerCode { get; init; } = Environment.GetEnvironmentVariable("FrescoPartnerCode");

        public EagleEyeController(EagleEyeProcess processEE, EagleEyeService serviceEE)
        {
            _processEE = processEE;
            _serviceEE = serviceEE;
        }

        [OpenApiOperation(operationId: "Post_WalletBackEndpoints", Summary = "Add points to customer account", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WalletBackEndPointsRequest), Description = "WalletBackEndPointsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletBackEndPointsResponse), Description = "customer points information")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]
        [Function("WalletBackEndpoints")]
        public async Task<HttpResponseData> WalletBackEndpoints([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/WalletBackEndpoints")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("WalletBackEndpoints");
            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            logger.LogInformation($"wallet BackEndPoints process started ");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            WalletBackEndPointsRequest request = SEG.Shared.Serializer.JsonDeserialize<WalletBackEndPointsRequest>(requestBody);

            if (request == null || String.IsNullOrEmpty(request.WalletId) || request.PointsValue == 0 || string.IsNullOrEmpty(request.ReasonCode) || string.IsNullOrEmpty(request.SchemeReference))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "WalletID is required , POints value cannot be null or 0 ,reasonCode is required , SchemeReference is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            if (request.Location == null || String.IsNullOrEmpty(request.Location.StoreId) || string.IsNullOrEmpty(request.WalletTransactionDescription) || string.IsNullOrEmpty(request.WalletTransactionState) || string.IsNullOrEmpty(request.WalletTransactionType))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Location is required , StoreId required , WalletTransactionDescription required , WalletTransactionState required , WalletTransactionType required  "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }


            var result = await _serviceEE.WalletBackEndpoints(request);

            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response WalletBackEndpoints"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }

        /// <summary>
        /// --get all recommendations from azure 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OpenApiOperation(operationId: "Post_GetWalletRecommendations", Summary = "retuns available recommended campiagns ", Description = "api response returns recommendations")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetWalletRecommendationsRequest), Description = "GetWalletRecommendationsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "list of available  recommendations")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetWalletRecommendations")]
        public async Task<HttpResponseData> GetWalletRecommendations([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetWalletRecommendations")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetWalletRecommendations");
            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetWalletRecommendationsRequest request = SEG.Shared.Serializer.JsonDeserialize<GetWalletRecommendationsRequest>(requestBody);


            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.WalletId) || String.IsNullOrEmpty(request.MemberId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required  , WalletID is required , MemberID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetWalletRecommendationsCache(request, getBannerPartnerCode(request.ChainId)).ConfigureAwait(false);

            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetAllOffersCache"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        [OpenApiOperation(operationId: "Post_LoadWalletAccount", Summary = "activates and add campaign to customer wallet", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoadWalletAccountRequest), Description = "LoadWalletAccountRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoadUnloadWalletAccountResponse), Description = "sucess response for activated campaign in customer wallet")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]
        [Function("LoadWalletAccount")]
        public async Task<HttpResponseData> LoadWalletAccount([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/LoadWalletAccount")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("LoadWalletAccount");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            LoadWalletAccountRequest request = SEG.Shared.Serializer.JsonDeserialize<LoadWalletAccountRequest>(requestBody);

            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.CampaignId) || String.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required , CampaignID is Required , WalletID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            Response<LoadUnloadWalletAccountResponse> result = await _processEE.LoadWalletAccount(request);

            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response LoadWalletAccount"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        [OpenApiOperation(operationId: "Post_AcceptRecommendation", Summary = "activates and add recommended campaign to customer wallet", Description = "RecommendationGuid and CatalogueGuid are required for recommendations")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoadWalletAccountRequest), Description = "LoadWalletAccountRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoadUnloadWalletAccountResponse), Description = "success response for activated recommendation")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("AcceptRecommendation")]
        public async Task<HttpResponseData> AcceptRecommendation([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/AcceptRecommendation")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("AcceptRecommendation");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            LoadWalletAccountRequest request = SEG.Shared.Serializer.JsonDeserialize<LoadWalletAccountRequest>(requestBody);



            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.WalletId) || String.IsNullOrEmpty(request.MemberId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required  , WalletID is required , MemberID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            if (request == null || string.IsNullOrEmpty(request.RecommendationGuid) || string.IsNullOrEmpty(request.CatalogueGuid))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "RecommendationGuid is required  , CatalogueGuid is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            Response<LoadUnloadWalletAccountResponse> result = await _processEE.AcceptRecommendation(request);

            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response AcceptRecommendation"
                };


                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        /// --get all offers from azure redis cache 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OpenApiOperation(operationId: "Post_GetAllOffersCache", Summary = "retuns available campaigns , unloaded accounts in customers wallet , available recommended campiagns ", Description = "api response is a combination of regular campiagns, recommendations and unloaded accounts")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetAllCampaignsCacheRequest), Description = "GetAllCampaignsCacheRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "list of available campiagns, unloaded accounts , recommendations")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetAllOffersCache")]
        public async Task<HttpResponseData> GetAllOffersCache([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetAllOffersCache")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetAllOffersCache");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetAllCampaignsCacheRequest request = Serializer.JsonDeserialize<GetAllCampaignsCacheRequest>(requestBody);


            if (request == null || request.ChainId == 0)
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null ,Chain ID is reuired"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetAllOffersCache(request, getBannerPartnerCode(request.ChainId));

            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetAllOffersCache"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }




        /// <summary>
        /// --get all MBO  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetAllMBOCache", Summary = "retunrs all available MBO , activated and unloaded MBO , Boosters and deal of the week", Description = "MBO , booster and deal of the week")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetAllCampaignsCacheRequest), Description = "GetAllCampaignsCacheRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "list of MBO , boosters and deal of the week")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetAllMBOCache")]
        public async Task<HttpResponseData> GetAllMBOCache([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetAllMBOCache")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetAllMBOCache");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetAllCampaignsCacheRequest request = SEG.Shared.Serializer.JsonDeserialize<GetAllCampaignsCacheRequest>(requestBody);


            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.WalletId) || String.IsNullOrEmpty(request.MemberId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required  , WalletID is required , MemberID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetAllMBOCache(request, getBannerPartnerCode(request.ChainId), request.ChainId.ToString());

            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetAllMBOCache"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }



        /// <summary>
        /// --get all offers from azure redis cache 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetAllOffersCacheSFMC", Summary = "Sales force marketing cloud avaialble campaigns for customer implemenation", Description = "specifically designed for sales force trageted customers")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetAllCampaignsCacheRequest), Description = "GetAllCampaignsCacheRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "list of available campiagns, unloaded accounts , recommendations")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]
        [Function("GetAllOffersCacheSFMC")]
        public async Task<HttpResponseData> GetAllOffersCacheSFMC([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetAllOffersCacheSFMC")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetAllOffersCacheSFMC");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetAllCampaignsCacheRequest request = SEG.Shared.Serializer.JsonDeserialize<GetAllCampaignsCacheRequest>(requestBody);

            if (request == null || request.ChainId == 0 || string.IsNullOrEmpty(request.MemberId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null , ChainID is required , MemberID is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }


            var result = await _processEE.GetAllOffersCacheSFMC(request, getBannerPartnerCode(request.ChainId));

            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetAllOffersCacheSFMC"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }




        /// <summary>
        /// --get all offers from azure redis cache 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetWalletAccountsCacheSFMC", Summary = "sales for marketing Funactionality - returns both loaded and unloaded accounts in customer wallet", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetAllCampaignsCacheRequest), Description = "GetAllCampaignsCacheRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "loaded and unloaded accounts in customer wallet")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]


        [Function("GetWalletAccountsCacheSFMC")]
        public async Task<HttpResponseData> GetWalletAccountsCacheSFMC([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetWalletAccountsCacheSFMC")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetWalletAccountsCacheSFMC");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetAllCampaignsCacheRequest request = SEG.Shared.Serializer.JsonDeserialize<GetAllCampaignsCacheRequest>(requestBody);

            if (request == null || request.ChainId == 0 || string.IsNullOrEmpty(request.MemberId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null , ChainID is required , MemberID is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }


            var result = await _processEE.GetWalletAccountsCacheSFMC(request, getBannerPartnerCode(request.ChainId));

            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetWalletAccountsCacheSFMC"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        [OpenApiOperation(operationId: "Post_UnLoadWalletAccount", Summary = "unload account from customer wallet - deactive a campaign in customer wallet", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SetWalletAccountStateRequest), Description = "SetWalletAccountStateRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoadUnloadWalletAccountResponse), Description = "sucess response for unloaded account")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("UnLoadWalletAccount")]
        public async Task<HttpResponseData> UnLoadWalletAccount([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/UnLoadWalletAccount")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("UnLoadWalletAccount");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            SetWalletAccountStateRequest request = SEG.Shared.Serializer.JsonDeserialize<SetWalletAccountStateRequest>(requestBody);

            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.AccountId) || String.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required , AccountId is Required , WalletID is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
            var result = await _processEE.UnLoadedWalletAccount(request);

            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response UnLoadedWalletAccount"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_SetWalletAccountStateToLoaded", Summary = "set account from unloaded to loaded in customer wallet", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SetWalletAccountStateRequest), Description = "SetWalletAccountStateRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoadUnloadWalletAccountResponse), Description = "move state from unloaded to loaded on customer account")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("SetWalletAccountStateToLoaded")]
        public async Task<HttpResponseData> SetWalletAccountStateToLoaded([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/SetWalletAccountStateToLoaded")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("SetWalletAccountStateToLoaded");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            SetWalletAccountStateRequest request = SEG.Shared.Serializer.JsonDeserialize<SetWalletAccountStateRequest>(requestBody);


            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.AccountId) || String.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required , AccountId is Required , WalletID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
            var result = await _processEE.SetWalletAccountState(request);

            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response SetWalletAccountState"
                };


                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        //// event hub and azure cache coupon data ( get accounts ) 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetWalletAccountsCache", Summary = "get all active accounts in customer wallet", Description = "retrun active loaded campaigns in customer wallet")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetWalletAccountsRequest), Description = "GetWalletAccountsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WalletCouponsCacheResponse), Description = "retrun active loaded campaigns in customer wallet")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]
        [Function("GetWalletAccountsCache")]
        public async Task<HttpResponseData> GetWalletAccountsCache([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetWalletAccountsCache")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetWalletAccountsCache");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetWalletAccountsRequest request = SEG.Shared.Serializer.JsonDeserialize<GetWalletAccountsRequest>(requestBody);

            if (request == null || request.ChainId == 0 || String.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "ChainID is required , WalletID is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetWalletAccountsCache(request, getBannerPartnerCode(request.ChainId)).ConfigureAwait(false);
            if (result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response GetWalletAccountsCache"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Get_GetCampaignIdCache", Summary = "get campaign information", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Coupon), Description = "campaign details")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetCampaignIdCache")]
        public async Task<HttpResponseData> GetCampaignIdCache([HttpTrigger(AuthorizationLevel.Function, "get", Route = "EE/GetCampaignIdCache")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetCampaignIdCache");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(req.Url.Query);

            string campaignId = queryDictionary["campaignId"];

            if (String.IsNullOrEmpty(campaignId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "campaignId is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = _processEE.GetCampaignIdFromCache(campaignId);
            if (result != null)
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "campaignId does not exits in azure cache "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetWalletTransactions", Summary = "points transactions", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetWalletAccountsRequest), Description = "GetWalletAccountsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetWalletAccountsTrasactionsResponse), Description = "points transactions")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetWalletTransactions")]
        public async Task<HttpResponseData> GetWalletTransactions([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetWalletTransactions")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetWalletTransactions");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };
                return await createResponse(req, eagleEyeBadResponse);
            }

            GetWalletAccountsRequest request = SEG.Shared.Serializer.JsonDeserialize<GetWalletAccountsRequest>(requestBody);


            if (request == null || string.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null , WalletID is required"
                };
                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetWalletTransactions(request).ConfigureAwait(false);
            if (result != null && result.Result != null)
            {
                var storeLocatorDbConnectionString = Environment.GetEnvironmentVariable("SLLMongoDbConnection").ToString();
                var storeLocatorRedisConnection = Environment.GetEnvironmentVariable("SLLRedisCacheConnection").ToString();
                result.Result = await _processEE.UpdateStoreAddresses(
                        result.Result,
                        storeLocatorDbConnectionString,
                        storeLocatorRedisConnection).ConfigureAwait(false);

                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response on getWalletTransactions"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Post_GetYTDPointSavings", Summary = "YTD points savings", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetWalletAccountsRequest), Description = "GetWalletAccountsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetYTDPointSavingsResponse), Description = "points savings")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetYTDPointSavings")]
        public async Task<HttpResponseData> GetYTDPointSavings([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetYTDPointSavings")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetYTDPointSavings");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request is required "
                };
                return await createResponse(req, eagleEyeBadResponse);
            }

            GetWalletAccountsRequest request = SEG.Shared.Serializer.JsonDeserialize<GetWalletAccountsRequest>(requestBody);


            if (request == null || string.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null , WalletID is required"
                };
                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetYTDPointSavings(request).ConfigureAwait(false);
            if (result != null)
            {

                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response on YTDPointSavingsResponse"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        [OpenApiOperation(operationId: "Get_GetWalletAccountPoints", Summary = "returns points information , points expired date", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetWalletAccountsRequest), Description = "GetWalletAccountsRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetWalletAccountsResponse), Description = "points information , points expired date")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]

        [Function("GetWalletAccountPoints")]
        public async Task<HttpResponseData> GetWalletAccountPoints([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/GetWalletAccountPoints")] 
        HttpRequestData req, FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("GetWalletAccountPoints");

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseData response = null;
            EagleEyeBadResponse eagleEyeBadResponse = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "List request is required "
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            GetWalletAccountsRequest request = SEG.Shared.Serializer.JsonDeserialize<GetWalletAccountsRequest>(requestBody);

            if (request == null || string.IsNullOrEmpty(request.WalletId))
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 Bad Request",
                    ErrorDescription = "Request cannot be null , WalletID is required"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }

            var result = await _processEE.GetWalletAccountPoints(request).ConfigureAwait(false);
            if (result != null && result.Result != null)
            {
                response = req.CreateResponse(result.StatusCode);
                await response.WriteAsJsonAsync(result.Result);
                return response;
            }
            else
            {
                eagleEyeBadResponse = new EagleEyeBadResponse()
                {
                    ErrorCode = "400 ",
                    ErrorDescription = "empty response on GetWalletAccountPoints"
                };

                return await createResponse(req, eagleEyeBadResponse);
            }
        }

        private string getBannerPartnerCode(int chainId)
        {
            switch (chainId)
            {
                case 1:
                    return WDPartnerCode;
                case 2:
                    return BiloPartnerCode;
                case 3:
                    return HarveysPartnerCode;
                case 4:
                    return FrescoPartnerCode;
                default:
                    return WDPartnerCode;
            }
        }

        private async Task<HttpResponseData> createResponse(HttpRequestData req, EagleEyeBadResponse eagleEyeBadResponse)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteAsJsonAsync(eagleEyeBadResponse);
            return response;
        }

    }
}
