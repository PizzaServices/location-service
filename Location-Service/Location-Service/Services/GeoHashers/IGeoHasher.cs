namespace Location_Service.Services.GeoHashers;

public interface IGeoHasher
{
    string? Encode(double latitude, double longitude);
}