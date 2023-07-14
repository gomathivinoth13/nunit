using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExtensionsEETagRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Ignore)]
        public List<EETagData> items { get; set; }
    }

}
