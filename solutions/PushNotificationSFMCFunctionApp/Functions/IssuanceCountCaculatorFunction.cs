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
    public class IssuanceCountCaculatorFunction
    {
        private TelemetryClient _telemetry;


        public IssuanceCountCaculatorFunction(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        [FunctionName("IssuanceCountCaculator")]
        public async Task<HttpResponseMessage> IssuanceCountTrigger(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "IssuanceCountCaculator")]HttpRequestMessage req, ILogger log)
        {
            //ObjectResult objectResult = null;
            List<CampaignIssuanceCountRequest> data = new List<CampaignIssuanceCountRequest>();

            try
            {
                log.LogInformation("IssuanceTrigger HTTP trigger function processed a request.");

                // Get the request body
                dynamic dataArray = await req.Content.ReadAsAsync<object>();

                if (dataArray.ToString().Length > 262144 || dataArray.Count > 100)
                {
                    return new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                }

                //log.LogInformation(string.Format("Raw request :{0}", dataArray));


                var json = JsonConvert.SerializeObject(dataArray);

                data = JsonConvert.DeserializeObject(json, typeof(List<CampaignIssuanceCountRequest>));

                log.LogInformation(string.Format("Request :{0}", JsonConvert.SerializeObject(data)));

                //string redisCampaignConnectionString = "redis-omnichannel-seg-dev.redis.cache.windows.net:6380,password=KU76o+n4a5RlEtAMgZ85zl368ymlRDuJRdGilE77GYU=,ssl=True,abortConnect=False";
                ////string redisConnectionString = config["redisConnectionString"];
                ////string dbConnectionString = "Server=tcp:tablestorageexport.database.windows.net,1433;Database=Loyalty;User ID=tablestorageadmin@tablestorageexport;Password=Admin123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                ////string dbConnectionString = config["dbConnectionString"];

                string redisCampaignConnectionString = Environment.GetEnvironmentVariable("RedisCampaignConnectionString");
                CampaignCountProcess process = new CampaignCountProcess(redisCampaignConnectionString);
                await process.issuanceCalculator(data, log).ConfigureAwait(false);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>
                {
                    ["Request"] = JsonConvert.SerializeObject(data)
                };
                _telemetry.TrackEvent("IssuanceCountCaculator");

                log.LogInformation(string.Format("Exception :{0}", e.Message));

                _telemetry.TrackException(e, properties);

                ObjectResult objectResultException = new ObjectResult(statusCode(e));
                objectResultException.StatusCode = 500;

                //return objectResultException;
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