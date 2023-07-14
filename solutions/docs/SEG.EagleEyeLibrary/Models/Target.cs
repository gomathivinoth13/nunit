using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Target
    {
        [JsonProperty(PropertyName = "campaignId", NullValueHandling = NullValueHandling.Ignore)]
        public string CampaignId { get; set; }
    }
}
