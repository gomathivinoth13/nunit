using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class AccountTrasaction
    {
        [JsonProperty(PropertyName = "event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "source", NullValueHandling = NullValueHandling.Ignore)]
        public int Source { get; set; }

        [JsonProperty(PropertyName = "balancesBefore", NullValueHandling = NullValueHandling.Ignore)]
        public BalancesInfo balancesBefore { get; set; }

        [JsonProperty(PropertyName = "balancesAfter", NullValueHandling = NullValueHandling.Ignore)]
        public BalancesInfo balancesAfter { get; set; }

        [JsonProperty(PropertyName = "dateCreated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime dateCreated { get; set; }

        [JsonProperty(PropertyName = "lastUpdated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime lastUpdated { get; set; }

        [JsonProperty(PropertyName = "transactionDetails", NullValueHandling = NullValueHandling.Ignore)]
        public TransactionDetails transactionDetails { get; set; }

        //  "event": "CREATE",
        //"value": 0,
        //"source": 1,
        //"balancesBefore": {
        //  "current": 0,
        //  "lifetime": 0
        //},
        //"balancesAfter": {
        //  "current": 0,
        //  "lifetime": 0
        //},
        //"transactionDetails": [],
        //"properties": [],
        //"dateCreated": "2020-10-19T14:30:21+01:00",
        //"lastUpdated": "2020-10-19T14:30:21+01:00"
    }
}
