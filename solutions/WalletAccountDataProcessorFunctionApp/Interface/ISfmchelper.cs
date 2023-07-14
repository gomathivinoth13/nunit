using SalesForceLibrary.Models;
using SEG.SalesForce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface ISfmchelper
    {
        public Task<WalletAccountDataModel> InsertList(List<WalletAccountIDEventData> inputModel);
        public Task<DataExtentionsResponse> insertSFMC(WalletAccountDataModel dataExtentionRequest);


    }
}
