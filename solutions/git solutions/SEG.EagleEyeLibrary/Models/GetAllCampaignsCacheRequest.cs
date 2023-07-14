using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetAllCampaignsCacheRequest
    {
        /// <summary>
        /// 
        /// </summary>

        [JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        /// <summary>
        ///  //Query string Pagination Limit Default value : 100
        /// </summary>
        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(100)]
        public int Limit { get; set; }


        /// <summary>
        ///  //Query string Pagination Offset Default value : 0
        /// </summary>
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(0)]
        public int Offset { get; set; }


        ///// <summary>
        /////  //Query string Pagination Offset Default value : 0
        ///// </summary>
        //[JsonProperty(PropertyName = "nextCursor", NullValueHandling = NullValueHandling.Ignore)]
        //public int nextCursor { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
        //[JsonProperty(PropertyName = "sortBy", NullValueHandling = NullValueHandling.Ignore)]
        //public string SortBy { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[JsonProperty(PropertyName = "filterBy", NullValueHandling = NullValueHandling.Ignore)]
        //public string FilterBy { get; set; }


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
        /// This will be True if Customer Opts into Alcohol Promotions, False(or null) if they Opt Out
        /// </summary>
        [JsonProperty(PropertyName = "alcoholOffersAccepted", NullValueHandling = NullValueHandling.Ignore)]
        public bool AlcoholOffersAccepted { get; set; }
    }
}
