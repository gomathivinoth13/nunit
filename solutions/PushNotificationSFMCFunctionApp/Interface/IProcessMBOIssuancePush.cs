using Microsoft.Extensions.Logging;
using PushNotificationSFMCFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationSFMCFunctionApp.Interface
{
    public interface IProcessMBOIssuancePush
    {
        //Task MBOIssuancePush(List<EagleEyeMBOIssuanceEventData> dataList, ILogger log);
        Task MBOIssuancePush(EagleEyeMBOIssuanceEventData data, ILogger log);
    }
}
