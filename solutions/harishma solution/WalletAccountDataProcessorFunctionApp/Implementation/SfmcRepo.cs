using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class SfmcRepo : ISfmcRepo
    {
        private readonly SalesForceAPIMService _salesForceService;
        private readonly IAccountLogRepository _accountLogRepository;
        private readonly ILogger<SfmcRepo> _log;
        private readonly string SfmcDataExtensionKey;

        public SfmcRepo(SalesForceAPIMService salesForceAPIMService, IAccountLogRepository accountLogRepository, ILogger<SfmcRepo> log, IConfiguration configuration)
        {
            _salesForceService = salesForceAPIMService ?? throw new ArgumentNullException(nameof(salesForceAPIMService));
            _accountLogRepository = accountLogRepository ?? throw new ArgumentNullException(nameof(accountLogRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            SfmcDataExtensionKey = Environment.GetEnvironmentVariable("SEG_Key") ?? throw new ArgumentNullException(nameof(SfmcDataExtensionKey));
        }

        /// <summary>
        /// Insert the elemets to a list to insert to SFMC
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> InsertToSFMC(List<WalletAccountIDEventData> dataArray)
        {
            var walletAccountDataModel = new WalletAccountDataModel();
            walletAccountDataModel.items = new List<SalesForceLibrary.Models.AccountId.Item>();
            var dataExtentionsResponse = new DataExtentionsResponse();
            foreach (var inputModel in dataArray)
            {
                walletAccountDataModel.items.Add(new SalesForceLibrary.Models.AccountId.Item
                {
                    Account_ID = inputModel.AccountID,
                    Wallet_ID = inputModel.WalletID,
                    Valid_To = inputModel.Dates.End,
                    Valid_From = inputModel.Dates.Start,
                    Campaign_ID = inputModel.CampaignID,
                    State = inputModel.State,
                    ClientType = inputModel.ClientType,
                    Type = inputModel.Type,
                    Status = inputModel.Status
                });
            }
            if (walletAccountDataModel.items.Count > 0)
            {
                dataExtentionsResponse = await SetWalletAccountIDData(walletAccountDataModel);
            }
            return dataExtentionsResponse;
        }
        /// <summary>
        /// Insert to SFMC
        /// </summary>
        /// <param name="dataExtentionRequest"></param>
        /// <returns></returns>
        public async Task<DataExtentionsResponse> SetWalletAccountIDData(WalletAccountDataModel walletAccountDataModel)
        {
            try
            {
                var dataExtentionsResponse = new DataExtentionsResponse();

                dataExtentionsResponse = await _salesForceService.InsertAccountId(walletAccountDataModel, SfmcDataExtensionKey);
                var walletAccountIDEventData = new List<WalletAccountIDEventData>();
                foreach (var item in walletAccountDataModel.items)
                {
                    walletAccountIDEventData.Add(new WalletAccountIDEventData() { AccountID = item.Account_ID });

                }
                await _accountLogRepository.UpdateWalletIdAccountEvents(walletAccountIDEventData);


                return dataExtentionsResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                throw;
            }
        }
    }
}
