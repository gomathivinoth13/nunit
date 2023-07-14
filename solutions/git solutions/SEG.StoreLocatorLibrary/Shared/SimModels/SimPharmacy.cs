using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared
{
    [DataContract]
    public class SimPharmacy
    {
        [DataMember(Name = "PharmacyPhone", EmitDefaultValue = false)]
        public string PharmacyPhone { get; set; }

        [DataMember(Name = "PharmacyHoursSun", EmitDefaultValue = false)]
        public string PharmacyHoursSun { get; set; }

        [DataMember(Name = "PharmacyHoursMon", EmitDefaultValue = false)]
        public string PharmacyHoursMon { get; set; }

        [DataMember(Name = "PharmacyHoursTue", EmitDefaultValue = false)]
        public string PharmacyHoursTue { get; set; }

        [DataMember(Name = "PharmacyHoursWed", EmitDefaultValue = false)]
        public string PharmacyHoursWed { get; set; }

        [DataMember(Name = "PharmacyHoursThu", EmitDefaultValue = false)]
        public string PharmacyHoursThu { get; set; }

        [DataMember(Name = "PharmacyHoursFri", EmitDefaultValue = false)]
        public string PharmacyHoursFri { get; set; }

        [DataMember(Name = "PharmacyHoursSat", EmitDefaultValue = false)]
        public string PharmacyHoursSat { get; set; }
    }
}
