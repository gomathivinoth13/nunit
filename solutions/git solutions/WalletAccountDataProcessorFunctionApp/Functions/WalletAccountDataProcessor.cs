using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SEG.SalesForce.Models;
using SalesForceLibrary.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;
using System.Collections.Generic;
using System.IO;

namespace WalletAccountDataProcessorFunctionApp
{
    public class WalletAccountDataProcessor
    {
        #region Variables
        WalletAccountDataModel walletAccountDataModel = new WalletAccountDataModel();
        DataExtentionsResponse dataExtentionsResponse = new DataExtentionsResponse();
        private readonly ISfmchelper _sfmchelper;
        private readonly ISetResponse _config;
        private HttpResponseData httpresponseData;
        private readonly ILogger<WalletAccountDataProcessor> log;
        IProcessAccountIdData _process;
        WalletAccountIDEventData WalletAccountIDEventData = new WalletAccountIDEventData();




        #endregion

        public WalletAccountDataProcessor(ISfmchelper sfmchelper, ISetResponse config, ILogger<WalletAccountDataProcessor> logger, IProcessAccountIdData processAccountIdData)
        {
            _sfmchelper = sfmchelper;
            _config = config;
            log = logger;
            _process = processAccountIdData ;
        }

        [Function("WalletAccountDataProcessor")]
        public async Task<HttpResponseData> RWalletAccountDataProcessor([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            try
            {
                if (req != null)
                {
                    dynamic dataArray = await req.ReadFromJsonAsync<List<WalletAccountIDEventData>>();
                    if (dataArray == null)
                    {
                        log.LogInformation("Data array is null , Error Occured while  processing request");
                        httpresponseData = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, "Data array is null", req);
                    }
                    else
                    {
                        #region Checking for stream analysis
                        log.LogInformation(await new StreamReader(req.Body).ReadToEndAsync());
                        if (dataArray.ToString().Length > 262144 || dataArray.Count > 100)
                        {
                            HttpResponseMessage Errorresponse = new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                            log.LogInformation(Errorresponse.ToString());
                            httpresponseData = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, Errorresponse.ToString(), req);
                        }
                        #endregion
                        else
                        {
                            List<WalletAccountIDEventData> request = dataArray;
                            if (request.Count > 0)
                            {
                                //Insert To Log Db
                                await _process.WalletAccountIDEventPush(request,log) ;

                                walletAccountDataModel = await _sfmchelper.InsertList(request);
                                log.LogInformation("Processing request:");
                                dataExtentionsResponse = await _sfmchelper.insertSFMC(walletAccountDataModel);
                                if (dataExtentionsResponse != null && string.IsNullOrWhiteSpace(dataExtentionsResponse.errorcode))
                                {
                                    log.LogInformation(dataExtentionsResponse.requestId);
                                    httpresponseData = _config.SetHttpResponseData(HttpStatusCode.OK, "Data inserted Successfully", req);
                                    log.LogInformation("Data inserted Successfully");

                                    //Insert To Log Db
                                   await _config.SetAccountId(WalletAccountIDEventData, walletAccountDataModel, log);

                                }
                                else
                                {
                                    httpresponseData = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, "Insert to SFMC was not successfull", req);
                                    log.LogInformation("Insert to SFMC was not successfull");
                                }
                               
                            }
                            else
                            {
                                httpresponseData = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, "Input was null, Can't process the request", req);
                                log.LogInformation("Input was null, Can't process the request");
                            }
                        }
                    }
                    return httpresponseData;
                }
                else
                {
                    httpresponseData = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, "Input was null, Can't process the request", req);
                    log.LogInformation("Input was null, Can't process the request");
                    return httpresponseData;

                }
            }
            catch (Exception ex)
            {
                log.LogInformation(string.Format("Exception :{0}", ex.Message + ex.InnerException + ex.StackTrace));
                var response = _config.SetHttpResponseData(HttpStatusCode.RequestEntityTooLarge, "Some error occured", req);
                return response;
            }

        }

    }
}
