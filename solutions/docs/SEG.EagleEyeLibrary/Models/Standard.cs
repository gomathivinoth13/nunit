using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Standard
    {
        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public Value Value { get; set; }

        [JsonProperty(PropertyName = "interval", NullValueHandling = NullValueHandling.Ignore)]
        public Interval interval { get; set; }

        [JsonProperty(PropertyName = "discountAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "finalAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string FinalAmount { get; set; }

        [JsonProperty(PropertyName = "percentageAmount", NullValueHandling = NullValueHandling.Ignore)]
        public string PercentageAmount { get; set; }


        [JsonProperty(PropertyName = "allFree", NullValueHandling = NullValueHandling.Ignore)]
        public string AllFree { get; set; }
    }
}
