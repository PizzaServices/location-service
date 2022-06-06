namespace Location_Service.DataAccesses.Locations;

public interface ILocationDataAccess
{
    // Read
    public LocationRecord? GetById(Guid id);
    public List<LocationRecord> GetByRadius(double lat, double lon, int meters);
    
    
    // Write
    public LocationRecord Create(double latitude, double longitude);
}