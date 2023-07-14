using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Reward
    {

        [JsonProperty(PropertyName = "standard", NullValueHandling = NullValueHandling.Ignore)]
        public Standard Standard { get; set; }

        [JsonProperty(PropertyName = "product", NullValueHandling = NullValueHandling.Ignore)]
        public Product Product { get; set; }
    }
}
