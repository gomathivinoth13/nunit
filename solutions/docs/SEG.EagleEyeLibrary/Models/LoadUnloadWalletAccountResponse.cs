using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class LoadUnloadWalletAccountResponse : EagleEyeFailureResponse
    {
        [JsonProperty(PropertyName = "accountId", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "campaignId", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignId { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        //ECOUPON

        [JsonProperty(PropertyName = "clientType", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientType { get; set; }
        //":"MANUFACTURER"

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        //ACTIVE",

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        //":"LOADED",

        [JsonProperty(PropertyName = "dates", NullValueHandling = NullValueHandling.Ignore)]
        public Dates Dates { get; set; }


        [JsonProperty(PropertyName = "balances", NullValueHandling = NullValueHandling.Ignore)]
        public Balances Balances { get; set; }
    }
}
