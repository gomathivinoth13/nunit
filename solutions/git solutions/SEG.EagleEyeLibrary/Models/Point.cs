using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Point
    {
        [JsonProperty(PropertyName = "points", NullValueHandling = NullValueHandling.Ignore)]
        public Int64 Points { get; set; }

        [JsonProperty(PropertyName = "validTo", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidTo { get; set; }
    }
}
