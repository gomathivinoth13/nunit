using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetYTDPointSavingsResponse
    {
        [JsonProperty(PropertyName = "totalSavings", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalSavings { get; set; }
    }
}
