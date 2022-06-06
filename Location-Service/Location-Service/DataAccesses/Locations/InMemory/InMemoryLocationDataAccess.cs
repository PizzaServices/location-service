using Location_Service.Containers;

namespace Location_Service.DataAccesses.Locations.InMemory;

public class LocationDataAccess : ILocationDataAccess
{
    private readonly ISearchTree<Guid, LocationRecord> _locationRecords;
    private readonly ISearchTree<string, List<Guid>> _hashBucket;

    public LocationDataAccess()
    {
        _locationRecords = new BinarySearchTree<Guid, LocationRecord>();
        _hashBucket = new BinarySearchTree<string, List<Guid>>();
    }
    
    // Read
    public LocationRecord? GetById(Guid id)
    {
        return _locationRecords.Get(id);
    }

    public List<LocationRecord> GetByRadius(double lat, double lon, int meters)
    {
        var hashes = ProximityHash.GetGeoHashRadiusApproximation(lat,
                                                                          lon,
                                                                          meters,
                                                                          9);

        return hashes
            .SelectMany(geoHash => _hashBucket.Get(geoHash) ?? new List<Guid>())
            .Select(id => _locationRecords.Get(id))
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
        
        _locationRecords.Insert(record.Id, record);
        
        if(_hashBucket.Contains(record.Hash))
            _hashBucket.Get(record.Hash)!.Add(record.Id);
        else
            _hashBucket.Insert(record.Hash, new List<Guid>{ record.Id });

        return record;
    }
    
    
}