using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Banner
    {
        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public Value Value { get; set; }
    }
}
