using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared.ResponseModels
{
    [DataContract]
    public class StoreAddressResponse
    {
        [DataMember(Name = "StoreCode", EmitDefaultValue = false)]
        public int StoreCode { get; set; }

        [DataMember(Name = "StoreName", EmitDefaultValue = false)]
        public string StoreName { get; set; }

        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public AddressResponse Address { get; set; } = new AddressResponse();

        [DataMember(Name = "Phone", EmitDefaultValue = false)]
        public string Phone { get; set; }
    }
}
