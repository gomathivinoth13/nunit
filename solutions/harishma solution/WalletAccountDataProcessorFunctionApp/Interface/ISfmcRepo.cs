using SalesForceLibrary.Models;
using SEG.SalesForce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface ISfmcRepo
    {
        public Task<DataExtentionsResponse> InsertToSFMC(List<WalletAccountIDEventData> walletAccountInputModel);
        public Task<DataExtentionsResponse> SetWalletAccountIDData(WalletAccountDataModel dataExtentionRequest);
    }
}
