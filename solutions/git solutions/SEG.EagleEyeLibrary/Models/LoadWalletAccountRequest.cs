using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class LoadWalletAccountRequest
    {
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "campaignId", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignId { get; set; }


        [JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "catalogueGuid", NullValueHandling = NullValueHandling.Ignore)]
        public string CatalogueGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "recommendationGuid", NullValueHandling = NullValueHandling.Ignore)]
        public string RecommendationGuid { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "storeId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreId
        {
            get { return _storeId; }
            set { _storeId = value?.PadLeft(4, '0'); }
        }
        private string _storeId;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "chainId", NullValueHandling = NullValueHandling.Ignore)]
        public int ChainId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "actionType", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionType { get; set; }
    }
}
