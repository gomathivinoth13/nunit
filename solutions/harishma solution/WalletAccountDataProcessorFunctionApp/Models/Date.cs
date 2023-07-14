using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WalletAccountDataProcessorFunctionApp.Models
{
    public class Dates
    {
        [JsonPropertyName("start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime Start { get; set; }

        [JsonPropertyName("end")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime End { get; set; }

    }
}
