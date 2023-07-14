using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SalesForceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Interface
{
    public interface ISetResponse
    {
        public HttpResponseData SetHttpResponseData(HttpStatusCode code, string message, HttpRequestData requestData);
        public  Task SetAccountId(WalletAccountIDEventData eventData, WalletAccountDataModel walletAccountDataModel, ILogger logger);



    }
}
