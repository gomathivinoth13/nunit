
using MongoDB.Driver.GeoJsonObjectModel;

namespace SEG.StoreLocatorLibrary.Shared
{
    public class Location
    {
        private double _latitude;
        private double _longitude;

        public int LocationTypeCode { get; set; }

        public string LocationTypeDescription { get; set; }

        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                Geo = GetGeo(_longitude, _latitude);
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value > 0 ? -value : value;
                Geo = GetGeo(_longitude, _latitude);
            }
        }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Geo { get; set; } = GetGeo(0, 0);


        private static GeoJsonPoint<GeoJson2DGeographicCoordinates> GetGeo(double longitude, double latitude) =>
            new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(longitude, latitude));

        //public override string ToString() =>
        //    $"{LocationTypeCode} {LocationTypeDescription} {Latitude} {Longitude}";
    }
}
