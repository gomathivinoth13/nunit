using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Core.Models
{
    public class WalletBackEndPointsRequest
    {

        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "pointsValue", NullValueHandling = NullValueHandling.Ignore)]
        public int PointsValue { get; set; }

        [JsonProperty(PropertyName = "schemeReference", NullValueHandling = NullValueHandling.Ignore)]
        public string SchemeReference { get; set; }

        [JsonProperty(PropertyName = "reasonCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ReasonCode { get; set; }

        [JsonProperty(PropertyName = "reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "walletTransactionType", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletTransactionType { get; set; }

        [JsonProperty(PropertyName = "walletTransactionState", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletTransactionState { get; set; }

        [JsonProperty(PropertyName = "walletTransactionDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletTransactionDescription { get; set; }
    }
}
