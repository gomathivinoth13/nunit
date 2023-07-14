using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExtensionsEEUPCRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Ignore)]
        public List<EEUpcData> items { get; set; }
    }

}
