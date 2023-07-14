using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Qualification
    {
        [JsonProperty(PropertyName = "product", NullValueHandling = NullValueHandling.Ignore)]
        public Product Product { get; set; }

    }
}
