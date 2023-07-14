using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class BalancesInfo
    {

        [JsonProperty(PropertyName = "current", NullValueHandling = NullValueHandling.Ignore)]
        public int Current { get; set; }


        [JsonProperty(PropertyName = "lifetime", NullValueHandling = NullValueHandling.Ignore)]
        public int Lifetime { get; set; }

    }
}
