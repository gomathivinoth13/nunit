using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MBOIssuanceData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Member_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Member_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Account_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Account_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Wallet_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Wallet_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Campaign_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Campaign_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ClientType", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Merchant_Store_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Merchant_Store_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Merchant_Parent_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Merchant_Parent_ID { get; set; }
    }
}
