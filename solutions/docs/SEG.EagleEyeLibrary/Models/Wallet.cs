using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Wallet
    {
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "friendlyName", NullValueHandling = NullValueHandling.Ignore)]
        public string FriendlyName { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

    }
}

//        {
//  "walletId": "10663859",
//  "friendlyName": "John's Wallet",
//  "status": "ACTIVE",
//  "type": "CONSUMER",
//  "state": "EARNBURN",
//  "relationships": {
//    "parent": [],
//    "child": [
//      "10663858"
//    ],
//    "associate": [],
//    "donor": []
//    },
//  "meta": {
//    "key1": "Value1",
//    "foo": "bar"
//  },
//  "dateCreated": "2017-01-03T16:25:58+00:00",
//  "lastUpdated": "2017-01-03T16:25:58+00:00"
//}
//    }
//}
