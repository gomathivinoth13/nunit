using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParallelAccessApis;
using SEG.ApiService.Models.Payload;
using SEG.EagleEyeLibrary.Models;
using SEG.EagleEyeLibrary.Process;

namespace EagleEyeFunctionApp.Functions
{
    public class EEHealthCheckFunction
    {
        private ILogger _logger;
        private readonly EagleEyeProcess _processEE;

        public EEHealthCheckFunction(EagleEyeProcess processEE)
        {
            _processEE = processEE;
        }

        [OpenApiOperation(operationId: "Get_GetEEHealthCheck", Summary = "Get EE health check", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ApiHealthResponse), Description = "EE health check response data")]
        [Function(nameof(EEHealthCheckFunction))]
        public async Task<HttpResponseData> GetEEHealthCheck([HttpTrigger(AuthorizationLevel.Function, "get", Route = "EE/eehealthcheck")] HttpRequestData req, FunctionContext context)
        {
            _logger = context.GetLogger<EEHealthCheckFunction>();
            _logger.LogInformation($"{nameof(EEHealthCheckFunction)} has processed a request"); ;
            var ocpApimSubscriptionKey = Environment.GetEnvironmentVariable("OcpApimSubscriptionKey");

            try
            {
                var eeHealthCheckApis = Environment.GetEnvironmentVariable("EEHealthCheckApis");
                var apiArray = eeHealthCheckApis.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var urlArray = apiArray.Select(api => new Uri(api)).ToArray();

                var isAllHealthy = await ApiParallelProcessing.ProcessApiArrayAsync<ApiHealthResponse>(urlArray, "Response", ocpApimSubscriptionKey, logger: _logger);

                var response = isAllHealthy ? req.CreateResponse(HttpStatusCode.OK) : req.CreateResponse(HttpStatusCode.InternalServerError);
                var healthResponse = new ApiHealthResponse
                {
                    ApiStatusName = "ee_status",
                    Response = isAllHealthy
                };

                await response.WriteAsJsonAsync(healthResponse);
                return response;
            }
            catch(Exception)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                var healthResponse = new ApiHealthResponse
                {
                    ApiStatusName = "ee_status",
                    Response = false
                };

                await response.WriteAsJsonAsync(healthResponse);
                return response;
            }
        }
    }
}
