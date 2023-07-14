using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class WalletRecommendations
    {
        [JsonProperty(PropertyName = "guid", NullValueHandling = NullValueHandling.Ignore)]
        public string Guid { get; set; }

        [JsonProperty(PropertyName = "catalogue", NullValueHandling = NullValueHandling.Ignore)]
        public string Catalogue { get; set; }

        [JsonProperty(PropertyName = "target", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Target { get; set; }

        //"target": [
        //  "campaignId: 12345"
        //],

        [JsonProperty(PropertyName = "validFrom", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ValidFrom { get; set; }

        [JsonProperty(PropertyName = "validTo", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ValidTo { get; set; }
        //"channels": [
        //  "WEB",
        //  "APP"
        //],

        [JsonProperty(PropertyName = "weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        //"meta": {},

        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        [JsonProperty(PropertyName = "dateCreated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "lastUpdated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdated { get; set; }

    }
}
