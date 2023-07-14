using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotificationSFMCFunctionApp.Models
{
    public class CampaignIssuanceCountRequest
    {
        public string CampaignId { get; set; }
        public string CampaignEndDate { get; set; }
        public string CampaignStatus { get; set; }
    }
}
