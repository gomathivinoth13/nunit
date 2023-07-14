using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using SEG.EagleEyeLibrary.Models;
using SEG.EagleEyeLibrary.Process;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using SEG.EagleEyeLibrary.Models.CustomerCareCenter;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace EagleEyeFunctionApp.Controllers
{
    public class EagleEyeProcessPointsController
    {
        private readonly EagleEyePointsProcess _processPoints;

        public EagleEyeProcessPointsController(EagleEyePointsProcess processPoints)
        {
            _processPoints = processPoints;
        }

        [OpenApiOperation(operationId: "Post_ProcessPointsCustomerService", Summary = "Points assignment for customer webservice", Description = "")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ProcessPointsCustomerServiceRequest), Description = "ProcessPointsCustomerServiceRequest", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PointsProcessCustomerServiceResponse), Description = "Points assignment for customer webservice")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(EagleEyeFailureResponse))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(EagleEyeBadResponse))]
        [Function("ProcessPointsCustomerService")]
        public async Task<HttpResponseData> ProcessPointsCustomerService(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "EE/ProcessPointsCustomerService")] HttpRequestData req, 
            FunctionContext context, CancellationToken cancellationToken)
        {
            var logger = context.GetLogger("ProcessPointsCustomerService");
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

                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                    await response.WriteAsJsonAsync(eagleEyeBadResponse);
                    return response;
                }

                ProcessPointsCustomerServiceRequest request = SEG.Shared.Serializer.JsonDeserialize<ProcessPointsCustomerServiceRequest>(requestBody);

                var result = await _processPoints.ProcessPointsCustomerService(request);

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
                        ErrorDescription = "empty response ProcessPointsCustomerCare"
                    };


                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                    await response.WriteAsJsonAsync(eagleEyeBadResponse);
                    return response;

                }
        }
    }
}
