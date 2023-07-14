using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Location
    {
        [JsonProperty(PropertyName = "locationIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationIdentifier { get; set; }

        [JsonProperty(PropertyName = "storeId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "storeParentId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreParentId { get; set; }


        [JsonProperty(PropertyName = "parentLocationIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentLocationIdentifier { get; set; }
    }
}
