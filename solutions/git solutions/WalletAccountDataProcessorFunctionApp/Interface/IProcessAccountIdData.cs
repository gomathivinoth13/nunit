using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface IProcessAccountIdData
    {
        public Task WalletAccountIDEventPush(List<WalletAccountIDEventData> dataList, ILogger log);
        public Task UpdateWalletIdAccountEvent(WalletAccountIDEventData data, ILogger log);



    }
}
