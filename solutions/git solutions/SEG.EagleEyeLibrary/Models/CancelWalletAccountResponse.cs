using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class CancelWalletAccountResponse : EagleEyeFailureResponse
    {
        [JsonProperty(PropertyName = "accountId", NullValueHandling = NullValueHandling.Ignore)]
        public string accountId { get; set; }

        [JsonProperty(PropertyName = "event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "account", NullValueHandling = NullValueHandling.Ignore)]
        public Account Account { get; set; }

        //        {
        //  "accountTransactionId": "502395631",
        //  "parentAccountTransactionId": "",
        //  "accountId": "360658057",
        //  "account": {
        //    "accountId": "360658057",
        //    "walletId": "25463178",
        //    "campaignId": "727452",
        //    "type": "POINTS",
        //    "clientType": "RETAILPOINTS",
        //    "status": "CANCELLED",
        //    "state": "LOADED",
        //    "dates": {
        //      "start": "2020-04-22T09:47:56+01:00",
        //      "end": "2038-01-19T03:14:07+00:00"
        //    },
        //    "meta": [],
        //    "dateCreated": "2020-04-22T09:47:56+01:00",
        //    "lastUpdated": "2020-06-24T22:18:52+01:00",
        //    "overrides": [],
        //    "balances": {
        //      "current": 0,
        //      "usable": 0,
        //      "locked": 0,
        //      "lifetime": 0,
        //      "lifetimeSpend": 0,
        //      "lifetimeSpendValue": 0
        //    },
        //    "relationships": []
        //    },
        //  "event": "CANCEL",
        //  "value": 0,
        //  "source": 1,
        //  "balancesBefore": {
        //    "current": 0,
        //    "lifetime": 0
        //  },
        //  "balancesAfter": {
        //    "current": 0,
        //    "lifetime": 0
        //  },
        //  "transactionDetails": [],
        //  "properties": [],
        //  "dateCreated": "2020-06-24T22:18:52+01:00",
        //  "lastUpdated": "2020-06-24T22:18:52+01:00"
        //}
    }
}
