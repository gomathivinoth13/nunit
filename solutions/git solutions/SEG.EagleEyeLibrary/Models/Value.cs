using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Value
    {
        [JsonProperty(PropertyName = "percentageAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string PercentageAmount { get; set; }


        [JsonProperty(PropertyName = "discountAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "standard", NullValueHandling = NullValueHandling.Ignore)]
        public Standard Standard { get; set; }
    }
}
