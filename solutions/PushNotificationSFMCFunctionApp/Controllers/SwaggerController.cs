﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;


namespace PushNotificationSFMCFunctionApp.Controllers
{
    public static class SwaggerController
    {
        //[SwaggerIgnore]
        //[FunctionName("Swagger")]
        //public static Task<HttpResponseMessage> Run(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Swagger/json")] HttpRequestMessage req,
        //    [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        //{
        //    return Task.FromResult(swashBuckleClient.CreateSwaggerDocumentResponse(req));
        //}

        //[SwaggerIgnore]
        //[FunctionName("SwaggerUi")]
        //public static Task<HttpResponseMessage> Run2(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Swagger/ui")] HttpRequestMessage req,
        //    [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        //{
        //    return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        //}
    }
}
