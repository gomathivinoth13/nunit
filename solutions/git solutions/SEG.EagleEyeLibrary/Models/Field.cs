using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Field
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "fieldData", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldData { get; set; }

        [JsonProperty(PropertyName = "fieldName", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "fieldType", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldType { get; set; }
    }
}
