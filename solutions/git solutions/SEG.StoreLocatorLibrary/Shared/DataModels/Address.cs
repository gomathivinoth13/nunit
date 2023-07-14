namespace SEG.StoreLocatorLibrary.Shared
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public override string ToString() =>
            $"{AddressLine1} {AddressLine2} {City} {State} {Zipcode} {County} {Country} {Latitude} {Longitude} ";
    }
}
