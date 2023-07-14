using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared
{
    public class MediaLink
    {
        [DataMember(Name = "Twitter", EmitDefaultValue = false)]
        public string Twitter { get; set; }

        [DataMember(Name = "Facebook", EmitDefaultValue = false)]
        public string Facebook { get; set; }

        [DataMember(Name = "Pintrest", EmitDefaultValue = false)]
        public string Pinterest { get; set; }

        [DataMember(Name = "Web", EmitDefaultValue = false)]
        public string Web { get; set; }
    }
}
