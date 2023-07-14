using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Details
    {
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "alternativeName", NullValueHandling = NullValueHandling.Ignore)]
        public string AlternativeName { get; set; }

        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "alternativeDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string AlternativeDescription { get; set; }

        [JsonProperty(PropertyName = "printerMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string PrinterMessage { get; set; }

        [JsonProperty(PropertyName = "screenMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ScreenMessage { get; set; }

        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "startDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        [JsonProperty(PropertyName = "mode", NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "actionType", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionType { get; set; }
    }
}
