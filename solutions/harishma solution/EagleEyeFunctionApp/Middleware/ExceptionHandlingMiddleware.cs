using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using SEG.EagleEyeLibrary.Models;

namespace EagleEyeFunctionApp.Middleware
{
    internal class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger _logger;

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            _logger = context.GetLogger<ExceptionHandlingMiddleware>();

            var functionName = context.FunctionDefinition.Name;
            var httpReqData = await context.GetHttpRequestDataAsync();
            var invocationResult = context.GetInvocationResult();
            HttpResponseData? newHttpResponse = null;
            EagleEyeFailureResponse? eeResponse = null;

            try
            {
                await next(context);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation($"A cancellation token was received when processing {functionName}");

                eeResponse = statusCode(ex);
                if (httpReqData != null)
                {
                    newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.ServiceUnavailable);
                    await newHttpResponse.WriteAsJsonAsync(eeResponse, newHttpResponse.StatusCode);
                    invocationResult.Value = newHttpResponse;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected exception occurred when processing {functionName}");

                eeResponse = statusCode(ex);
                if (httpReqData != null)
                {
                    newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.InternalServerError);
                    await newHttpResponse.WriteAsJsonAsync(eeResponse, newHttpResponse.StatusCode);
                    invocationResult.Value = newHttpResponse;
                };
            }
        }

        private EagleEyeFailureResponse statusCode(Exception e)
        {
            return (new EagleEyeFailureResponse
            {
                ErrorDescription = e.Message,
                ErrorCode = (e.Data["ErrorCode"] != null) ? e.Data["ErrorCode"].ToString() : "5000",
            });
        }
    }
}
