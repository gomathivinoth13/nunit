using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Product
    {

        [JsonProperty(PropertyName = "bilo", NullValueHandling = NullValueHandling.Ignore)]
        public Bilo Bilo { get; set; }


        [JsonProperty(PropertyName = "frescoymas", NullValueHandling = NullValueHandling.Ignore)]
        public Frescoymas Frescoymas { get; set; }


        [JsonProperty(PropertyName = "harveys", NullValueHandling = NullValueHandling.Ignore)]
        public Harveys Harveys { get; set; }


        [JsonProperty(PropertyName = "winndixie", NullValueHandling = NullValueHandling.Ignore)]
        public Winndixie Winndixie { get; set; }

    }
}
