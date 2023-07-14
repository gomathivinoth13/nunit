using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExtentionsPetClubChildRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Ignore)]
        public List<PetClubChildItem> items { get; set; }
    }
}
