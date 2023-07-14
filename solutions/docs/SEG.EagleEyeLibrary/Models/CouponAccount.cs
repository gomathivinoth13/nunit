using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class CouponAccount
    {
        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(PropertyName = "dates", NullValueHandling = NullValueHandling.Ignore)]
        public Dates Dates { get; set; }

        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public Details Details { get; set; }

    }
}

