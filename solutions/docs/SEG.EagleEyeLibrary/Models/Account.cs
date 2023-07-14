using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Account
    {
        [JsonProperty(PropertyName = "accountId", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }


        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }


        [JsonProperty(PropertyName = "campaignId", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignId { get; set; }


        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "clientType", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientType { get; set; }


        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }


        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }


        [JsonProperty(PropertyName = "dates", NullValueHandling = NullValueHandling.Ignore)]
        public Dates Dates { get; set; }


        [JsonProperty(PropertyName = "dateCreated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateCreated { get; set; }

        [JsonProperty(PropertyName = "lastUpdated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastUpdated { get; set; }


        [JsonProperty(PropertyName = "balances", NullValueHandling = NullValueHandling.Ignore)]
        public Balances Balances { get; set; }


        [JsonProperty(PropertyName = "coupon", NullValueHandling = NullValueHandling.Ignore)]
        public Coupon Coupon { get; set; }

        [JsonProperty(PropertyName = "points", NullValueHandling = NullValueHandling.Ignore)]
        public Int64 Points { get; set; }

        [JsonProperty(PropertyName = "validTo", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidTo { get; set; }

        [JsonProperty(PropertyName = "pointsInfo", NullValueHandling = NullValueHandling.Ignore)]
        public List<Point> PointsInfo { get; set; }

    }
}
