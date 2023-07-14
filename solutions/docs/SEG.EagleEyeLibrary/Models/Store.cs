using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Store
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StoreId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StoreType { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string Banner { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryCluster { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SecondaryCluster { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DMA { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MacroCluster { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string id { get; set; }
    }
}
