using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared
{
    [DataContract]
    public class Pharmacy
    {
        private List<PharmacyHours> pharmacyHrs;

        public static int pharmacyHrsAutoID = 1;

        [DataMember(Name = "PharmacyHours")]
        public List<PharmacyHours> PharmacyHours { get; set; } // TODO: Remove this line and above *************

        [DataMember(Name = "PharmacyPhone", EmitDefaultValue = false)]
        public string PharmacyPhone { get; set; }

    }
}
