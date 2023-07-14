
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface IAccountLogRepository
    {
        public Task InsertWalletAccountLog(List<WalletAccountIDEventData> dataList);
        public Task UpdateWalletIdAccountEvents(List<WalletAccountIDEventData> dataList);



    }
}
