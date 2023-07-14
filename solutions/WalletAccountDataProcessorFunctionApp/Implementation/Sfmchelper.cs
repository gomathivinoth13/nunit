using SalesForceLibrary.Models;
using SalesForceLibrary.SalesForceAPIM;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{
    public class Sfmchelper : ISfmchelper
    {
        WalletAccountDataModel walletAccountDataModel = new WalletAccountDataModel();
        SalesForceAPIMService salesForceService;

        /// <summary>
        /// Insert the elemets to a list to insert to SFMC
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public Task<WalletAccountDataModel> InsertList(List<WalletAccountIDEventData> walletAccountInputModel)
        {

            walletAccountDataModel.items = new List<SalesForceLibrary.Models.AccountId.Item>();
            foreach (var inputModel in walletAccountInputModel)
            {
                walletAccountDataModel.items.Add(new SalesForceLibrary.Models.AccountId.Item()
                {
                    Account_ID    = inputModel.AccountID,
                    Wallet_ID     = inputModel.WalletID,
                    Valid_To      = inputModel.Dates.end,
                    Valid_From    = inputModel.Dates.start,
                    Campaign_ID   = inputModel.CampaignID,
                    State         = inputModel.State,
                    ClientType    = inputModel.ClientType,
                    Type          = inputModel.Type,
                    Status        = inputModel.Status
                });
            }
            return Task.FromResult(walletAccountDataModel);
        }
        /// <summary>
        /// Insert to SFMC
        /// </summary>
        /// <param name="dataExtentionRequest"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> insertSFMC(WalletAccountDataModel dataExtentionRequest)
        {
            setconfiguration();
            DataExtentionsResponse dataExtentionsResponse = new DataExtentionsResponse();
            if (dataExtentionRequest.items.Count > 0)
            {
                dataExtentionsResponse = await salesForceService.InsertAccountId(dataExtentionRequest, Environment.GetEnvironmentVariable("SEG_Key")).ConfigureAwait(false);

            }
            else
            {
                dataExtentionsResponse = new DataExtentionsResponse();
                dataExtentionsResponse.errorcode = "Error";
            }
            return dataExtentionsResponse;
        }
        /// <summary>
        /// Setting configuration values
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
            #endregion
        }
    }
}
