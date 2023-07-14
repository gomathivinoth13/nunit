using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SEG.ApiService.Models.Payload;

namespace EagleEyeFunctionApp.Controllers
{
    public class HealthCheckFunction
    {
        private ILogger _logger;

        [OpenApiOperation(operationId: "Get_GetHealthCheck", Summary = "Get health check", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ApiHealthResponse), Description = "health check response data")]
        [Function(nameof(HealthCheckFunction))]
        public async Task<HttpResponseData> GetHealthCheck([HttpTrigger(AuthorizationLevel.Function, "get", Route = "EE/healthcheck")] HttpRequestData req, FunctionContext context)
        {
            _logger = context.GetLogger<HealthCheckFunction>();
            _logger.LogInformation($"{nameof(HealthCheckFunction)} has processed a request");

            var response = req.CreateResponse(HttpStatusCode.OK);
            var healthResponse = new ApiHealthResponse
            {
                ApiStatusName = "seg_ee_api_status",
                Response = true
            };
            await response.WriteAsJsonAsync(healthResponse);
            return response;
        }
    }
}
