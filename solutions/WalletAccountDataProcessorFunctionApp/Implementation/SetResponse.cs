using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SalesForceLibrary.Models;
using System.Net;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{
    public class SetResponse: ISetResponse
    {
        IProcessAccountIdData process;

        public SetResponse(IProcessAccountIdData processAccountIdData)
        {
            process = processAccountIdData;
        }

        public HttpResponseData SetHttpResponseData(HttpStatusCode code, string message, HttpRequestData requestData)
        {
            var responseData = requestData.CreateResponse(code);
            responseData.WriteString(message);
            return responseData;

        }
        public async Task SetAccountId(WalletAccountIDEventData eventData , WalletAccountDataModel walletAccountDataModel , ILogger logger)
        {
            foreach (var x in walletAccountDataModel.items)
            {
                eventData.AccountID = x.Account_ID;
                await process.UpdateWalletIdAccountEvent(eventData ,logger);
            }
        }
    }
}
