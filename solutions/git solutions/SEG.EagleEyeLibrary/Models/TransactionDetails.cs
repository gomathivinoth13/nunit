using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class TransactionDetails
    {
        [JsonProperty(PropertyName = "merchant_store_id", NullValueHandling = NullValueHandling.Ignore)]
        public string merchant_store_id { get; set; }


        [JsonProperty(PropertyName = "merchant_store_parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string merchant_store_parent_id { get; set; }

        [JsonProperty(PropertyName = "reasonCode", NullValueHandling = NullValueHandling.Ignore)]
        public string reasonCode { get; set; }

        [JsonProperty(PropertyName = "shoppingCenterName", NullValueHandling = NullValueHandling.Ignore)]
        public string ShoppingCenterName { get; set; }

        [JsonProperty(PropertyName = "storeAddress1", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreAddress1 { get; set; }

        [JsonProperty(PropertyName = "storeAddress2", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreAddress2 { get; set; }

        [JsonProperty(PropertyName = "storeCity", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreCity { get; set; }

        [JsonProperty(PropertyName = "storeState", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreState { get; set; }

        [JsonProperty(PropertyName = "storeZip", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreZip { get; set; }
    }

}
