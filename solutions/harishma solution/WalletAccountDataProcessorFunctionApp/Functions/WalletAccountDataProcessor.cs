using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace WalletAccountDataProcessorFunctionApp
{
    public class WalletAccountDataProcessor
    {
        private readonly ISfmcRepo _sfmcRepo;
        private readonly ILogger<WalletAccountDataProcessor> _log;
        private readonly IAccountLogRepository _process;



        public WalletAccountDataProcessor(ISfmcRepo sfmcRepo, ILogger<WalletAccountDataProcessor> logger, IAccountLogRepository processAccountIdData)
        {
            _sfmcRepo = sfmcRepo ?? throw new ArgumentNullException(nameof(sfmcRepo));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _process = processAccountIdData ?? throw new ArgumentNullException(nameof(processAccountIdData));

        }
        [Function(nameof(WalletAccountDataProcessor))]
        public async Task<HttpResponseData> WalletAccountDataProcess([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            try
            {

                if (req.Body.Length <= 0 || req.Body.Length > 262144)
                {
                    return SetErrorResponse(HttpStatusCode.RequestEntityTooLarge, ResponseMessage.RequestEntityTooLargeMessage, req);
                }

                var dataArray = await req.ReadFromJsonAsync<List<WalletAccountIDEventData>>();
                if (dataArray.Count == 0)
                {
                    return SetErrorResponse(HttpStatusCode.BadRequest, ResponseMessage.NullErrorMessage, req);
                }
                if (dataArray.Count > 100)
                {
                    return SetErrorResponse(HttpStatusCode.RequestEntityTooLarge, ResponseMessage.RequestEntityTooLargeMessage, req);
                }

                await _process.InsertWalletAccountLog(dataArray);
                var dataExtensionsResponse = await _sfmcRepo.InsertToSFMC(dataArray);

                if (!string.IsNullOrWhiteSpace(dataExtensionsResponse.errorcode))
                {
                    return SetErrorResponse(HttpStatusCode.InternalServerError, ResponseMessage.ErrorMessage, req);
                }

                return SetHttpResponseData(HttpStatusCode.OK, ResponseMessage.SuccessMessage, req);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                return SetHttpResponseData(HttpStatusCode.InternalServerError, ResponseMessage.ErrorMessage, req);
            }
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
