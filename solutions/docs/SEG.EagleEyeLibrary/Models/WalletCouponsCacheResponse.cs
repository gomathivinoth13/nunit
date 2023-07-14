using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class WalletCouponsCacheResponse : EagleEyeFailureResponse
    {
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }

        //[JsonProperty(PropertyName = "nextCursor", NullValueHandling = NullValueHandling.Ignore)]
        //public int nextCursor { get; set; }

        //[JsonProperty(PropertyName = "currentCursor", NullValueHandling = NullValueHandling.Ignore)]
        //public int currentCursor { get; set; }

        [JsonProperty(PropertyName = "total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }


        [JsonProperty(PropertyName = "orderBy", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBy> OrderBy { get; set; }

        [JsonProperty(PropertyName = "coupons", NullValueHandling = NullValueHandling.Ignore)]
        public List<Coupon> Coupons { get; set; }

        //[JsonProperty(PropertyName = "accounts", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Account> Accounts { get; set; }

        [JsonProperty(PropertyName = "statusCode", NullValueHandling = NullValueHandling.Ignore)]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sortBy", NullValueHandling = NullValueHandling.Ignore)]
        public string SortBy { get; set; }

        /// <summary>
        /// This will be True if Customer Opts into Alcohol Promotions, False(or null) if they Opt Out
        /// </summary>
        [JsonIgnore]
        public bool AlcoholOffersAccepted { get; set; }
    }
}
