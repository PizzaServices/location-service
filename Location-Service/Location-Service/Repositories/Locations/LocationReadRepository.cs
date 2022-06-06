using Location_Service.DataAccesses.Locations;

namespace Location_Service.Repositories.Locations;

public class LocationReadRepository : ILocationReadRepository
{
    private readonly ILocationDataAccess _dataAccess;

    public LocationReadRepository(ILocationDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    
    public Location? Get(Guid id)
    {
        var record = _dataAccess.GetById(id);

        return ConvertRecord(record);
    }

    public IEnumerable<Location> GetByRadius(double lat, double lon, int meters)
    {
        return _dataAccess
                .GetByRadius(lat, lon, meters)
                .Select(ConvertRecord)
                .Where(record => record != null)
                .ToList()!;
    }

    private static Location? ConvertRecord(LocationRecord? record)
    {
        if (record == null)
            return null;
        
        return new Location
        {
            Hash = record.Hash,
            Id = record.Id,
            Latitude = record.Latitude,
            Longitude = record.Longitude,
        };
    }
}