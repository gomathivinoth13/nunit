using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotificationSFMCFunctionApp.Models
{
    public class EagleEyeMBOIssuanceEventData
    {
        [JsonProperty(PropertyName = "EventID", NullValueHandling = NullValueHandling.Ignore)]
        public string EventID { get; set; }

        [JsonProperty(PropertyName = "EventName", NullValueHandling = NullValueHandling.Ignore)]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "AccountID", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID { get; set; }

        [JsonProperty(PropertyName = "WalletID", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletID { get; set; }

        [JsonProperty(PropertyName = "CampaignID", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignID { get; set; }


        [JsonProperty(PropertyName = "Merchant_Store_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Merchant_Store_ID { get; set; }

        [JsonProperty(PropertyName = "Merchant_Parent_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Merchant_Parent_ID { get; set; }

        [JsonProperty(PropertyName = "Status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "CouponEndDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CouponEndDate { get; set; }

        [JsonProperty(PropertyName = "ClientType", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientType { get; set; }

        [JsonProperty(PropertyName = "Created_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Created_DT { get; set; }

        [JsonProperty(PropertyName = "Created_Source", NullValueHandling = NullValueHandling.Ignore)]
        public string Created_Source { get; set; }

        [JsonProperty(PropertyName = "Updated_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Updated_DT { get; set; }


        [JsonProperty(PropertyName = "Updated_Source", NullValueHandling = NullValueHandling.Ignore)]
        public string Updated_Source { get; set; }

    }
}
