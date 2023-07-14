
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PushNotificationSFMCFunctionApp.Models
{
    public class CampaignIssuanceCount
    {
        [JsonProperty(PropertyName = "CampaignID", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignID { get; set; }

        [JsonProperty(PropertyName = "IssuanceCount", NullValueHandling = NullValueHandling.Ignore)]
        public Int64 IssuanceCount { get; set; }

        [JsonProperty(PropertyName = "Status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "CampaignEndDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CampaignEndDate { get; set; }

        [JsonProperty(PropertyName = "Created_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created_DT { get; set; }

        [JsonProperty(PropertyName = "Created_Source", NullValueHandling = NullValueHandling.Ignore)]
        public string Created_Source { get; set; }

        [JsonProperty(PropertyName = "Updated_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Updated_DT { get; set; }

        [JsonProperty(PropertyName = "Updated_Source", NullValueHandling = NullValueHandling.Ignore)]
        public string Updated_Source { get; set; }
    }
}
