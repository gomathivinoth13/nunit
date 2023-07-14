using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetWalletRecommendationsResponse : EagleEyeFailureResponse
    {
        [JsonProperty(PropertyName = "walletRecommendations", NullValueHandling = NullValueHandling.Ignore)]
        public List<WalletRecommendations> WalletRecommendations { get; set; }
    }
}
