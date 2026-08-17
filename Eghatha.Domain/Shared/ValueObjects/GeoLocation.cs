using ErrorOr;
using Newtonsoft.Json;

namespace Eghatha.Domain.Shared.ValueObjects
{
    public record GeoLocation
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }


        [JsonConstructor]
        public GeoLocation(double latitude, double longitude)
        {
     

            Latitude = latitude;
            Longitude = longitude;
        }


       

        public static ErrorOr<GeoLocation> Create ( double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
              return  Error.Validation(
                    code:"GeoLocation.InvalidLatitude",
                    description:"Latitude must be between -90 and 90");

            if (longitude < -180 || longitude > 180)
            return  Error.Validation(
                  code: "GeoLocation.Invalidlongitude",
                  "Longitude must be between -180 and 180");

            return new GeoLocation(latitude, longitude);
        }
    }
}
