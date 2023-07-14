using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared.ResponseModels
{
    public class PharmacyResponse
    {
        [DataMember(Name = "PharmacyPhone", EmitDefaultValue = false)]
        public string PharmacyPhone { get; set; }
    }
}
