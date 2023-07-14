using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class CampaignsWeight
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Id { get; set; }

        [JsonProperty(PropertyName = "weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; }



    }
}
