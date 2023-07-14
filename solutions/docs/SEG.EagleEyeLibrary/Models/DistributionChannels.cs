using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class DistributionChannels
    {
        //segDigital
        //    data
        //    couponCategory
        //    fieldData



        [JsonProperty(PropertyName = "segDigital", NullValueHandling = NullValueHandling.Ignore)]
        public SegDigital SegDigital { get; set; }



        [JsonProperty(PropertyName = "ecrebo", NullValueHandling = NullValueHandling.Ignore)]
        public Ecrebo Ecrebo { get; set; }
    }
}
