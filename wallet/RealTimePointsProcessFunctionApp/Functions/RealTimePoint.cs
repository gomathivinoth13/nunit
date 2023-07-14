#region namespace
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SEG.EagleEyeLibrary;
using SEG.EagleEyeLibrary.Process;
using SEG.EagleEyeLibrary.Models;
using Microsoft.Extensions.Configuration;
using SalesForceLibrary.SalesForceAPIM;
using SEG.SalesForce.Models;
using System.Collections.Generic;
using System.Linq;
using RealTimePointsProcessFunctionApp.Models;
using Microsoft.Extensions.Options;
using SEG.EagleEyeLibrary.Controllers;
using System.Net.Http;
using System.Net;
using SEG.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
#endregion

namespace RealTimePointsProcessFunctionApp.Functions
{

    public class RealTimePoint
    {
        private EagleEyeProcess processEE;
        private EagleEyeService serviceEE;
        EagleEyeDAL serviceDAL;
        SalesForceAPIMService salesForceService;
        DataExtentionsRequest dataExtentionRequest = new DataExtentionsRequest();
        DataExtentionsResponse dataExtentionsResponse = null;
        List<Item> list = new List<Item>();
        Response<GetWalletAccountsResponse> pointsResult = null;
        List<Point> pointsInfo = new List<Point>();
        Item item = new Item();
        GetWalletAccountsRequest getWallet = new GetWalletAccountsRequest();



