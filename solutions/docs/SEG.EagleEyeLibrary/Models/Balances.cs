using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Balances
    {
        [JsonProperty(PropertyName = "available", NullValueHandling = NullValueHandling.Ignore)]
        public int Available { get; set; }

        [JsonProperty(PropertyName = "refundable", NullValueHandling = NullValueHandling.Ignore)]
        public int Refundable { get; set; }

        [JsonProperty(PropertyName = "totalSpend", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalSpend { get; set; }

        [JsonProperty(PropertyName = "transactionCount", NullValueHandling = NullValueHandling.Ignore)]
        public int TransactionCount { get; set; }

        [JsonProperty(PropertyName = "totalUnits", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalUnits { get; set; }

        [JsonProperty(PropertyName = "current", NullValueHandling = NullValueHandling.Ignore)]
        public int Current { get; set; }

        [JsonProperty(PropertyName = "usable", NullValueHandling = NullValueHandling.Ignore)]
        public int Usable { get; set; }


        [JsonProperty(PropertyName = "locked", NullValueHandling = NullValueHandling.Ignore)]
        public int Locked { get; set; }

        [JsonProperty(PropertyName = "lifetime", NullValueHandling = NullValueHandling.Ignore)]
        public int Lifetime { get; set; }

    }
}
