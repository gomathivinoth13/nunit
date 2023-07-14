using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AccessTokenV2
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "accessToken", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "expiresIn", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpiresIn { get; set; }

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

