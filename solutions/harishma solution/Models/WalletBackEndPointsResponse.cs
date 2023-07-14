using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Core.Models
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
    }
}
