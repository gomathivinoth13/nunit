using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SalesForceLibrary.Models
{
    public class EETagData
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
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Include_Exclude { get; set; }

    }
}
