using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;
using System.IO;
using PushNotificationSFMCFunctionApp.Models;
using System.Text.Json;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Implementation;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace PushNotificationSFMCFunctionApp.Functions
{
    public class PushNotificationSFMCFunction
    {

        private TelemetryClient _telemetry;
        private IEagleEyeMBOIssuanceEventData _eagleEyeMBOIssuanceEventData;

        public PushNotificationSFMCFunction(IEagleEyeMBOIssuanceEventData eventDate, TelemetryClient telemetry)
        {
            _eagleEyeMBOIssuanceEventData = eventDate;
            _telemetry = telemetry;
        }

        [Function("MBONotificationTrigger")]
        public async Task<HttpResponseData> MBONotificationTrigger(
           [HttpTrigger(Microsoft.Azure.Functions.Worker.AuthorizationLevel.Anonymous, "post", Route = "MBONotificationTrigger")] HttpRequestData req, FunctionContext context)
        {
            ObjectResult objectResult = null;
            //List<EagleEyeMBOIssuanceEventData> data = null;

            EagleEyeMBOIssuanceEventData data = null;
            var log = context.GetLogger("RealTimePointsProcess");

            try
            {
                log.LogInformation("MBONotificationTrigger HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                {
                    var response = req.CreateResponse(HttpStatusCode.BadRequest);
                    response.WriteString("Recipe Ingredients request is required");
                    return response;
                }

                log.LogInformation(string.Format("Raw request :{0}", requestBody));

                data = System.Text.Json.JsonSerializer.Deserialize<EagleEyeMBOIssuanceEventData>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                log.LogInformation(string.Format("Request :{0}", JsonConvert.SerializeObject(data)));

                ProcessMBOIssuancePush process = new ProcessMBOIssuancePush(_eagleEyeMBOIssuanceEventData);

                await process.MBOIssuancePush(data, log).ConfigureAwait(false);

                var responseData = req.CreateResponse(HttpStatusCode.OK);
                responseData.WriteString("Success");
                return responseData;
            }

            catch (Exception e)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>
                {
                    ["Request"] = JsonConvert.SerializeObject(data)
                };
                _telemetry.TrackEvent("MBONotificationTrigger");

                log.LogInformation(string.Format("Exception :{0}", e.Message));

                _telemetry.TrackException(e, properties);
                var responseData = req.CreateResponse(HttpStatusCode.RequestEntityTooLarge);
                responseData.WriteString(e.ToString());
                return responseData;
            }
        }


        [Function("MBONotificationTriggerArray")]
        public async Task<HttpResponseData> MBONotificationTriggerArray(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "MBONotificationTriggerArray")] HttpRequestData req, FunctionContext context)
        {
            List<EagleEyeMBOIssuanceEventData> data = null;
            var log = context.GetLogger("RealTimePointsProcess");

            try
            {
                log.LogInformation("MBONotificationTrigger HTTP trigger function processed a request.");

                // Get the request body
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Get the request body
                dynamic dataArray = JsonConvert.DeserializeObject(requestBody);

                if (dataArray.ToString().Length > 262144 || dataArray.Count > 100)
                {
                    HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                    log.LogInformation(httpResponse.ToString());
                    return req.CreateResponse(HttpStatusCode.RequestEntityTooLarge);
                }

                //log.LogInformation(string.Format("Raw request :{0}", dataArray));


                var json = JsonConvert.SerializeObject(dataArray);

                data = JsonConvert.DeserializeObject(json, typeof(List<EagleEyeMBOIssuanceEventData>));

                log.LogInformation(string.Format("Request :{0}", JsonConvert.SerializeObject(data)));

                ProcessMBOIssuancePush process = new ProcessMBOIssuancePush(_eagleEyeMBOIssuanceEventData);

                await process.MBOIssuancePushArray(data, log).ConfigureAwait(false);

                return req.CreateResponse(HttpStatusCode.OK);
            }

            catch (Exception e)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>
                {
                    ["Request"] = JsonConvert.SerializeObject(data)
                };
                _telemetry.TrackEvent("MBONotificationTriggerArray");

                log.LogInformation(string.Format("Exception :{0}", e.Message));

                _telemetry.TrackException(e, properties);

                ObjectResult objectResultException = new ObjectResult(statusCode(e));
                objectResultException.StatusCode = 500;

                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private MBOIssuancePushFailureResponse statusCode(Exception e)
        {
            return (new MBOIssuancePushFailureResponse
            {
                ErrorDescription = e.Message,
                ErrorCode = (e.Data["ErrorCode"] != null) ? e.Data["ErrorCode"].ToString() : "5000",
            });
        }

    }
}