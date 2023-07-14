using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using SEG.EagleEyeLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.ApplicationInsights;
using SEG.EagleEyeLibrary;
using Microsoft.AspNetCore.Http;
using System.IO;
using PushNotificationSFMCFunctionApp.Models;
using System.Text.Json;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Implementation;
using System.Linq;
using System.Net.Http;

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

        [FunctionName("MBONotificationTrigger")]
        public async Task<IActionResult> MBONotificationTrigger(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "MBONotificationTrigger")]HttpRequest req, ILogger log)
        {
            ObjectResult objectResult = null;
            //List<EagleEyeMBOIssuanceEventData> data = null;

            EagleEyeMBOIssuanceEventData data = null;

            try
            {
                log.LogInformation("MBONotificationTrigger HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                {
                    objectResult = new ObjectResult(new MBOIssuancePushFailureResponse()
                    {
                        ErrorCode = "400 Bad Request",
                        ErrorDescription = "Recipe Ingredients request is required "
                    });

                    log.LogInformation(requestBody);
                    objectResult.StatusCode = 200;
                    return objectResult;
                }

                log.LogInformation(string.Format("Raw request :{0}", requestBody));

                data = System.Text.Json.JsonSerializer.Deserialize<EagleEyeMBOIssuanceEventData>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                log.LogInformation(string.Format("Request :{0}", JsonConvert.SerializeObject(data)));

                ProcessMBOIssuancePush process = new ProcessMBOIssuancePush(_eagleEyeMBOIssuanceEventData);

                await process.MBOIssuancePush(data, log).ConfigureAwait(false);

                objectResult = new ObjectResult(new MBOIssuancePushFailureResponse()
                {
                    ErrorCode = "Sucess ",
                    ErrorDescription = "Sucess"
                });

                objectResult.StatusCode = 200;
                return objectResult;
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

                ObjectResult objectResultException = new ObjectResult(statusCode(e));
                objectResultException.StatusCode = 413;

                return objectResultException;
            }
        }


        [FunctionName("MBONotificationTriggerArray")]
        public async Task<HttpResponseMessage> MBONotificationTriggerArray(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "MBONotificationTriggerArray")]HttpRequestMessage req, ILogger log)
        {
            List<EagleEyeMBOIssuanceEventData> data = null;

            try
            {
                log.LogInformation("MBONotificationTrigger HTTP trigger function processed a request.");

                // Get the request body
                dynamic dataArray = await req.Content.ReadAsAsync<object>();


                if (dataArray.ToString().Length > 262144 || dataArray.Count > 100)
                {
                    return new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                }

                //log.LogInformation(string.Format("Raw request :{0}", dataArray));


                var json = JsonConvert.SerializeObject(dataArray);

                data = JsonConvert.DeserializeObject(json, typeof(List<EagleEyeMBOIssuanceEventData>));

                log.LogInformation(string.Format("Request :{0}", JsonConvert.SerializeObject(data)));

                ProcessMBOIssuancePush process = new ProcessMBOIssuancePush(_eagleEyeMBOIssuanceEventData);

                await process.MBOIssuancePushArray(data, log).ConfigureAwait(false);

                return new HttpResponseMessage(HttpStatusCode.OK);
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

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
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