using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class SetWalletAccountStateRequest
    {
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }


        //[JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        //public string MemberId { get; set; }

        [JsonProperty(PropertyName = "accountId", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }


        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "storeId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "chainId", NullValueHandling = NullValueHandling.Ignore)]
        public int ChainId { get; set; }
    }
}
