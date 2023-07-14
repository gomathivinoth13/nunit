using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Rules
    {
        [JsonProperty(PropertyName = "redemption", NullValueHandling = NullValueHandling.Ignore)]
        public Redemption Redemption { get; set; }
    }
}
