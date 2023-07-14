 
using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared
{
    [DataContract]
    public class Timings
    {
        [DataMember(Name = "autoID", EmitDefaultValue = false)]
        public int AutoID { get; set; }

        [DataMember(Name = "STR_ID", EmitDefaultValue = false)]
        public int STR_ID { get; set; }

        [DataMember(Name = "Day", EmitDefaultValue = false)]
        public string STR_HRS_DY_NM { get; set; }

        [DataMember(Name = "StoreOpenTime", EmitDefaultValue = false)]
        public string STR_HRS_OPN_TM { get; set; }

        [DataMember(Name = "StoreCloseTime", EmitDefaultValue = false)]
        public string STR_HRS_CL_TM { get; set; }
    }
}

