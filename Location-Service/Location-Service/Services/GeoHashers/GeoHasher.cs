namespace Location_Service.Services.GeoHashers;

public class GeoHasher :  Geohash.Geohasher, IGeoHasher
{
    public string? Encode(double latitude, double longitude)
    {
        return base.Encode(latitude, longitude, 7);
    }
}