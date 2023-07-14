using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EEGroupData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CouponID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Include_Exclude { get; set; }
    }
}
