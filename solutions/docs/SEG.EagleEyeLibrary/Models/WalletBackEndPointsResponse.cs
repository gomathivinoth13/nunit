using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class WalletBackEndPointsResponse : EagleEyeFailureResponse
    {

        [JsonProperty(PropertyName = "walletTransactionId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletTransactionId { get; set; }

        [JsonProperty(PropertyName = "parentWalletTransactionId", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentWalletTransactionId { get; set; }


        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "reference", NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }

        [JsonProperty(PropertyName = "transactionDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionDateTime { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }


        //        {
        //    "walletTransactionId": "75468603",
        //    "parentWalletTransactionId": "0",
        //    "walletId": "40843778",
        //    "reference": "eesbep801f8fce5b13915e7f64acff2d005a81a37dd3bc",
        //    "transactionDateTime": "2021-01-27T15:04:02+00:00",
        //    "identityId": null,
        //    "identity": null,
        //    "type": "ADJUSTMENT",
        //    "status": "SETTLED",
        //    "meta": {
        //        "key1": "value1",
        //        "wallettransactionmeta": "testMetaTxt",
        //        "transactiondescription": "ChristestTxt",
        //        "transactiondate": "2021-01-27 15:04:02"
        //    },
        //    "state": "ORIGINAL",
        //    "expiryDate": null,
        //    "accounts": [
        //        {
        //            "accountId": "588926369",
        //            "accountTransactionId": "1369822370"
        //        }
        //    ],
        //    "basket": {
        //        "contents": null,
        //        "summary": {
        //            "pointsCredited": 12,
        //            "pointsRedeemed": 0,
        //            "pointsRefunded": 0
        //        },
        //        "payment": null
        //    },
        //    "channel": "api",
        //    "location": {
        //        "storeId": null,
        //        "storeParentId": null
        //    },
        //    "dateCreated": "2021-01-27T15:04:02+00:00",
        //    "lastUpdated": "2021-01-27T15:04:03+00:00"
        //}

    }
}
