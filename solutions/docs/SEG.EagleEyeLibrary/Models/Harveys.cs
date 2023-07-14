using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Harveys
    {
        [JsonProperty(PropertyName = "collections", NullValueHandling = NullValueHandling.Ignore)]
        public List<Collection> Collections { get; set; }



    }
}
