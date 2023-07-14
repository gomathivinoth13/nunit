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
        public string StoreId
        {
            get { return _storeId; }
            set { _storeId = value?.PadLeft(4, '0'); }
        }
        private string _storeId;

        [JsonProperty(PropertyName = "storeParentId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreParentId { get; set; }


        [JsonProperty(PropertyName = "parentLocationIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentLocationIdentifier { get; set; }
    }
}
