using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Redemption
    {
        [JsonProperty(PropertyName = "maximumAccountUsage", NullValueHandling = NullValueHandling.Ignore)]
        public string MaximumAccountUsage { get; set; }
    }
}
