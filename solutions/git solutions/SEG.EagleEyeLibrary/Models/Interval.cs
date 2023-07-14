using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Interval
    {

        [JsonProperty(PropertyName = "discountAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountAmount { get; set; }


        [JsonProperty(PropertyName = "percentageAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string PercentageAmount { get; set; }
    }
}
