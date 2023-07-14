using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class pushNotificationResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "tokenId", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "errorcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Errorcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "documentation", NullValueHandling = NullValueHandling.Ignore)]
        public string Documentation { get; set; }
    }
}
