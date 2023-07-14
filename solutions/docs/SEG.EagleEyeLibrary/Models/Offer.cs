using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Offer
    {
        [JsonProperty(PropertyName = "promoId", NullValueHandling = NullValueHandling.Ignore)]
        public string PromoId { get; set; }

        [JsonProperty(PropertyName = "offerType", NullValueHandling = NullValueHandling.Ignore)]
        public string OfferType { get; set; }


        [JsonProperty(PropertyName = "qualification", NullValueHandling = NullValueHandling.Ignore)]
        public Qualification Qualification { get; set; }


        [JsonProperty(PropertyName = "reward", NullValueHandling = NullValueHandling.Ignore)]
        public Reward Reward { get; set; }
    }
}
