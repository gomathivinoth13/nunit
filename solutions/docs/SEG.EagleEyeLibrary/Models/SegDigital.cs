using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class SegDigital
    {
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }
    }
}
