using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Campaign
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public Details Details { get; set; }

        [JsonProperty(PropertyName = "artwork", NullValueHandling = NullValueHandling.Ignore)]
        public ArtWork Artwork { get; set; }

        [JsonProperty(PropertyName = "offer", NullValueHandling = NullValueHandling.Ignore)]
        public Offer Offer { get; set; }

        [JsonProperty(PropertyName = "distributionChannels", NullValueHandling = NullValueHandling.Ignore)]
        public DistributionChannels DistributionChannels { get; set; }


        [JsonProperty(PropertyName = "settings", NullValueHandling = NullValueHandling.Ignore)]
        public Settings Settings { get; set; }


        [JsonProperty(PropertyName = "rules", NullValueHandling = NullValueHandling.Ignore)]
        public Rules Rules { get; set; }
    }
}
