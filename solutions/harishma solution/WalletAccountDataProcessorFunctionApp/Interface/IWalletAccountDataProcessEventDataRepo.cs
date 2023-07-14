
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface IWalletAccountDataProcessEventDataRepo
    {
        public Task<bool> SetWalletAccountIdEventData(List<WalletAccountIDEventData> data);
        public Task<bool> UpdateWalletAccountIdEventData(List<WalletAccountIDEventData> data);


    }
}
