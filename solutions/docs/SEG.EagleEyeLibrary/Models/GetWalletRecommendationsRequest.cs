using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetWalletRecommendationsRequest
    {
        [JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }


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


        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        //        channelstring ($string)(query)
        //Search by Recommendation channel channel - Search by Recommendation channel

        [JsonProperty(PropertyName = "channel", NullValueHandling = NullValueHandling.Ignore)]
        public string Channel { get; set; }


        //        validFrom
        //string ($ees-date-time)(query)Search by Recommendation Valid From date-time(in ATOM format)

        //validFrom - Search by Recommendation Valid From date-time(in ATOM format)

        [JsonProperty(PropertyName = "validFrom", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidFrom { get; set; }


        //        validTo
        //string ($ees-date-time)(query)
        //Search by Recommendation Valid To date-time(in ATOM format) validTo - Search by Recommendation Valid To date-time(in ATOM format)

        [JsonProperty(PropertyName = "validTo", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidTo { get; set; }

    }
}
