using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Collection
    {
        [JsonProperty(PropertyName = "minimumUnits", NullValueHandling = NullValueHandling.Ignore)]
        public string minimumUnits { get; set; }

        [JsonProperty(PropertyName = "minimumProductSpend", NullValueHandling = NullValueHandling.Ignore)]
        public string minimumProductSpend { get; set; }

        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public Value Value { get; set; }
    }
}
