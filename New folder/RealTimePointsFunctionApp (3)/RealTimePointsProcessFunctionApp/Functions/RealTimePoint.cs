using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SEG.EagleEyeLibrary;
using SEG.EagleEyeLibrary.Process;
using SEG.EagleEyeLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using RealTimePointsProcessFunctionApp.Models;
using System.Net.Http;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using RealTimePointsProcessFunctionApp.Interface;
using RealTimePointsProcessFunctionApp.Constants;
namespace RealTimePointsProcessFunctionApp.Functions
{

    public class RealTimePoint
    {
        private HttpResponseData _httpResponseData;
        private readonly ISfmcService _SfmcService;
        private readonly ILogger<RealTimePoint> _log;
        private readonly EagleEyeProcess _eagleEyeProcess;
        private readonly EagleEyeService _eagleEyeService;

        public RealTimePoint(ISfmcService SfmcService, ILogger<RealTimePoint> logger, EagleEyeProcess eagleEyeProcess, EagleEyeService eagleEyeService)
        {
            _SfmcService = SfmcService ?? throw new ArgumentNullException(nameof(SfmcService));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _eagleEyeProcess = eagleEyeProcess ?? throw new ArgumentNullException(nameof(eagleEyeProcess));
            _eagleEyeService = eagleEyeService ?? throw new ArgumentNullException(nameof(eagleEyeService));
        }

        [Function("RealTimePointsProcess")]
        public async Task<HttpResponseData> RealTimePointsProcess([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RealTimePointsProcess")] HttpRequestData req,
         ILogger<RealTimePoint> logger)
        {
            try
            {
                if (req.Body.Length <= 0 || req.Body.Length > 262144)
                {
                    return SetErrorResponse(HttpStatusCode.RequestEntityTooLarge, ResponseMessage.RequestEntityTooLargeMessage, req);
                }

                var dataArray = await req.ReadFromJsonAsync<List<RealtimepointRequest>>();

                if (dataArray.Count == 0)
                {
                    return SetErrorResponse(HttpStatusCode.BadRequest, ResponseMessage.NullErrorMessage, req);
                }
                if (dataArray.Count > 100)
                {
                    return SetErrorResponse(HttpStatusCode.RequestEntityTooLarge, ResponseMessage.RequestEntityTooLargeMessage, req);
                }

                bool isSuccess = false; 

                foreach (var data in dataArray)
                {
                    if (!string.IsNullOrEmpty(data.WalletID))
                    {
                        var getWallet = new GetWalletAccountsRequest();
                        getWallet.WalletId = data.WalletID;
                        var pointsResult = await _eagleEyeProcess.GetWalletAccountPoints(getWallet);
                        if (pointsResult != null && pointsResult.IsSuccessful && pointsResult.Result != null && pointsResult.Result.Results != null && pointsResult.Result.Results.Count > 0)
                        {
                            var walletInfo = await _eagleEyeService.GetWalletIdentities(data.WalletID);
                            var memberId = walletInfo.Result.Results.FirstOrDefault(x => x.Type.Trim() == "MEMBER_ID")?.Value;
                            if (memberId != null)
                            {
                                var point = pointsResult.Result.Results.FirstOrDefault();
                                if (point != null)
                                {
                                    var dataExtentionsResponse = await _SfmcService.RealTimeDataProcess(point);
                                    if (string.IsNullOrWhiteSpace(dataExtentionsResponse.errorcode))
                                    {
                                        _log.LogWarning(ResponseMessage.SuccessMessage);
                                        isSuccess = true; 
                                        continue;
                                    }
                                    else
                                    {
                                        _log.LogWarning(ResponseMessage.ErrorMessage);
                                        _httpResponseData = SetErrorResponse(HttpStatusCode.InternalServerError, ResponseMessage.ErrorMessage, req);
                                    }
                                }
                                else
                                {
                                    _log.LogWarning(ResponseMessage.NullErrorMessage);
                                    _httpResponseData = SetErrorResponse(HttpStatusCode.InternalServerError, ResponseMessage.ErrorMessage, req);
                                }
                            }
                            else
                            {
                                _log.LogWarning(ResponseMessage.NullErrorMessage);
                                _httpResponseData = SetErrorResponse(HttpStatusCode.InternalServerError, ResponseMessage.ErrorMessage, req);
                            }
                        }
                        else
                        {
                            _log.LogWarning(ResponseMessage.NullErrorMessage);
                            _httpResponseData = SetErrorResponse(HttpStatusCode.BadRequest, ResponseMessage.ErrorMessage, req);
                        }
                    }
                    else
                    {
                        _log.LogWarning(ResponseMessage.NullErrorMessage);
                        _httpResponseData = SetErrorResponse(HttpStatusCode.BadRequest, "Invalid input data", req);
                    }
                    return _httpResponseData;
                }

                if (isSuccess)
                {
                    return SetSuccessResponse(req);
                }
                else
                {
                    return SetErrorResponse(HttpStatusCode.InternalServerError, "No insertions were successful", req);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return SetHttpResponseData(HttpStatusCode.InternalServerError, ex.Message, req);
            }
        }

        private HttpResponseData SetSuccessResponse(HttpRequestData req)
        {
            _log.LogInformation(ResponseMessage.SuccessMessage);
            return SetHttpResponseData(HttpStatusCode.OK, ResponseMessage.SuccessMessage, req);
        }

        private HttpResponseData SetErrorResponse(HttpStatusCode statusCode, string message, HttpRequestData req)
        {
            _log.LogWarning(message);
            return SetHttpResponseData(statusCode, message, req);
        }

        private HttpResponseData SetHttpResponseData(HttpStatusCode statusCode, string message, HttpRequestData req)
        {
            var httpResponseData = req.CreateResponse(statusCode);
            httpResponseData.WriteString(message);
            httpResponseData.Headers.Add("Content-Type", "text/plain");
            return httpResponseData;
        }


    }


}

