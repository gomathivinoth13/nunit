using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Data
    {

        [JsonProperty(PropertyName = "startDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }


        [JsonProperty(PropertyName = "couponCategory", NullValueHandling = NullValueHandling.Ignore)]
        public Field CouponCategory { get; set; }


        [JsonProperty(PropertyName = "couponDisclaimerTermsAndConditions", NullValueHandling = NullValueHandling.Ignore)]
        public Field CouponDisclaimerTermsAndConditions { get; set; }

        //[JsonProperty(PropertyName = "digitalImage", NullValueHandling = NullValueHandling.Ignore)]
        //public string DigitalImage { get; set; }

        [JsonProperty(PropertyName = "listViewImage", NullValueHandling = NullValueHandling.Ignore)]
        public Field ListViewImage { get; set; }

        [JsonProperty(PropertyName = "digitalViewImage", NullValueHandling = NullValueHandling.Ignore)]
        public Field DigitalViewImage { get; set; }


        [JsonProperty(PropertyName = "brand", NullValueHandling = NullValueHandling.Ignore)]
        public Field Brand { get; set; }

    }
}
