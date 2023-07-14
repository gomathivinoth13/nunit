using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExtensionsProductSurveyRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Ignore)]
        public List<ProductSurveyItem> items { get; set; } = new List<ProductSurveyItem>();
    }
}
