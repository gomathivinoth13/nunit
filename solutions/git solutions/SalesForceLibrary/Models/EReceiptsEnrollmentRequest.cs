using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{

    public class EReceiptsEnrollmentRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "member_id", NullValueHandling = NullValueHandling.Ignore)]
        public string member_id { get; set; }

        [JsonProperty(PropertyName = "eReceiptEmailOptInStatus", NullValueHandling = NullValueHandling.Ignore)]
        public bool EReceiptEmailOptInStatus { get; set; }

        [JsonProperty(PropertyName = "eReceiptPaperlessOptInStatus", NullValueHandling = NullValueHandling.Ignore)]
        public bool EReceiptPaperlessOptInStatus { get; set; }

    }

}