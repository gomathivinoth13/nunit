using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SEG.StoreLocatorLibrary.Shared
{
    public class ZipcodeDetails
    {
        public ZipcodeDetails()
        {
            Location = new double[2];
        }


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //public ObjectId _id { get; set; }

        public string ZipCode { get; set; }
        public string CityStateName { get; set; }
        public string CityStateNameAbrv { get; set; }
        public string LastLineCity { get; set; }
        public string StateAbrv { get; set; }

        public double _latitude, _longitude;
        public double[] Location { get; set; }
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                Location[1] = value;
            }
        }

        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                Location[0] = value;
            }
        }
    }
}
