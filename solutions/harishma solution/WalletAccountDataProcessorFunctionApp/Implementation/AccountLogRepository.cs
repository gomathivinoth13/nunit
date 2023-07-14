using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;
using System.Linq;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{

    public class AccountLogRepository : IAccountLogRepository
    {
        private readonly IWalletAccountDataProcessEventDataRepo _walletAccountIDEventData;
        private readonly ILogger<AccountLogRepository> _log;
        public AccountLogRepository(IWalletAccountDataProcessEventDataRepo walletData, ILogger<AccountLogRepository> log)
        {
            _walletAccountIDEventData = walletData ?? throw new ArgumentNullException(nameof(walletData)); 
            _log                      = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task InsertWalletAccountLog(List<WalletAccountIDEventData> dataList)
        {
            var filteredDataList = dataList.Where(data => !string.IsNullOrEmpty(data.AccountID)).ToList();

            foreach (WalletAccountIDEventData data in filteredDataList)
            {
                data.Created_DT = DateTime.UtcNow;
                data.Created_Source = ResponseMessage.Source_Create;
                data.Updated_DT = DateTime.UtcNow;
            }

            if (filteredDataList.Count > 0)
            {
                bool result = await _walletAccountIDEventData.SetWalletAccountIdEventData(filteredDataList);
            }
            else
            {
                _log.LogWarning(ResponseMessage.NullErrorMessage);
            }
        }


        public async Task UpdateWalletIdAccountEvents(List<WalletAccountIDEventData> dataList)
        {
            var filteredDataList = dataList.Where(data => !string.IsNullOrEmpty(data.AccountID)).ToList();

            foreach (var data in dataList)
            {

                data.Created_Source = ResponseMessage.Source_Process;
                data.Updated_DT = DateTime.UtcNow;
            }
            if(filteredDataList.Count > 0)
            {
                bool result = await _walletAccountIDEventData.UpdateWalletAccountIdEventData(dataList);

            }
            else
            {
                _log.LogWarning(ResponseMessage.NullErrorMessage);
            }
        }


    }
}
