using PushNotificationSFMCFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationSFMCFunctionApp.Interface
{
    public interface ICampaignIssuanceCount
    {
        Task<CampaignIssuanceCount> GetCampaignIssuanceCountData(string campaignID);

        Task<bool> InsertCampaignIssuanceCountData(CampaignIssuanceCount data);

        Task<bool> UpdateCampaignIssuanceCountData(CampaignIssuanceCount data);
    }
}
