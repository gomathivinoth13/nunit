using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MBOIssuanceJourneyRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "contactKey", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "eventDefinitionKey", NullValueHandling = NullValueHandling.Ignore)]

        public string EventDefinitionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public MBOIssuanceData data { get; set; }
    }
}
