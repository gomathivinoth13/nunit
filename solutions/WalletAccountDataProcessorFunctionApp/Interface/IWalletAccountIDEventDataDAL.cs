using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface IWalletAccountIDEventDataDAL
    {
        public Task<bool> SetWalletAccountIdEventData(WalletAccountIDEventData data);
        public Task<bool> UpdateWalletAccountIdEventData(WalletAccountIDEventData data);


    }
}
