using Geohash;
using Location_Service.DataAccesses.Locations;

namespace Location_Service.Repositories.Locations;

public class LocationWriteRepository : ILocationWriteRepository
{
    private readonly ILocationDataAccess _dataAccess;

    public LocationWriteRepository(ILocationDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    
    public Location Create(string hash, double latitude, double longitude)
    {
        var record = _dataAccess.Create(hash, latitude, longitude);

        return new Location
        {
            Id = record.Id,
            Hash = hash,
            Latitude = latitude,
            Longitude = longitude,
        };
    }
}