using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PetClubChildItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "PetID", NullValueHandling = NullValueHandling.Ignore)]
        public string PetID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "member_id", NullValueHandling = NullValueHandling.Ignore)]
        public string member_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EnrollmentBanner", NullValueHandling = NullValueHandling.Ignore)]
        public string EnrollmentBanner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EnrollmentDate", NullValueHandling = NullValueHandling.Ignore)]
        public string EnrollmentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EnrollementSource", NullValueHandling = NullValueHandling.Ignore)]
        public string EnrollementSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "PetName", NullValueHandling = NullValueHandling.Ignore)]
        public string PetName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "PetTypeID", NullValueHandling = NullValueHandling.Ignore)]
        public string PetTypeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "PetTypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string PetTypeName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "LAST_UPDATE_DT", NullValueHandling = NullValueHandling.Ignore)]
        public string LAST_UPDATE_DT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "LAST_UPDATE_SOURCE", NullValueHandling = NullValueHandling.Ignore)]
        public string LAST_UPDATE_SOURCE { get; set; }

    }
}
