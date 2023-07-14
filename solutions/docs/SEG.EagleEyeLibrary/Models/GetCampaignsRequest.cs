using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetCampaignsRequest
    {
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }

        [JsonProperty(PropertyName = "total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// /Query string for controlling Paginated Results Sorting Order.Please note this query string needs to be construted from field name + comma sign and sorting order(ASC/DESC). Currently AIR supports sorting by keys: accountId, validFrom, validTo, dateCreated, lastUpdated
        /// </summary>
        [JsonProperty(PropertyName = "orderBy", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> OrderBy { get; set; }

    }
}