        [Function("RealTimePointsProcess")]
        public async Task<HttpResponseData> RealTimePointsProcess(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RealTimePointsProcess")] HttpRequestData req, FunctionContext context
            )
        {
            var log = context.GetLogger("RealTimePointsProcess");
            var defaultdatetime = DateTime.Parse("01/01/2000 10:00:00 AM");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);
            dynamic dataArray = JsonConvert.DeserializeObject(requestBody);
            if (dataArray == null)
            {
                log.LogInformation("Data array is null , Error Occured while  processing request");
                var responseData = Sethttpresponsedata(HttpStatusCode.OK, "Data array is null", req);
                return responseData;
            }
            if (dataArray.ToString().Length > 262144 || dataArray.Count > 100)
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                log.LogInformation(httpResponse.ToString());
                var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, httpResponse.ToString(), req);
                return responseData;
            }

            log.LogInformation(req.ToString());
            var json = JsonConvert.SerializeObject(dataArray);
            List<RealtimepointRequest> request = JsonConvert.DeserializeObject(json, typeof(List<RealtimepointRequest>));
            log.LogInformation(request.ToString());

            if (request.Count > 0)
            {
                try
                {
                    setconfiguration();
                    bool WalletIdExists = false;
                    bool IsInserted = false;
                    foreach (var requ in request)
                    {

                        if (string.IsNullOrEmpty(requ.WalletID))
                        {
                            log.LogInformation(" wallet id  required");
                            continue;
                        }
                        log.LogInformation("GetWalletAccountPoints starts for  walletid: " + requ.WalletID + "");
                        WalletIdExists = true;
                        getWallet.WalletId = requ.WalletID;

                        // retriving points with expiry date
                        pointsResult = await processEE.GetWalletAccountPoints(getWallet).ConfigureAwait(false);
                        if (pointsResult != null && pointsResult.IsSuccessful && pointsResult.Result != null && pointsResult.Result.Results != null && pointsResult.Result.Results.Count > 0)
                        {
                            log.LogInformation("points retrive from GetWalletAccountPoints successfully");

                            //retrive member id from wallet id
                            var walletInfo = await serviceEE.GetWalletIdentities(requ.WalletID).ConfigureAwait(false);

                            if (walletInfo.Result.Results == null && walletInfo.Result.Results.Count < 0)
                            {
                                log.LogInformation("Wallet info is null for " + requ.WalletID + "");
                                var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "Wallet info returned null for  +" + requ.WalletID + " +, Request was not successfull", req);
                                return responseData;
                            }

                            log.LogInformation("Wallet info returned from GetWalletIdentities for " + requ.WalletID + " ");

                            var value = walletInfo.Result.Results.Where(x => x.Type.Trim() == "MEMBER_ID").FirstOrDefault();
                            if (value == null | value.Value == null)
                            {
                                log.LogInformation("Value or member id returned  null for " + requ.WalletID + " ");
                                var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "member id returned null for " + requ.WalletID + ", Request was not successfull", req);
                                return responseData;
                            }
                            log.LogInformation("Memberid is present for the specific walletid, for " + requ.WalletID + ", and member id is: +" + value.Value + "+");

                            var r = pointsResult.Result.Results.FirstOrDefault();
                            item.MEMBER_ID = value.Value;

                            if (r.PointsInfo != null)
                            {
                                log.LogInformation("Points are present for current wallet id, for " + requ.WalletID + "");
                                Point point = r.PointsInfo.OrderBy(a => a.ValidTo).First();
                                //  item.MEMBER_ID = value.Value;
                                item.Expiring_Points = (int)point.Points;
                                item.Next_Exprn_Dt = (DateTime)point.ValidTo;
                                log.LogInformation("Expiring_points: " + item.Expiring_Points.ToString() + "Next_Exprn_Dt :" + item.Next_Exprn_Dt.ToString());
                                IsInserted = true;

                            }
                            else
                            {
                                // insert to 0 list
                                dataExtentionRequest = await InsertList(0, item.MEMBER_ID, 0,defaultdatetime);
                                dataExtentionsResponse = await insertSFMC(dataExtentionRequest);
                                log.LogInformation("Points are not present  for current wallet id, for " + requ.WalletID + "");
                                continue;
                            }
                            if (r.Balances != null)
                            {
                                log.LogInformation("Balance are present for current wallet id, for " + requ.WalletID + "");
                                item.Current_Points_Balance = r.Balances.Usable;
                                log.LogInformation("Current_point_balance :" + item.Current_Points_Balance.ToString());
                                IsInserted = true;
                            }
                            else
                            {
                                log.LogInformation("Balance are not present for current wallet id, for " + requ.WalletID + "");
                            }
                            if (!IsInserted)
                            {
                                log.LogInformation("Balance And pointArray are null for " + requ.WalletID + "");
                                continue;
                            }
                            else
                            {
                                dataExtentionRequest = await InsertList(item.Current_Points_Balance, item.MEMBER_ID, item.Expiring_Points, item.Next_Exprn_Dt);

                            }
                            //insert data to list
                            if (dataExtentionRequest.items == null)
                            {
                                log.LogInformation("dataExtentionRequest is null for " + requ.WalletID + "");
                                var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "dataExtentionRequest is null for " + requ.WalletID + ", Request was not successfull", req);
                                return responseData;

                            }
                            else
                            {
                                //push data to sfmc
                                log.LogInformation("started pushing point data to sfmc for " + requ.WalletID + "", dataExtentionRequest);
                                dataExtentionsResponse = await insertSFMC(dataExtentionRequest);
                                if (dataExtentionsResponse != null & dataExtentionsResponse.errorcode == null)
                                {
                                    log.LogInformation(" Points Added to Sfmc successfully for " + requ.WalletID + "", dataExtentionsResponse);
                                }
                                else
                                {
                                    log.LogInformation(" dataExtentionsResponse  is null or  Some Error occured while adding data to SFMC " + requ.WalletID + "");
                                    var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "dataExtentionsResponse  is null or Some Error occured while adding data to SFMC for " + requ.WalletID + ", Request was not successfull", req);
                                    return responseData;
                                }
                            }
                        }
                        else
                        {
                            //insert 0 to list
                            dataExtentionRequest = await InsertList(0, item.MEMBER_ID, 0, defaultdatetime);
                            dataExtentionsResponse = await insertSFMC(dataExtentionRequest);
                            log.LogInformation("Points are not present  for current wallet id, for " + requ.WalletID + "");
                            continue;
                          
                        }

                    }
                    if (WalletIdExists)
                    {
                        if (IsInserted)
                        {
                            var responseData = Sethttpresponsedata(HttpStatusCode.OK, "points successfullt  added to sfmc", req);
                            return responseData;
                        }
                        else
                        {
                            log.LogInformation("Point array and balance array are null no data is inserted in to database ");
                            var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "Point array and balance array are null no data is inserted in to database ", req);
                            return responseData;

                        }
                    }
                    else
                    {
                        log.LogInformation("walletID is null");
                        var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "walletID is required ", req);
                        responseData.WriteString("");
                        return responseData;

                    }
                }

                catch (Exception ex)
                {
                    log.LogError("Error occured while processing request", ex);
                    var responseData = Sethttpresponsedata(HttpStatusCode.RequestEntityTooLarge, "Some error occured", req);
                    return responseData;

                }
            }
            else
            {
                log.LogInformation("Request body is null  , Error Occured while  processing request");
                var responseData = Sethttpresponsedata(HttpStatusCode.OK, "Request body is null , Error Occured while  processing request ", req);
                return responseData;
            }

        }
        /// <summary>
        /// set comfiguration values
        /// </summary>
        private void setconfiguration()
        {
            #region config values
            salesForceService = new SalesForceAPIMService(Environment.GetEnvironmentVariable("SalesForceAPIMAuthEndPoint"),
                                                           Environment.GetEnvironmentVariable("SalesForceAPIMBaseEndPoint"),
                                                           Environment.GetEnvironmentVariable("SEG_ClientID"),
                                                           Environment.GetEnvironmentVariable("SEG_ClientSecret"),
                                                           Environment.GetEnvironmentVariable("redisConnectionString"),
                                                           Environment.GetEnvironmentVariable("OcpApimSubscriptionKey"));

            serviceDAL = new EagleEyeDAL(Environment.GetEnvironmentVariable("ClientIDEE"),
                                                             Environment.GetEnvironmentVariable("SecretEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlCampaignsEE"),
                                                             Environment.GetEnvironmentVariable("OcpApimSubscriptionKey")
                                                             );
            processEE = new EagleEyeProcess(Environment.GetEnvironmentVariable("ClientIDEE"),
                                                             Environment.GetEnvironmentVariable("SecretEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlCampaignsEE"),
                                                             Environment.GetEnvironmentVariable("CacheConnectionString"),
                                                             Environment.GetEnvironmentVariable("CosmosEndpointUri"),
                                                             Environment.GetEnvironmentVariable("CosmosPrimaryKey"),
                                                             Environment.GetEnvironmentVariable("CosmosDataBaseId"),
                                                             Environment.GetEnvironmentVariable("CosmosContainerId"),
                                                             Environment.GetEnvironmentVariable("CacheServer"),
                                                             Environment.GetEnvironmentVariable("OcpApimSubscriptionKey"),
                                                             Environment.GetEnvironmentVariable("loyaltyAzureConnection"));
            serviceEE = new EagleEyeService(Environment.GetEnvironmentVariable("ClientIDEE"),
                                                             Environment.GetEnvironmentVariable("SecretEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlEE"),
                                                             Environment.GetEnvironmentVariable("BaseUrlCampaignsEE"),
                                                             Environment.GetEnvironmentVariable("OcpApimSubscriptionKey")

             );
            #endregion

           
        }
        /// <summary>
        /// Create an httpresponse data to return
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        private HttpResponseData Sethttpresponsedata(HttpStatusCode code, string message, HttpRequestData requestData)
        {
            var responseData = requestData.CreateResponse(code);
            responseData.WriteString(message);
            return responseData;

        }
        /// <summary>
        /// Insert data to SFMC 
        /// </summary>
        /// <param name="dataExtentionRequest"></param>
        /// <returns></returns>
        private async Task<DataExtentionsResponse> insertSFMC(DataExtentionsRequest dataExtentionRequest)
        {
            DataExtentionsResponse dataExtentionsResponse = new DataExtentionsResponse();
            dataExtentionsResponse = await salesForceService.UpsertAsync(dataExtentionRequest, Environment.GetEnvironmentVariable("seg_key")).ConfigureAwait(false);
            // dataExtentionsResponse = await salesForceService.UpsertAsync(dataExtentionRequest, "SEG_All_Cust_Data").ConfigureAwait(false);
            return dataExtentionsResponse;
        }
        /// <summary>
        /// Add data to a list to save that to sfmc
        /// </summary>
        /// <param name="Current_point_balance"></param>
        /// <param name="member_id"></param>
        /// <param name="Expiring_points"></param>
        /// <param name="Next_exp_date"></param>
        /// <returns></returns>
        private Task<DataExtentionsRequest> InsertList(int Current_point_balance, string member_id, int Expiring_points, DateTime Next_exp_date)
        {                
            dataExtentionRequest.items = new List<Item>()
                                    {
                                  new Item
                                  {
                                  Current_Points_Balance = Current_point_balance,
                                  MEMBER_ID              = member_id,
                                  Expiring_Points        = Expiring_points,
                                  Next_Exprn_Dt          = Next_exp_date
                                } };
            return Task.FromResult(dataExtentionRequest);

        }
    }


}

