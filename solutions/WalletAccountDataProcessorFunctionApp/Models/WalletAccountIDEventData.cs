using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalletAccountDataProcessorFunctionApp.Models
{
    public class WalletAccountIDEventData
    {
        [JsonProperty(PropertyName = "EventID", NullValueHandling = NullValueHandling.Ignore)]
        public string EventID { get; set; }

        [JsonProperty(PropertyName = "EventName", NullValueHandling = NullValueHandling.Ignore)]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "ObjectType", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectType { get; set; }

        [JsonProperty(PropertyName = "Event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "AccountID", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID { get; set; }

        [JsonProperty(PropertyName = "WalletID", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletID { get; set; }

        [JsonProperty(PropertyName = "CampaignID", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignID { get; set; }


        [JsonProperty(PropertyName = "State", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(PropertyName = "Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "Status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "ClientType", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientType { get; set; }

        [JsonProperty(PropertyName = "Created_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Created_DT { get; set; }

        [JsonProperty(PropertyName = "Created_Source", NullValueHandling = NullValueHandling.Ignore)]
        public string Created_Source { get; set; }

        [JsonProperty(PropertyName = "Updated_DT", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Updated_DT { get; set; }

        public Dates Dates { get; set; }

    }
    public class Dates
    {
        [JsonProperty(PropertyName = "start", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime start { get; set; }

        [JsonProperty(PropertyName = "end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime end { get; set; }

    }
}