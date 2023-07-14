using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class ArtWork
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "encodedImage", NullValueHandling = NullValueHandling.Ignore)]
        public string EncodedImage { get; set; }

        [JsonProperty(PropertyName = "imageUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }
    }
}
