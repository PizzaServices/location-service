namespace Location_Service.DataAccesses.Locations.InMemory;

public class LocationDataAccess : ILocationDataAccess
{
    private readonly IList<LocationRecord> _locationRecords;

    public LocationDataAccess()
    {
        _locationRecords = new List<LocationRecord>();
    }
    
    // Read
    public LocationRecord? GetByHash(string hash)
    {
        return _locationRecords.FirstOrDefault(record => record.Hash == hash);
    }

    public LocationRecord? GetById(Guid id)
    {
        return _locationRecords.FirstOrDefault(record => record.Id == id);
    }

    public List<LocationRecord> GetByRadius(double lat, double lon, int meters)
    {
        var hashes = ProximityHash.GetGeoHashRadiusApproximation(lat,
                                                                          lon,
                                                                          meters,
                                                                          9);

        return hashes
            .Select(geoHash => _locationRecords
                .FirstOrDefault(record => record.Hash == geoHash))
            .Where(record => record != null)
            .ToList()!;
    }

    // Write
    public LocationRecord Create(string hash, double latitude, double longitude)
    {
        var record = new LocationRecord
        {
            Hash = hash,
            Id = Guid.NewGuid(),
            Latitude = latitude,
            Longitude = longitude,
        };
        
        _locationRecords.Add(record);

        return record;
    }
    
    
}