using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetWalletIdentitiesResponse
    {

        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }

        [JsonProperty(PropertyName = "total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]

        public int Offset { get; set; }

        [JsonProperty(PropertyName = "filter", NullValueHandling = NullValueHandling.Ignore)]

        public List<string> Filter { get; set; }

        [JsonProperty(PropertyName = "orderBy", NullValueHandling = NullValueHandling.Ignore)]

        public List<OrderBy> OrderBy { get; set; }


        [JsonProperty(PropertyName = "results", NullValueHandling = NullValueHandling.Ignore)]

        public List<Identity> Results { get; set; }



    }
}
