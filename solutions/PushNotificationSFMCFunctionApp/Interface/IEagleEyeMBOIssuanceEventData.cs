using Microsoft.Extensions.Logging;
using PushNotificationSFMCFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationSFMCFunctionApp.Interface
{
    public interface IEagleEyeMBOIssuanceEventData
    {
        Task<EagleEyeMBOIssuanceEventData> GetEagleEyeMBOIssuanceEventData(string accountID);

        Task<bool> SetEagleEyeMBOIssuanceEventData(EagleEyeMBOIssuanceEventData data);
    }
}
